using System;
using System.Collections.Generic;
using UnityEngine;

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
    
    public PlayerController playerController;
    
    // For now before the enemy script is set up i setup the event in the game manager side 

    void Start()
    {
        //Level manager Script sends the event that its started
        playerController = FindObjectOfType<PlayerController>();
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
    
    
    
}
