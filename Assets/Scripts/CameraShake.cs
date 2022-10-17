/// ---------------------------------------------------------------------
/// File: CameraShake.cs
/// Project: Monster Invasion
/// Author: EverCG(Sethu)
/// Description: Helper script to create a simple camera shake effect with provided intensity and duration
/// Target: MainCamera
/// Instances: Single
/// ---------------------------------------------------------------------

using System.Threading.Tasks;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    #region Inspector Fields
    // Property - Shake Duration
    [SerializeField] private float duration = 0.5f;
    // Property - Shake Intensity
    [SerializeField] float intensity = 0.25f;
    #endregion

    #region Private Properties
    // Camera start position 
    private Vector3 startPos;
    #endregion

    /// <summary>
    /// Unity Method - Awake
    /// </summary>
    void Awake()
    {
        // Get camera start position
        startPos = transform.position;
    }

    /// <summary>
    /// Perform a camera shake effect
    /// </summary>    
    public async void Shake()
    {
        await ShakeEffect(duration, intensity);
    }

    /// <summary>
    /// Asynchronous method to perform a shake effect
    /// </summary>
    /// <param name="duration">Shake effect duration</param>
    /// <param name="intensity">Shake effect intensity</param>        
    private async Task ShakeEffect(float duration, float intensity)
    {
        // Time since start of shake
        float elapsedTime = 0.0f;

        // Reset camera position
        transform.position = startPos;

        // Loop until elapsed time reaches its duration
        while (elapsedTime < duration)
        {
            // Update elapsed time
            elapsedTime += Time.deltaTime;

            // Shake completion percentage
            float progress = elapsedTime / duration;

            // Calculate damping value
            float dampness = 1.0f - Mathf.Clamp(4.0f * progress - 3.0f, 0.0f, 1.0f);

            // Map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;

            // Apply damping
            x *= intensity * dampness; y *= intensity * dampness;

            // Create shake offset vector
            Vector3 offset = new Vector3(x, y, startPos.z);

            // Apply the shake value to the camera transform
            transform.position = startPos + offset;

            // Wait frame
            await Task.Yield();
        }

        // Apply back the original position
        Camera.main.transform.position = startPos;
    }
}
