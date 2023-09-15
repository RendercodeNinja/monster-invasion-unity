/// ------------------------------------------------------------------------------------
/// File: UIManager.cs
/// Project: Monster Invasion
/// Author: RendercodeNinja
/// Description: Game user interface elements animation and button click event handling
/// Target: UIManager
/// Instances: Single
/// ------------------------------------------------------------------------------------

using MonsterInvasion.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Inspector Fields    
    [Header("Labels"), Space(6)]
    // InGame score text object
    [SerializeField] private Text labelLevelScore;
    // Final score text object
    [SerializeField] private Text labelFinalScore;

    // UI Panels    
    [Header("Panels"), Space(6)]
    [SerializeField] private GameObject panelInGame;
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject panelHelp;
    #endregion

    #region Public Properties
    // Property - Set current level score
    public string LevelScore { set { labelLevelScore.text = "SCORE - " + value; } }
    // Property - Set game final score
    public string FinalScore { set { labelFinalScore.text = value; } }
    #endregion

    #region Private Properties
    // Reference to the Animator
    private Animator mAnimator;
    // Reference to the GameManager class
    private GameManager mGameManager;
    #endregion

    /// <summary>
    /// Unity Method - Awake
    /// </summary>
    void Awake()
    {
        // Cache reference to self components
        mAnimator = GetComponent<Animator>();

        // Cache references to other components
        mGameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Sets the current state for the UI
    /// </summary>
    /// <param name="state">Target state for UI</param>
    public void SetUIState(UIStates state)
    {
        switch (state)
        {
            // Show InGame panel
            case UIStates.InGame:
                ShowInGamePanel(); break;

            // Show GameOver panel
            case UIStates.GameOver:
                ShowGameOverPanel(); break;
        }
    }

    /// <summary>
    /// Set to show InGame panel
    /// </summary>    
    void ShowInGamePanel()
    {
        // Show InGame panel
        panelInGame.SetActive(true);

        // Hide GameOver panel       
        panelGameOver.SetActive(false);

        // Hide Help panel
        panelHelp.SetActive(false);
    }

    /// <summary>
    /// Set to show GameOver panel
    /// </summary>
    void ShowGameOverPanel()
    {
        // Show GameOver panel
        panelGameOver.SetActive(true);

        // Hide InGame panel        
        panelInGame.SetActive(false);

        // Hide Help panel
        panelHelp.SetActive(false);
    }

    #region Button click event recievers

    /// <summary>
    /// Gets invoked when Play button is clicked
    /// </summary>
    public void OnClickPlay()
    {
        // Call GameManager to start the game
        mGameManager.StartGame();

        // Set the UI to start game by animating the title and main menu buttons
        mAnimator.SetTrigger("StartGame");

        // Set the UI state to InGame
        SetUIState(UIStates.InGame);
    }

    /// <summary>
    /// Gets invoked when Retry button is clicked
    /// </summary>
    public void OnClickRetry()
    {
        // Call GameManager to restart the game
        mGameManager.RestartGame();

        // Set the UI state to InGame
        SetUIState(UIStates.InGame);
    }

    /// <summary>
    /// Gets invoked when Help button is clicked
    /// </summary>
    public void OnClickHelp() =>
        panelHelp.SetActive(true);

    /// <summary>
    /// Gets invoked when OK button within help menu is clicked
    /// </summary>
    public void OnClickHelpOK() =>
        panelHelp.SetActive(false);

    #endregion
}