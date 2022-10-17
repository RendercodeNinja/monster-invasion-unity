/// ---------------------------------------------------------------------
/// File: MonsterHealth.cs
/// Project: Monster Invasion
/// Author: EverCG(Sethu)
/// Description: Handles Monster character health and damages
/// Target: MonsterPrefab
/// Instances: Multiple
/// ---------------------------------------------------------------------

using MonsterInvasion.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    #region Inspector Fields
    // Monster maximum health
    [SerializeField] private int maxHealth = 1;
    #endregion

    #region Private Properties
    // Monster current health
    private int mHealth;
    // Reference to the MonsterAI component on this Monster
    private MonsterAI mMonsterAI;
    // Reference to the ParticlePool class
    private ParticlePool mParticlePool;
    #endregion

    /// <summary>
    /// Unity Method - Awake
    /// </summary>
    private void Awake()
    {
        // Cache reference to self components
        mMonsterAI = GetComponent<MonsterAI>();

        // Cache references to other components
        mParticlePool = FindObjectOfType<ParticlePool>();
    }

    /// <summary>
    /// Invoking this method will make the Monster take damage
    /// </summary>    
    public void TakeDamage()
    {
        // Decrement the health
        mHealth--;

        // Check if health is empty and invoke OnDeath if health falls below zero
        if (mHealth <= 0)
            mMonsterAI.OnDeath();
        // Show projectile hit particle
        else
            mParticlePool.ShowParticle(ParticleTypes.ProjectileLost, transform.position);
    }

    /// <summary>
    /// Unity Method - OnEnable
    /// </summary>    
    void OnEnable()
    {
        // Reset Monster health to max
        mHealth = maxHealth;
    }
}
