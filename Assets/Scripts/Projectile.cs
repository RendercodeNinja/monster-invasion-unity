/// ---------------------------------------------------------------------
/// File: Projectile.cs
/// Project: Monster Invasion
/// Author: EverCG(Sethu)
/// Description: Single projectile object movement and collision detection
/// Target: LaserProjectile(Prefab)
/// Instances: Multiple
/// ---------------------------------------------------------------------

using MonsterInvasion.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Inspector Fields
    // Speed of the projectile
    [SerializeField] private float speed = 45.0f;
    #endregion 

    #region Private Properties
    // Reference to the Rigidbody2D
    private Rigidbody2D mRigidbody2D;
    // Reference to the ParticlePool class
    private ParticlePool mParticlePool;
    #endregion

    /// <summary>
    /// Unity Method - Awake
    /// </summary>
    private void Awake()
    {
        // Cache reference to self components
        mRigidbody2D = GetComponent<Rigidbody2D>();
        // Cache references to other components
        mParticlePool = FindObjectOfType<ParticlePool>();
    }

    /// <summary>
    /// Fires a single projectile from the provided transform point
    /// </summary>
    /// <param name="firePoint">Transform of the object from which firing should start</param>
    public void FireProjectile(Transform firePoint)
    {
        // Set the projectile to the firepoint
        transform.position = firePoint.position;
        // Set the orientation of the firepoint orientation
        transform.localRotation = firePoint.rotation;
        // Set the firing velocity when the projectile is activated
        mRigidbody2D.velocity = firePoint.up * speed;
    }

    /// <summary>
    /// Unity Method - OnTriggerEnter2D
    /// </summary>
    /// <param name="other">Collider of other object against which collision trigger passed</param>    
    void OnTriggerEnter2D(Collider2D other)
    {
        // If the hit object is a Monster
        if (other.gameObject.GetComponent<MonsterHealth>())
        {
            // Make call to the MonsterHealth to take damage
            other.gameObject.GetComponent<MonsterHealth>().TakeDamage();
            // Deactivate this projectile
            DeactivateProjectile();
        }
        else
        // If the hit object is a boundary or the Platform on top
        if (other.gameObject.CompareTag("Boundary") || other.gameObject.CompareTag("Platform"))
        {
            // Show particle at the hit position of the boundary
            mParticlePool.ShowParticle(ParticleTypes.ProjectileLost, transform.position + transform.up);
            // Deactivate the projectile
            DeactivateProjectile();
        }
    }

    /// <summary>
    /// Deactivates this projectile to reuse for next cycle
    /// </summary>    
    public void DeactivateProjectile()
    {
        // Reset velocity
        mRigidbody2D.velocity = Vector2.zero;
        // Disable the projectile object
        gameObject.SetActive(false);
    }

}
