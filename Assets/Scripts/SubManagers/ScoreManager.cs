using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public struct MultiplierTier
{
    public float tierMultiplier;
    public int enemyKillsNeeded;
    
}
public class ScoreManager : MonoBehaviour
{
    #region Variables
    
    [SerializeField]
    private int currentPoints;

    [SerializeField] 
    private int uncalculatedScore;
    
    [SerializeField]
    private int savedPlayerScore;
    
    [SerializeField]
    private int enemyKillCount;
    
    public LevelData levelData;
    
    [Header("Combo Bar Functionality")]
    [SerializeField] private int comboBarHP = 5;
    //The amount of kills/points needed to reach the multiplier state
    [SerializeField] private int killStreakValue;
    [SerializeField] private bool isKillStreaking;
    [SerializeField] private float streakCutoff = 5.0f;
    [SerializeField] private MultiplierTier[] comboTier;
    [SerializeField] private int comboHitThreshold;
    [SerializeField] private int playerScore;
    private float tierMultiplier;
    public int totalPoints;

    #endregion
    

    
    

    void Awake()
    {
        Health.OnHealthChangeEvent += OnDamagedCombo;
        CheckpointLogic.OnCheckpointReached += StoreScore;
        //Enemy event subscribe
        Health.OnUnitDeathEvent += UpdateScore;
    }
    
    void OnDestroy()
    {
        Health.OnHealthChangeEvent -= OnDamagedCombo;
        CheckpointLogic.OnCheckpointReached -= StoreScore;
        Health.OnUnitDeathEvent -= UpdateScore;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        

    }

    // Update is called once per frame
    void Update()
    {
        if (!isKillStreaking)
        {
            if (enemyKillCount >= killStreakValue)
            {
                StartKillStreaking();
            }
        }
        //Multiplier Coroutine
        
    }
    

    void StartKillStreaking()
    {
        
        //Start the killstreaking coroutine 
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
        
        //change name
        uncalculatedScore = 0;
        
        //timer has to reset everytime a point is gained
        // can only run while this combo still has HP
        
        while (timer > 0  && comboBarHP > 0)
        {
            //if the current points increases during the loop set reset the timer
            if (enemyKillCount > lastKill)
            {
                timer = streakCutoff;
                lastKill = enemyKillCount;
                uncalculatedScore++;

                for (int i = comboTier.Length - 1; i >= 0; i--)
                {
                    if (enemyKillCount <= comboTier[i].enemyKillsNeeded)
                    {
                        tierMultiplier = comboTier[i].tierMultiplier;
                    }
                }
            }
            else{
                timer -= Time.deltaTime;
            }
            
            
            yield return null;
            
        }

        enemyKillCount = 0;
        
        //End the kill Streaking state
        isKillStreaking = false;
        
        //Here the score multiplier is calculated and added on to the real score
        SetScore();
    }


    private void SetScore()
    {
        
        totalPoints =  playerScore + Mathf.RoundToInt(uncalculatedScore * tierMultiplier);
        playerScore = totalPoints;

    }

    private void StoreScore()
    {
        CheckpointLogic.storedScore = totalPoints;
    }

    void UpdateScore(Health.UnitAffiliation unitAffiliation, GameObject unit)
    {
        
        if (unitAffiliation == Health.UnitAffiliation.enemy)
        {
            //Check the scoreDistributor and add its value to the score
        }
    }
    

    private void OnDamagedCombo(int currentHealth, int maxHealth, int value)
    {
        //get the player's max health and check if they lost more than 1/3 of their hp during a combo
        
        //if the player gained health don't run and just leave the function
        if (value > 0)
        {
            return;
        }
        comboHitThreshold = Mathf.FloorToInt((maxHealth * .33f));
        
        if (comboHitThreshold  >  value)
        {
            comboBarHP -= 2;
        }
        else
        {
            comboBarHP -= 1;
        }
        
    }
    
   
   
}
