/// ---------------------------------------------------------------------
/// File: EnergyGenerator.cs
/// Project: Monster Invasion
/// Author: RendercodeNinja
/// Description: Handles energy generation for turret system. Continous
///              usage of the turret depletes generator energy and the
///              player has to wait for energy to regenerate
/// Target: Generators
/// Instances: Single
/// ---------------------------------------------------------------------

using System.Collections;
using UnityEngine;

public class EnergyGenerator : MonoBehaviour
{
    #region Inspector Fields
    // Number of shots an energy bar can deliver
    [SerializeField] private int shotsPerEnergyBar = 1;
    // Energy bar sprites transforms on the left
    [SerializeField] private Transform[] energyBarsLeft;
    // Energy bar sprites transforms on the right
    [SerializeField] private Transform[] energyBarsRight;
    #endregion

    #region Public Properties
    // Whether the generator is overheated or not
    public bool IsGeneratorOverHeated { get; private set; } = false;
    #endregion

    #region Private Properties    
    // Counter for shots to match shotsPerEnergyBar
    private int mShotCounter = 0;
    // Current index of the energy bar
    private int mEnergyIndex = 3;
    // Whether generator is in normal state or not
    private bool mGeneratorNormal = true;
    // Whether the generator is restoring after an overheat
    private bool mGeneratorRestoring = false;
    #endregion

    /// <summary>
    /// Gets invoked when trigger is held hold down (From TurretSystem) 
    /// </summary>
    public void OnTriggerHoldDown()
    {
        // Reduce the generator energy
        ReduceGeneratorEnergy();

        // Stop any ongoing the restoration
        StopCoroutine(RestoreGenerator());

        // State generator no longer restoring
        mGeneratorRestoring = false;
    }

    /// <summary>
    /// Invoked when firing trigger is released (From TurretSystem)
    /// </summary>
    public void OnTriggerReleased()
    {
        // Reset shot counter
        mShotCounter = 0;

        // Start the coroutine to restore the generator
        if (!mGeneratorRestoring && !mGeneratorNormal)
        {
            // Start generator restoration
            StartCoroutine(RestoreGenerator());

            // State generator is restoring
            mGeneratorRestoring = true;
        }
    }

    /// <summary>
    /// Method to reduce the generator energy
    /// </summary>
    private void ReduceGeneratorEnergy()
    {
        // Update the shotscounter until it reaches ShotsPerEnergyBar
        if (mShotCounter < shotsPerEnergyBar)
        {
            // Update the shot counter
            mShotCounter++;
        }
        else
        {
            // Decrement energy bar by one
            DecrementEnergyBar();

            // State generator is not in normal since atleast one energybar went down
            mGeneratorNormal = false;

            // Reset the counter
            mShotCounter = 0;
        }
    }

    /// <summary>
    /// Method to decrement EnergyBars
    /// </summary>
    private void DecrementEnergyBar()
    {
        // Disable the enery bar at index on the left
        energyBarsLeft[mEnergyIndex].gameObject.SetActive(false);
        // Disable the enery bar at index on the right
        energyBarsRight[mEnergyIndex].gameObject.SetActive(false);

        // Decrement the index
        if (mEnergyIndex > 0)
            mEnergyIndex--;
        // State generator over heated
        else if (mEnergyIndex == 0)
            IsGeneratorOverHeated = true;
    }

    /// <summary>
    /// Method to incriment EnergyBars
    /// </summary>
    private void IncrimentEnergyBar()
    {
        // Enable the energybar at the index on left
        energyBarsLeft[mEnergyIndex].gameObject.SetActive(true);
        // Enable the energybar at the index on right
        energyBarsRight[mEnergyIndex].gameObject.SetActive(true);

        // Incriment the index
        if (mEnergyIndex < 3)
            mEnergyIndex++;
        // State generator operation normal
        else if (mEnergyIndex == 3)
            mGeneratorNormal = true;
    }

    /// <summary>
    /// Coroutine to restore the generator
    /// </summary>    
    private IEnumerator RestoreGenerator()
    {
        // If generator is not in normal
        while (!mGeneratorNormal)
        {
            // Incriment Energybar by one
            IncrimentEnergyBar();

            // Wait half a second
            yield return new WaitForSeconds(0.25f);
        }

        // State generator is no longer overheated
        IsGeneratorOverHeated = false;
    }
}
