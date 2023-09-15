/// ---------------------------------------------------------------------
/// File: ProjectilePool.cs
/// Project: Monster Invasion
/// Author: RendercodeNinja
/// Description: Resource pool handling system for Projectile objects
/// Target: ProjectilePool
/// Instances: Single
/// ---------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    #region Inspector Fields
    // Projectile prefab object
    [SerializeField] private GameObject projectilePrefab;
    // Startup size of the projectile pool
    [SerializeField] private int poolStartSize = 5;
    #endregion

    #region Private Properties
    // List to track the projectile objects
    private readonly List<Projectile> mProjectilePool = new();
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
    /// Creates the initial pool set
    /// </summary>
    private void CreateInitialPoolSet()
    {
        // Create the projectiles upto pool start size
        for (int i = 0; i < poolStartSize; i++)
            CreateProjectile();
    }

    /// <summary>
    /// Creates a single Projectile instance, add to the pool and return the object
    /// </summary>
    /// <returns>Returns resultant Projectile object</returns>
    private Projectile CreateProjectile()
    {
        // Instantiate a new projectile object
        Projectile projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
        // Rename to remove 'clone' part
        projectile.name = projectilePrefab.name;
        // Root it to this object
        projectile.transform.parent = this.transform;
        // Keep the projectile deactivated
        projectile.gameObject.SetActive(false);
        // Add this projectile to the pool list
        mProjectilePool.Add(projectile);
        // Return the projectile object
        return projectile;
    }

    /// <summary>
    /// Get a free projectile object from the pool
    /// </summary>
    /// <returns>Returns resulant Projectile object</returns>
    public Projectile GetProjectile()
    {
        // Loop through each object in the pool list
        foreach (Projectile projectile in mProjectilePool)
        {
            // If this object is not active, return it
            if (!projectile.gameObject.activeSelf)
                return projectile;
        }

        // If no projectile is found free in the pool, create a new one and return
        return CreateProjectile();
    }

    /// <summary>
    /// Resets the projectile pool
    /// </summary>
    public void Reset()
    {
        // Loop through all projectile objects
        foreach (Projectile proj in mProjectilePool)
        {
            // Deactivate the projectile object
            proj.DeactivateProjectile();
            // Set it to root position 
            proj.transform.localPosition = Vector3.zero;
        }
    }
}
