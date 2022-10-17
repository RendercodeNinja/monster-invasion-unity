/// ---------------------------------------------------------------------
/// File: TurretSystem.cs
/// Project: Monster Invasion
/// Author: EverCG(Sethu)
/// Description: Handles turret turning and projectile firing based on input
/// Target: TurretSystem
/// Instances: Single
/// ---------------------------------------------------------------------

using MonsterInvasion.Generic;
using UnityEngine;

public class TurretSystem : MonoBehaviour
{
    #region Inspector Fields
    [Header("Turret Controls"), Space(6)]
    // Turret Barrel
    [SerializeField] private Transform turretBarrel;
    // Speed at which turret turns
    [SerializeField] private float turnSpeed = 12.5f;
    // Turret rotation limit in degrees
    [SerializeField] private float turretFOV = 75.0f;
    [Header("Turret Firing"), Space(6)]
    // Turret Firing point
    [SerializeField] private Transform firePoint;
    // Time interval between consecutive firings
    [SerializeField] private float fireRate = 0.2f;    
    #endregion

    #region Private Properties
    // Threshold limit for holding down
    private readonly float mHoldDownThreshold = 0.02f;
    // Turret current rotation value
    private float mTurretRotation = 0;
    // Time variable for controling fire
    private float mNextFire;
    // Timer to calculate hold down
    private float mHoldDownTimer;
    // Reference to the GameManager class
    private GameManager mGameManager;
    // Reference to the EnergyGenerator class
    private EnergyGenerator mEnergyGenerator;
    // Reference to the EnergyGenerator class
    private ProjectilePool mProjectilePool;
    #endregion

    /// <summary>
    /// Unity Method - Awake
    /// </summary>
    private void Awake()
    {
        // Cache references to other components
        mGameManager = FindObjectOfType<GameManager>();
        mEnergyGenerator = FindObjectOfType<EnergyGenerator>();
        mProjectilePool = FindObjectOfType<ProjectilePool>();
    }

    /// <summary>
    /// Unity Method - Update
    /// </summary>    
    private void Update()
    {
        // Update the turret turning and shooting if game is in playable state
        if (mGameManager.GameState == GameStates.Playable)
        {
            // Perform turret turning
            HandleTurretTurn();

            // Perform turret shooting
            HandleTurretShoot();
        }
    }

    /// <summary>
    /// Handles turret turning based on user input
    /// </summary>    
    private void HandleTurretTurn()
    {
        // Update the rotation value from the input axis
        mTurretRotation += Input.GetAxis("Horizontal") * turnSpeed * 10 * Time.deltaTime;
        // Clamp the turret rotation within the positive and negative limit
        mTurretRotation = Mathf.Clamp(mTurretRotation, -turretFOV, turretFOV);
        // Apply the final rotation to the turret barrel
        turretBarrel.rotation = Quaternion.Euler(0, 0, -mTurretRotation);
    }

    /// <summary>
    /// Handles turret projectile shooting based on user input
    /// </summary>   
    private void HandleTurretShoot()
    {
        // If the mouse button or space bar is held down
        if (Input.GetButton("Fire") && Time.time > mNextFire && !mEnergyGenerator.IsGeneratorOverHeated)
        {
            // Shoot a projectile
            ShootProjectile();

            // Keep updating the hold down timer
            mHoldDownTimer += Time.deltaTime;

            // If timer reached above threshold, inform generator that tigger is being held down
            if (mHoldDownTimer >= mHoldDownThreshold)
                mEnergyGenerator.OnTriggerHoldDown();
        }

        // Reset the hold down timer when button is released
        if (Input.GetButtonUp("Fire"))
        {
            // Reset Hold down timer
            mHoldDownTimer = 0;

            // Tell generator that trigger is released
            mEnergyGenerator.OnTriggerReleased();
        }

    }

    /// <summary>
    /// Method to shoot a single projectile from pool at the fire point
    /// </summary>    
    private void ShootProjectile()
    {
        // Get a projectile from the pool as Projectile object
        Projectile projectile = mProjectilePool.GetProjectile();
        // Activate the projectile               
        projectile.gameObject.SetActive(true);
        // Fire the projectile
        projectile.FireProjectile(firePoint);
        // Calculate next firing time
        mNextFire = Time.time + fireRate;
    }
}
