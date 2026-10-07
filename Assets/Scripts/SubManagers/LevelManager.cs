using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class LevelManager : MonoBehaviour
{
    [FormerlySerializedAs("playerController")] public GameObject player;

    #region Variables

    [SerializeField]
    private bool levelComplete;
    
    public GameObject[] checkpoints;

    //on Enemy death event should pass the enemy itself as a parameter for death handling
    // For example when an enemy dies the level manger should find the enemy killed and it to checkpoints temp killed enemies
    public GameObject[] enemies;

    public GameObject endpoint;

    public GameObject startpoint;
    
    // List of enemies could be stored in here but for the prototyped just getting them out if more important than polish
    public LevelData levelData;

    #endregion

    #region Events

    public delegate void EndLevel(bool completedLevel); 
    // bool parameter checks if the player has actually beaten the level (true) or simply ended the level by exiting it or quitting the game
    // in both cases, persistent level data that needs to be cleared can be cleared when the event is called
    // however, the parameter allows subscribers to check if the player has actually beat the level before awarding a rank to them
    
    public static EndLevel OnLevelEndEvent;

    #endregion

    
    

    void Awake()
    {
        //Subscribe to the enemy death event
    }
    
    void OnDestroy()
    {
        //Subscribe to the enemy death event
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


