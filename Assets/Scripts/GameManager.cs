/// ---------------------------------------------------------------------
/// File: GameManager.cs
/// Project: Monster Invasion
/// Author: EverCG(Sethu)
/// Description: Game state management for MonsterInvasion
/// Target: GameArena
/// Instances: Single
/// ---------------------------------------------------------------------

using MonsterInvasion.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Public Properties
    // Enumerator stating current game states    
    public GameStates GameState { get; private set; }
    #endregion

    #region Private Properties
    // Current score of the player
    private int mScore = 0;
    // Reference to the animator components
    private Animator mAnimator;
    // Reference to the MonsterManager class
    private MonsterManager mMonsterManager;
    // Reference to the ParticlePool class
    private ParticlePool mParticlePool;
    // Reference to the UIManager class
    private UIManager mUIManager;
    // Reference to the UIManager class
    private CameraShake mCameraShake;
    #endregion

    /// <summary>
    /// Unity Method - Awake
    /// </summary>
    private void Awake()
    {
        // Cache reference to self components
        mAnimator = GetComponent<Animator>();

        // Cache references to other components
        mMonsterManager = FindObjectOfType<MonsterManager>();
        mParticlePool = FindObjectOfType<ParticlePool>();
        mUIManager = FindObjectOfType<UIManager>();
        mCameraShake = FindObjectOfType<CameraShake>();
    }

    /// <summary>
    /// Starts the game on invocation
    /// </summary>
    public void StartGame()
    {
        // Make call to the animator to play the intro animation
        mAnimator.SetTrigger("Initiate");
    }

    /// <summary>
    /// Restarts the game on invocation
    /// </summary>
    public void RestartGame()
    {
        // Reset the score value to zero
        mScore = 0;

        // Update this score to the ingame score label
        mUIManager.LevelScore = mScore.ToString();

        // Reset and play the initiation animation
        mAnimator.SetTrigger("Restart");
    }

    /// <summary>
    /// Gets invoked when a GameOver occurs (from MonsterAI) 
    /// </summary>
    public async void OnGameOver()
    {
        // Reset all enemies to startup state
        mMonsterManager.Reset();

        // Reset the GameState to non playable state
        GameState = GameStates.NonPlayable;

        // Update the final score to the final score label
        mUIManager.FinalScore = mScore.ToString();

        // Show the player explosin
        mParticlePool.ShowParticle(ParticleTypes.PlayerExplode, new Vector3(0, -7.75f, 0));

        // Shake the camera
        mCameraShake.Shake();

        // Wait for a second
        await Task.Delay(1000);

        // Show the game over screen
        mUIManager.SetUIState(UIStates.GameOver);
    }

    /// <summary>
    /// Updates the player score by value (from MonsterAI)
    /// </summary>
    /// <param name="value">Score incriment value</param>
    public void UpdateScore(int value)
    {
        // Update the score integer
        mScore += value;

        // Update this score to the ingame score label
        mUIManager.LevelScore = mScore.ToString();
    }

    #region Animator Event Recievers

    /// <summary>
    /// Animation event triggers when game initiation is done
    /// </summary>
    public void OnGameReady()
    {
        // Set the game to playable state
        GameState = GameStates.Playable;
    }

    #endregion
}
