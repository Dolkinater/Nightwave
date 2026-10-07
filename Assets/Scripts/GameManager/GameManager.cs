using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Paused, MainMenu, Win, Lose, Active }

public class GameManager : MonoBehaviour
{
    
    #region SubManagers
    
    private LevelManager levelManager;
    public LevelManager _LevelManager => levelManager;
    
    private ScoreManager scoreManager;
    public ScoreManager _ScoreManager => scoreManager;
    
    #endregion
    // Singleton Instance
    public static GameManager instance { get; private set; }
    
    public GameState currentState;
    
    public PlayerData playerData;
    
    // For now before the enemy script is set up i setup the event in the game manager side 
    

    void Start()
    {
        //Level manager Script sends the event that its started
        
    }
    

    // Enforcement of Singleton instance
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
        
       // LevelManager.OnLevelQuit +=
        
    }

    void ChangeState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.Paused:
                Pause();
                break;
            case GameState.MainMenu:
                LoadMenu();
                break;
            case GameState.Win:
                LoadMenu();
                break;
        }
    }

    void Exit(bool completedLevel)
    {
        if (!completedLevel)
        {
            currentState = GameState.MainMenu;
           // SceneManager.LoadScene("MainMenu");
        }
        else
        {
            
        }
    }

    void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void Pause()
    {
        Time.timeScale = 0;
    }
    
    
}
