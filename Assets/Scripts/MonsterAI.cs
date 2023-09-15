/// ---------------------------------------------------------------------
/// File: MonsterAI.cs
/// Project: Monster Invasion
/// Author: RendercodeNinja
/// Description: Handles monster character movements and animations
/// Target: MonsterPrefab
/// Instances: Multiple
/// ---------------------------------------------------------------------

using MonsterInvasion.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    #region Inspector Fields
    // Property - Score point for this monster kill
    [SerializeField] private int scorePoint = 1;
    #endregion

    #region Private Properties
    // Monster states
    private MonsterStates mMonsterState = MonsterStates.Inert;
    // Horizontal velocity
    private float mHorizontalVelocity;
    // Vertical velocity
    private float mVerticalVelocity;
    // Monster is alive or not
    private bool mIsAlive = true;
    // Position at which Monster drops from platform
    private Vector3 mPlatfromDropPos;
    // Reference to the Animator
    private Animator mAnimator;
    // Reference to the Rigidbody2D
    private Rigidbody2D mRigidbody2D;
    // Reference to the Collider
    private Collider2D mCollider2D;
    // Reference to the GameManager class
    private GameManager mGameManager;
    // Reference to the MonsterManager class
    private MonsterManager mMonsterManager;
    // Reference to the ParticlePool class
    private ParticlePool mParticlePool;
    #endregion

    /// <summary>
    /// Unity Method - Awake
    /// </summary>
    private void Awake()
    {
        // Cache reference to self components
        mAnimator = GetComponent<Animator>();
        mRigidbody2D = GetComponent<Rigidbody2D>();
        mCollider2D = GetComponent<Collider2D>();

        // Cache references to other components
        mGameManager = FindObjectOfType<GameManager>();
        mMonsterManager = FindObjectOfType<MonsterManager>();
        mParticlePool = FindObjectOfType<ParticlePool>();
    }

    /// <summary>
    /// Unity Method - OnEnable
    /// </summary>
    private void OnEnable()
    {
        // State the Monster is alive (Its Alive..! Its Alive...! :D )
        mIsAlive = true;

        // Get Random horizontal and vertical velocity for this Monster movement
        mHorizontalVelocity = GetRandomHorVelocity();
        mVerticalVelocity = GetRandomVertVelocity();

        // Set animation state to sliding
        mAnimator.SetTrigger("Slide");

        // Enable gravity
        SetGravity(true);

        // Start the FSM
        FSM();
    }

    /// <summary>
    /// Unity Method - OnDestroy
    /// </summary>
    private void OnDestroy()
    {
        // State the Monster is nolonger alive
        mIsAlive = false;
    }

    /// <summary>
    /// Unity Method - OnCollisionEnter2D
    /// </summary>    
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Set the FSM to PlatformMotion if this Monster landed on the Platform
        if (other.gameObject.CompareTag("Platform"))
            SetFSMPlatformMotion();

        // Set the FSM to FloorMotion if this Monster landed on the Floor        
        if (other.gameObject.CompareTag("Floor"))
            SetFSMFloorMotion();

        // Kill Player if this Monster made contact
        if (other.gameObject.CompareTag("Player"))
            KillPlayer();
    }

    /// <summary>
    /// Unity Method - OnTriggerEnter2D
    /// </summary>        
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kill Player if this Monster made contact
        if (other.gameObject.CompareTag("Player"))
            KillPlayer();
    }

    #region AI and State Machines

    /// <summary>
    /// Finite State Machine for the Monster character
    /// </summary>    
    private async void FSM()
    {
        // Loop until the Monster is alive
        while (mIsAlive)
        {
            switch (mMonsterState)
            {
                case MonsterStates.Inert:
                    // Nothing To Do
                    break;

                case MonsterStates.PlatformMotion:
                    // Perform top platform movement
                    FSMPlatformMotion();
                    break;

                case MonsterStates.DroppingDown:
                    // Perform dropdown from top platform
                    FSMDropDownMotion();
                    break;

                case MonsterStates.FloorMotion:
                    // Perform floor movement
                    FSMFloorMotion();
                    break;
            }

            await Task.Yield();
        }
    }

    /// <summary>
    /// Perform FSM movement on top platform 
    /// </summary>    
    private void FSMPlatformMotion()
    {
        // Get the distance between this Monster and the drop position on platform
        float dropToDistance = (transform.position - mPlatfromDropPos).magnitude;

        // If distance is reached
        if (dropToDistance <= 1f)
        {
            // Stop the horrizontal movement
            mRigidbody2D.velocity = Vector2.zero;
            // Set the state to drop down
            SetFSMDropDown();
        }
    }

    /// <summary>
    /// Perform FSM drop down movment
    /// </summary>    
    private void FSMDropDownMotion()
    {
        // If the enmy reached close to floor, enable back gravity for once
        if (mMonsterState != MonsterStates.FloorMotion && transform.position.y < -5f)
            SetGravity(true);
    }

    /// <summary>
    /// Perform FSM movment on bottom floor
    /// </summary>    
    private void FSMFloorMotion()
    {
        // Nothing To Do
    }

    /// <summary>
    /// Set new FSM state as PlatformMotion
    /// </summary>    
    private void SetFSMPlatformMotion()
    {
        // Ignore if already PlatformMotion
        if (mMonsterState == MonsterStates.PlatformMotion)
            return;

        // Calculate a radom position to drop from the Platform
        int X_Point = Random.Range(-mMonsterManager.DropRange, mMonsterManager.DropRange + 1);
        mPlatfromDropPos = transform.position;
        mPlatfromDropPos.x = X_Point;

        // Disable gravity for this Monster
        SetGravity(false);

        // If the drop position is to the right, move this Monster towards right
        if (X_Point > transform.position.x)
            mRigidbody2D.velocity = new Vector2(mHorizontalVelocity, 0);
        else // Else if the drop position is to the left, move this Monster towards left
            mRigidbody2D.velocity = new Vector2(-mHorizontalVelocity, 0);

        // Set the animation to sliding
        mAnimator.SetTrigger("Slide");

        // Set the FSM state to PlatformMotion
        mMonsterState = MonsterStates.PlatformMotion;
    }

    /// <summary>
    /// Set new FSM state as DropDown
    /// </summary>    
    private void SetFSMDropDown()
    {
        // Do nothing if already Drop down motion
        if (mMonsterState == MonsterStates.DroppingDown)
            return;

        // Set velocity to move downward
        mRigidbody2D.velocity = new Vector2(0, -mVerticalVelocity);

        // Set the animation to Falling
        mAnimator.SetTrigger("Fall");

        // Set the FSM to DropDownMotion
        mMonsterState = MonsterStates.DroppingDown;
    }

    /// <summary>
    /// Set new FSM state as FloorMotion
    /// </summary>    
    private void SetFSMFloorMotion()
    {
        // Do nothing if already floor motion
        if (mMonsterState == MonsterStates.FloorMotion)
            return;

        // Disable gravity
        SetGravity(false);

        // If the Monster landed on left of turret, move right else move left
        if (transform.position.x < 0)
            mRigidbody2D.velocity = new Vector2(mHorizontalVelocity, 0);
        else
            mRigidbody2D.velocity = new Vector2(-mHorizontalVelocity, 0);

        // Set the animation to sliding
        mAnimator.SetTrigger("Slide");

        // Set the FSM to Floor motion
        mMonsterState = MonsterStates.FloorMotion;
    }

    #endregion

    /// <summary>
    /// Gets invoked when this Monster's death occurs (from MonsterHealth)
    /// </summary>
    public void OnDeath()
    {
        // Create explosion at the current position
        mParticlePool.ShowParticle(ParticleTypes.ProjectileHit, transform.position);

        // Make call to game manager to increase the score        
        mGameManager.UpdateScore(scorePoint);

        // Reset this Monster
        Reset();
    }

    /// <summary>
    /// Gets invoked when this Monster makes contact with Player
    /// </summary>    
    public void KillPlayer()
    {
        // Make call to gamemanager stating game over
        mGameManager.OnGameOver();

        // Reset this Monster 
        Reset();
    }

    /// <summary>
    /// Reset this Monster's states for next cycle
    /// </summary>    
    public void Reset()
    {
        // State the Monster is no longer alive
        mIsAlive = false;

        // Stop any further movement of this Monster
        mRigidbody2D.velocity = Vector2.zero;

        // Disable gravity 
        SetGravity(false);

        // Deactivate this Monster object
        gameObject.SetActive(false);

        // Reset the postion back to root
        transform.localPosition = Vector3.zero;

        // Set state to inert
        mMonsterState = MonsterStates.Inert;
    }

    /// <summary>
    /// Set effect of gravity on this Monster
    /// </summary>  
    /// <param name="enable">Whether to enable or disable gravity</param>
    private void SetGravity(bool enable)
    {
        // Set gravity scale
        mRigidbody2D.gravityScale = enable ? 1 : 0;
        // Set kinematic
        mRigidbody2D.isKinematic = !enable;
        // Update collider
        mCollider2D.isTrigger = !enable;
    }

    #region Helper Functions

    /// <summary>
    /// Returns a random vertical velocity within the range from MonsterManager
    /// </summary>    
    public float GetRandomVertVelocity() => Random.Range(mMonsterManager.VerticalVelocity.min,
            mMonsterManager.VerticalVelocity.max);

    /// <summary>
    /// Returns a random horizontal velocity within the range from MonsterManager
    /// </summary>
    public float GetRandomHorVelocity() => Random.Range(mMonsterManager.HorizontalVelocity.min,
            mMonsterManager.HorizontalVelocity.max);

    #endregion
}

