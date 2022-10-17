/// ---------------------------------------------------------------------
/// File: MonsterManager.cs
/// Project: Monster Invasion
/// Author: EverCG(Sethu)
/// Description: Handles monster character pool and spawning
/// Target: MonsterManager
/// Instances: Multiple
/// ---------------------------------------------------------------------

using MonsterInvasion.Generic;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    #region Inspector Fields
    [Header("Monster Prefabs"), Space(6)]
    // Monster Prefabs
    [SerializeField] private GameObject blueMonster;
    [SerializeField] private GameObject redMonster;

    [Header("Monster Movement & Drops"), Space(6)]
    // Range -x to x the monster can access and drop from the platform
    [SerializeField] private int dropRange = 12;
    // Initial horizontal velocity range
    [SerializeField] private RangeF initialHorizontalVelocity = new() { min = 6, max = 8 };
    // Initial vertical velocity range
    [SerializeField] private RangeF initialVerticalVelocity = new() { min = 3, max = 5 };

    [Header("Monster Spawning"), Space(6)]
    // Time duration between spawns
    [SerializeField] private RangeF spawnDuration = new() { min = 1.5f, max = 2.0f };
    // Array holding the spawn points
    [SerializeField] private Transform[] spawnPoints;
    #endregion

    #region Public Properties
    // Horizontal drop range from center
    public int DropRange { get => dropRange; }
    // Horizontal velocity range
    public RangeF HorizontalVelocity { get; private set; }
    // Vertical velocity range
    public RangeF VerticalVelocity { get; private set; }
    #endregion

    #region Private Properties
    // Pool list which holds the monster character objects
    private List<GameObject> mBlueMonsterPool = new();
    private List<GameObject> mRedMonsterPool = new();
    // Pool size for the monsters
    private readonly int mBluePoolSize = 10, mRedPoolSize = 5;
    // Spawn timer
    private float mSpawnTimer;
    // Time for next spawn
    private float mNextSpawnTime;
    // Counter for blue monster until a red monster is spawned
    private int m_MonsterCounter_Red;
    // Threshold for next red monster
    private int mNextRedMonster;
    private int mMonsterCounterSpeed;
    // Counter for swpawntime updation
    private int mSpawnTimeCounter;
    // Reference to the GameManager class
    private GameManager mGameManager;
    #endregion

    /// <summary>
    /// Unity Method - Awake
    /// </summary>
    void Awake()
    {
        // Cache references to other components
        mGameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Unity Method - Start
    /// </summary>
    void Start()
    {
        // Perform a reset
        Reset();

        // Create the inital set of monsters
        CreateInitialSet();
    }

    /// <summary>
    /// Unity Method - Update
    /// </summary>
    void Update()
    {
        // If the game is in playable state
        if (mGameManager.GameState == GameStates.Playable)
        {
            // Keep updating the spawn timer
            mSpawnTimer += Time.deltaTime;

            // If the timer reached next spawn time
            if (mSpawnTimer > mNextSpawnTime)
            {
                //Spawn a monster
                SpawnMonster();

                //Reset the spawn timer
                mSpawnTimer = 0;
            }
        }
    }

    /// <summary>
    /// Spawns a moster to the platform
    /// </summary>    
    private void SpawnMonster()
    {
        // If counter not reached to drop next red monster
        if (m_MonsterCounter_Red < mNextRedMonster)
        {
            // Drop a blue monster
            DropMonster(MonsterColor.Blue);

            // Update Blue monster counter
            m_MonsterCounter_Red++;
        }
        else
        {
            // If counter reached to drop a red monster, drop one
            DropMonster(MonsterColor.Red);            

            // Reset blue monster counter
            m_MonsterCounter_Red = 0;

            // Set threshold for next red monster
            mNextRedMonster = Random.Range(3, 10);
        }

        // Update the monster counter for speed updation
        mMonsterCounterSpeed++;

        //If monster counter for speed reached threshold
        if (mMonsterCounterSpeed > 9)
        {
            // Update till near 5
            if (VerticalVelocity.min < 5)
                VerticalVelocity.min += 0.25f;

            // Update till near 8
            if (VerticalVelocity.max < 9)
                VerticalVelocity.max += 0.25f;

            // Reset monster counter for speed
            mMonsterCounterSpeed = 0;
        }

        // Keep updating spawn time counter
        mSpawnTimeCounter++;

        if (mSpawnTimeCounter > 50)
        {
            // Decriment next spawn time with keeping above 1.5f
            if (mNextSpawnTime > spawnDuration.min)
                mNextSpawnTime -= 0.1f;

            // Reset spawn time counter
            mSpawnTimeCounter = 0;
        }
    }

    /// <summary>
    /// Drops a blue monster at the spawn point
    /// </summary>    
    void DropMonster(MonsterColor color)
    {
        // Get a monster from pool based on color
        GameObject monster = GetMonster(color);
        // Get a random spawnpoint's position
        Vector3 spawnPos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        // Set the monster to the position 
        monster.transform.position = spawnPos;
        // Activate the monster
        monster.SetActive(true);
    }

    #region Monster Pooling Code

    /// <summary>
    /// Create the initial set of monsters
    /// </summary>
    private void CreateInitialSet()
    {
        // Loop through pool count to create blue monsters
        for (int i = 0; i < mBluePoolSize; i++)
            CreateMonster(MonsterColor.Blue);

        // Loop through pool count to create red monsters
        for (int i = 0; i < mRedPoolSize; i++)
            CreateMonster(MonsterColor.Red);
    }

    /// <summary>
    /// Creates a new monster based on requested color into the pool
    /// </summary>
    /// <param name="color">Color enum of the monster required</param>
    /// <returns></returns>
    GameObject CreateMonster(MonsterColor color)
    {
        // Instantiate monster based on color
        GameObject monster = Instantiate(color == MonsterColor.Red ? redMonster : blueMonster,
            transform.position, Quaternion.identity);
        // Root it to this object
        monster.transform.SetParent(transform);
        // Remove 'clone' from the monster name
        monster.name = color == MonsterColor.Red ? redMonster.name : blueMonster.name;
        // Keep the monster object deactivated
        monster.SetActive(false);
        // Add the new monster to the pool list
        if (color == MonsterColor.Red)
            mRedMonsterPool.Add(monster);
        else
            mBlueMonsterPool.Add(monster);
        // Return the monster
        return monster;
    }

    /// <summary>
    /// Get a monster based on type from corresponding pool
    /// </summary>
    /// <param name="color">Color enum of the monster required</param>
    /// <returns></returns>
    GameObject GetMonster(MonsterColor color)
    {
        // Get the corresponding monster pool
        List<GameObject> targetPool = (color == MonsterColor.Red) ? mRedMonsterPool : mBlueMonsterPool;

        // Iterate through the monster pool
        foreach (GameObject monster in targetPool)
        {
            // If the  monster is not active, return it
            if (!monster.activeSelf)
                return monster;
        }

        // If no free monsters are found, create one and return
        return CreateMonster(color);
    }

    #endregion

    /// <summary>
    /// Reset all monsters
    /// </summary>    
    public void Reset()
    {
        // Reset all blue monsters
        foreach (GameObject monster in mBlueMonsterPool)
            monster.GetComponent<MonsterAI>().Reset();

        // Reset all red monster
        foreach (GameObject monster in mRedMonsterPool)
            monster.GetComponent<MonsterAI>().Reset();

        // Reset horizontal and vertical velocities to inital velocity
        HorizontalVelocity = initialHorizontalVelocity;
        VerticalVelocity = initialVerticalVelocity;

        // Reset counters
        m_MonsterCounter_Red = 0;
        mNextRedMonster = Random.Range(6, 10);
        mMonsterCounterSpeed = 0;
        mSpawnTimeCounter = 0;
        mNextSpawnTime = spawnDuration.max;
    }
}
