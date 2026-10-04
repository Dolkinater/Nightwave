using System;
using System.Collections.Generic;
using UnityEngine;

public enum Rank { S, A, B, C, D }

public class GameManager : MonoBehaviour
{
    // Singleton Instance
    public static GameManager instance { get; set; }
    
    public Rank currentRank {get; private set;}

    public float Score;
    
    // potential Dictionary swap for pairing the String for the level name and its Data
    public List<LevelData> levelData;
    private int points;
    private bool isTimerOn = false;
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

    public void AssignRank(Rank rank)
    {
        currentRank = rank;

        switch (Score)
        {
            case < 25.0f:
                rank = Rank.D;
                break;
            case >= 25.0f and < 50.0f :
                rank = Rank.C;
                break;
            case >=  50.0f and < 75.0f :
                rank = Rank.B;
                break;
            case >= 75.0f and < 100.0f :
                rank = Rank.A;
                break;
            case >= 100.0f:
                rank = Rank.S;
                break;
                
            
        }
    }

    // This is called at the end of the level 
    public void UpdateScore()
    {
        
        float killScore = points /* Current player kill score*/ / /* Max possible kills thats stored in level data*/ ;
        float timeScore =  /* how long the player took vs a theoratical optimal time*/;
        
        Score = (killScore * timeScore) * /* player combo meter value*/

    }


    private void PointCounter(int pointValue)
    {
        points += pointValue;
        
        onEnemyDeath?.Invoke();
    }
    
    
    
}
