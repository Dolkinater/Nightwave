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
    
    public delegate void OnChangeState(GameState newState);
    public static event OnChangeState OnChangeStateEvent;
    
    // For now before the enemy script is set up i setup the event in the game manager side 
    

    void Start()
    {
        ChangeState(GameState.MainMenu);
        
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
        
       
        
    }

    public void ChangeState(GameState newState)
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
            case GameState.Active:
                break;
        }
        
        OnChangeStateEvent?.Invoke(currentState);
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
            playerData.finalScore = playerData.tempScore;
            
            playerData.topRank = playerData.tempRank;
            LoadMenu();
        }
        
        
    }

    void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    void Pause()
    {
        Time.timeScale = 0;
    }
    
    
}
