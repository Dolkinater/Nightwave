using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Text
     public TextMeshProUGUI ScoreText;
     public TextMeshProUGUI HealthText;
     public TextMeshProUGUI KillStreakText;
     public TextMeshProUGUI MultiplierText;
     public TextMeshProUGUI DashStockText;
     public TextMeshProUGUI AbilityCooldownText;
    #endregion
    
    #region References
     public ScoreManager scoreManager;
     public GameObject player;
     public Health playerHealth;
     public GameObject killStreakUIObject;
     //Get references to the dash stock anf ability cooldowns
    #endregion

    void Awake()
    {
        ScoreManager.OnScoreEvent += UpdateScore;
    }

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHealth = player.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreManager.CheckKillStreak())
        {
            UpdateKillStreak();
        }
        
        UpdateHealth();
    }

    void UpdateScore(int playerScore)
    {
        ScoreText.text = playerScore.ToString();
    }

    void UpdateHealth()
    {
        HealthText.text = playerHealth.GetCurrentHealthPoints().ToString();
    }

    void UpdateKillStreak()
    {
       killStreakUIObject.SetActive(true);
       KillStreakText.text = "Kill Streak " + scoreManager.GetEnemyKillCount();
       MultiplierText.text = scoreManager.GetTierMultiplier().ToString();
    }


}
