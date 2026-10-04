using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public struct MultiplierTier
{
    public float tierMultiplier;
    public int enemyKillsNeeded;
    
}
public class LevelManager : MonoBehaviour
{
    [FormerlySerializedAs("playerController")] public GameObject player;
    
    [SerializeField]
    private bool levelComplete;
    
    [SerializeField]
    private int currentPoints;

    [SerializeField] 
    private int playerScore;
    
    [SerializeField]
    private int savedPlayerScore;

    private int enemyKillCount;
    
    private int comboBarHP = 5;

    private float tierMultiplier;
    
    [SerializeField]
    private MultiplierTier[] comboTier;
    

    //The amount of kills/points needed to reach the multiplier state
    [SerializeField]
    private int killStreakValue;

    [SerializeField] 
    private bool isKillStreaking;
    
    [SerializeField]
    private float streakCutoff = 5.0f;
    
    public LevelData levelData;
    

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
        if (enemyKillCount >= killStreakValue)
        {
            StartKillStreaking();
        }
        
    }

    void Timer(float time)
    {
        
    }

    void StartKillStreaking()
    {
        
        //Start the killstreaking coroutine s
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

    IEnumerator Multiplier()
    {
        
        //Make it so that it cant be called again
        isKillStreaking = true;
        
        float timer = streakCutoff;

        comboBarHP = 5;

        int lastKill = enemyKillCount;
        
        //timer has to reset everytime a point is gained
        // can only run while this combo still has HP
        
        while (timer > 0  && comboBarHP > 0)
        {
            //if the current points increases during the loop set reset the timer
            if (enemyKillCount > lastKill)
            {
                timer = streakCutoff;
                lastKill = enemyKillCount;
            }
            else{
                timer -= Time.deltaTime;

            }
            
            //Checks for Combo HP damage

            
            //Run a switchcase while kill streaking 
            foreach ( MultiplierTier tier in comboTier )
            {
                if (enemyKillCount >= tier.enemyKillsNeeded)
                {
                    tierMultiplier = tier.tierMultiplier;
                }
            }
            
            yield return null;
            
        }
        
        isKillStreaking = false;
    }
    
    
    private void KillCounter()
    {

        enemyKillCount++;
        // Enemy must listen to this event and this will fire off when the enemies die
        onEnemyDeath?.Invoke();
    }
    
    // event that plays when the level is complete 
    //has a function that writes the new info to a save/progress meter 
    //does level handling like taking to a score screen
}
