using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class LevelManager : MonoBehaviour
{
    [FormerlySerializedAs("playerController")] public GameObject player;

    #region Variables

    [SerializeField]
    private bool levelComplete;
    
    [SerializeField]
    private int currentPoints;

    [SerializeField] 
    private int playerScore;
    
    [SerializeField]
    private int savedPlayerScore;
    
    [SerializeField]
    private int enemyKillCount;

    public GameObject[] checkpoints;

    public GameObject[] enemies;

    public GameObject endpoint;
    
    public LevelData levelData;

    #endregion

    #region Events

    public delegate void EndLevel(bool completedLevel); 
    // bool parameter checks if the player has actually beaten the level (true) or simply ended the level by exiting it or quitting the game
    // in both cases, persistent level data that needs to be cleared can be cleared when the event is called
    // however, the parameter allows subscribers to check if the player has actually beat the level before awarding a rank to them
    
    public static EndLevel OnLevelEndEvent;

    #endregion

    [FormerlySerializedAs("points")] public int totalPoints;
    

    void Awake()
    {
        Health.OnUnitDeathEvent += OnPlayerDeath;
    }
    
    void OnDestroy()
    {
        Health.OnUnitDeathEvent -= OnPlayerDeath;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        //Find the Player Controller
        player = GameObject.FindGameObjectWithTag("Player");

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPlayerDeath(Health.UnitAffiliation unitAffiliation, GameObject unit )
    {
        if (unitAffiliation == Health.UnitAffiliation.player)
        {
            OnLevelQuit();
        }
        
    }
    
    
    
    public static void OnLevelComplete()
    {
        
        
        OnLevelEndEvent?.Invoke(true);
        // use cases:
        // exiting the user out of the level and returning them to level selection
        // grading the player's rank for the level now that their score has been calculated
        // marking the level as complete within the game save data
        // clearing any level-specific values that persist between scenes (i.e. values for score, player position, etc.)

    }

    public static void OnLevelQuit()
    {
        
        
        OnLevelEndEvent?.Invoke(false);
        // use cases:
        // exiting the user out of the level and returning them to level selection
        // clearing any level-specific values that persist between scenes (i.e. values for score, player position, etc.)
    }

   

   
}


