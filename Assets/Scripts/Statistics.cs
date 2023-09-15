/// ------------------------------------------------------------------------------------
/// File: Statistics.cs
/// Project: Monster Invasion
/// Author: RendercodeNinja
/// Description: Helper to display statistics information as an overlay on shortcut press
/// Target: UIManager
/// Instances: Single
/// ------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    // Statistics UI panel
    [SerializeField] private GameObject panelStatistics;
    // FrameBuffer text object
    [SerializeField] private Text labelFrameBuffer;
    // FPS text object
    [SerializeField] private Text labelFPS;

    // Whether the Statistics panel is visible or not
    private bool mIsStisticsVisible = false;
    // Delta time for FPS calculation
    private float deltaTime = 0.0f;

    /// <summary>
    /// Unity Method - Update
    /// </summary>
    void Update()
    {
        // If Shift + S + H is pressed, toggle Statistics panel
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S) && Input.GetKeyUp(KeyCode.H))
        {
            // Toogle statisctics visible
            mIsStisticsVisible = !mIsStisticsVisible;
            // Make the Statistics panel visible
            panelStatistics.SetActive(mIsStisticsVisible);
        }

        // Ignore if Statistics panel not visible
        if (!mIsStisticsVisible)
            return;

        // Display real-time FPS
        labelFPS.text = $"FPS - {GetFPS()}";
        // Display real-time screen size
        labelFrameBuffer.text = $"Frame Buffer - {Screen.width} X {Screen.height}";
    }

    /// <summary>
    /// Compute and return real-time render frame rate as string
    /// </summary>
    string GetFPS()
    {
        // Keep updating the delta time
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        // Calculate the fps
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;

        // Compose and return the string
        return string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
    }
}
