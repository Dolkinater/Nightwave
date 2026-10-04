using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum scoreMultiplier { 1.0, 1.5, 2.0 };
public class LevelManager : MonoBehaviour
{
    [FormerlySerializedAs("playerController")] public GameObject player;
    
    [SerializeField]
    private bool levelComplete;
    
    [SerializeField]
    private int currentPoints;

    //The amount of kills/points needed to reach the multiplier state
    [SerializeField]
    private int killStreakValue;

    [SerializeField] 
    private bool isKillStreaking;
    
    [SerializeField]
    private float streakCutoff = 5.0f;
    
    public LevelData levelData;
    
    private scoreMultiplier scoreMultiplier;

    [FormerlySerializedAs("points")] public int totalPoints;
    
    public  event Action onEnemyDeath;

    void Awake()
    {
        //subscribe to game manager event
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
        //update the level timer
        //Multiplier Coroutine
    }

    void Timer(float time)
    {
        
    }

    void StartKillStreaking()
    {
        while (isKillStreaking)
        {
            return;
        }

        StartCoroutine(Multiplier());
    }
    
    
    
    //Subscribe to the enemy death even then as the level is going on increment the points on kill to a hidden bar
    /// <summary>
    /// Create a function that starts a coroutine everytime a point is gained.
    /// IE a function that is listening to the on enemy death event
    /// the Listening fucntion starts the corroutine
    /// the function isn't active while the multipler corroutine is running
    /// once the coroutine is active it startss incrementing the score for the bar
    /// once the bar reaches a threshold it multiplies the score if the score point reaches a certain threshold
    /// </summary>
    /// <returns></returns>

    IEnumerable<WaitForSeconds> Multiplier()
    {
        //Stores the amount of points earned while killstreaking
        currentPoints = 0;
        
        //Make it so that it cant be called again
        isKillStreaking = true;
        
        while (isKillStreaking)
        {
            //increment points
            PointCounter(currentPoints);
            
            
            
            //Run a switchcase while kill streaking 
            switch (currentPoints)
            {
                case >= 100:
                    
            }
            
            yield return new WaitForSeconds(streakCutoff);

            break;
        }
        
        isKillStreaking = false;
    }
    
    
    private void PointCounter(int pointValue)
    {
        totalPoints += pointValue;
        
        onEnemyDeath?.Invoke();
    }
    
    // event that plays when the level is complete 
    //has a function that writes the new info to a save/progress meter 
    //does level handling like taking to a score screen
}
