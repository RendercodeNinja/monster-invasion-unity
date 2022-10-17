/// ---------------------------------------------------------------------
/// File: ParticlePool.cs
/// Project: Monster Invasion
/// Author: EverCG(Sethu)
/// Description: Resource pool handling system for Particle systems
/// Target: ParticlePool
/// Instances: Single
/// ---------------------------------------------------------------------

using MonsterInvasion.Generic;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    #region Inspector Fields
    // Projectile lost particle prefab
    [SerializeField] private ParticleSystem projectileLost;
    // Projectile lost particle pool size
    [SerializeField] private int pLPoolSize = 9;
    // Projectile hit particle prefab
    [SerializeField] private ParticleSystem projectileHit;
    // Projectile hit pool size
    [SerializeField] private int pHPoolSize = 5;
    // Player(Turret) explode prefab
    [SerializeField] private ParticleSystem playerExplode;
    // Player(Turret) explode pool size
    [SerializeField] private int pEPoolSize = 1;
    #endregion

    #region Private Properties
    // Pool which holds the projectile lost particles
    private readonly List<ParticleSystem> mPoolProjectileLost = new();
    // Pool which holds the projectile hit particles
    private readonly List<ParticleSystem> mPoolProjectileHit = new();
    // Pool whcih holds the player explode particles
    private readonly List<ParticleSystem> mPoolPlayerExplode = new();
    #endregion

    /// <summary>
    /// Unity Method - Start
    /// </summary>
    private void Start()
    {
        // Create the initial pool set
        CreateInitialPoolSet();
    }

    /// <summary>
    /// Create the initial poolset for each particle type
    /// </summary>
    void CreateInitialPoolSet()
    {
        // Create the projectile lost particle pool
        for (int i = 0; i < pLPoolSize; i++)
            CreateParticle(ParticleTypes.ProjectileLost);
        // Create the projectile hit particle pool
        for (int i = 0; i < pHPoolSize; i++)
            CreateParticle(ParticleTypes.ProjectileHit);
        // Create the player explode hit particle pool
        for (int i = 0; i < pEPoolSize; i++)
            CreateParticle(ParticleTypes.PlayerExplode);
    }

    /// <summary>
    /// Create a single ParticleSystem instance based on the type
    /// </summary>
    /// <param name="type">Type of the required particle</param>
    /// <returns></returns>
    ParticleSystem CreateParticle(ParticleTypes type)
    {
        switch (type)
        {
            // Particle - ProjectileLost
            default:
                {
                    // Create a particle object
                    ParticleSystem ps = Instantiate(projectileLost, transform.position, Quaternion.identity, transform);
                    // Remove clone from the name
                    ps.gameObject.name = projectileLost.gameObject.name;
                    // Add the newly created particle object to the pool list
                    mPoolProjectileLost.Add(ps);
                    // Return the particle system
                    return ps;
                }

            // Particle - ProjectileHit
            case ParticleTypes.ProjectileHit:
                {
                    // Create a particle object
                    ParticleSystem ps = Instantiate(projectileHit, transform.position, Quaternion.identity, transform);
                    // Remove clone from the name
                    ps.gameObject.name = projectileHit.gameObject.name;
                    // Add the newly created particle object to the pool list
                    mPoolProjectileHit.Add(ps);
                    // Return the particle system
                    return ps;
                }

            // Particle - PlayerExplode
            case ParticleTypes.PlayerExplode:
                {
                    // Create a particle object
                    ParticleSystem ps = Instantiate(playerExplode, transform.position, Quaternion.identity, transform);
                    // Remove clone from the name
                    ps.gameObject.name = playerExplode.gameObject.name;
                    // Add the newly created particle object to the pool list
                    mPoolPlayerExplode.Add(ps);
                    // Return the particle system
                    return ps;

                }
        }
    }

    /// <summary>
    /// Shows a particle at desired location based on the type choosen from pool
    /// </summary>
    /// <param name="type">Type of the particle to show</param>
    /// <param name="position">Position at which the particle should show</param>    
    public void ShowParticle(ParticleTypes type, Vector3 position)
    {
        switch (type)
        {
            // Particle - ProjectileLost
            default:
                {
                    // Loop through each particle in the pool
                    foreach (ParticleSystem ps in mPoolProjectileLost)
                    {
                        // If the particle is not playing
                        if (!ps.isPlaying)
                        {
                            // Place the particle at the position, play and return
                            ps.transform.position = position; ps.Play(); return;
                        }
                    }

                    // If no free particle found, create a new particle to pool
                    ParticleSystem newps = CreateParticle(ParticleTypes.ProjectileLost);
                    // Place the particle at the position and play
                    newps.transform.position = position; newps.Play(); break;
                }

            // Particle - ProjectileHit
            case ParticleTypes.ProjectileHit:
                {
                    // Loop through each particle in the pool
                    foreach (ParticleSystem ps in mPoolProjectileHit)
                    {
                        // If the particle is not playing
                        if (!ps.isPlaying)
                        {
                            // Place the particle at the position, play and return
                            ps.transform.position = position; ps.Play(); return;
                        }
                    }

                    // If no free particle found, create a new particle to pool
                    ParticleSystem newps = CreateParticle(ParticleTypes.ProjectileHit);
                    // Place the particle at the position and play
                    newps.transform.position = position; newps.Play(); break;
                }

            // Particle - PlayerExplode
            case ParticleTypes.PlayerExplode:
                {
                    // Loop through each particle in the pool
                    foreach (ParticleSystem ps in mPoolPlayerExplode)
                    {
                        // If the particle is not playing
                        if (!ps.isPlaying)
                        {
                            //Place the particle at the position, play and return
                            ps.transform.position = position; ps.Play(); return;
                        }
                    }

                    // If no free particle found, create a new particle to pool
                    ParticleSystem newps = CreateParticle(ParticleTypes.PlayerExplode);
                    // Place the particle at the position and play
                    newps.transform.position = position; newps.Play(); break;
                }
        }
    }
}
