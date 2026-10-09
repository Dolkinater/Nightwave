using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Text
     public TextMeshProUGUI ScoreText;
     public TextMeshProUGUI HealthText;
     public TextMeshProUGUI KillStreakText;
     public TextMeshProUGUI MultiplierText;
     public TextMeshProUGUI DashStockText;
     public TextMeshProUGUI AbilityCooldownText;
     public TextMeshProUGUI TimerText;
    #endregion
    
    #region References
     public ScoreManager scoreManager;
     public GameObject player;
    public PlayerStateMachine playerSM;
     public Health playerHealth;
     public GameObject killStreakUIObject;
    public Image fadeOutImage;

    public Slider comboHealthSlider;
    public Slider multiplierTimerSlider;
     //Get references to the dash stock anf ability cooldowns
    #endregion


    private void OnEnable()
    {

        //ScoreManager.OnScoreEvent -= UpdateScore;
    }

    private void OnDisable()
    {

        //ScoreManager.OnScoreEvent -= UpdateScore;
    }

    public bool fadeOut = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHealth = player.GetComponent<Health>();

        ScoreText.text = "Score: " + ScoreManager.playerScore;
        KillStreakText.text = "Kill Streak: " + 0;
        MultiplierText.text = scoreManager.GetTierMultiplier().ToString() + "x";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateKillStreak();

        UpdateHealth();

        FadeOut();

        UpdateAbilityText();

        UpdateScore();

        UpdateSliders();

        DashStockText.text = "Dash Stocks: " + playerSM.GetCurrentDashStock();
        TimerText.text = "" + Timer.levelTimer;
    }

    void UpdateSliders()
    {
        comboHealthSlider.value = scoreManager.GetComboHealthValue();
        multiplierTimerSlider.value = scoreManager.GetCutoffTimeValue();

    }

    void UpdateScore()
    {
        ScoreText.text = "Score: " + ScoreManager.playerScore + " + " + scoreManager.GetUncalculatedScore();
    }

    void UpdateHealth()
    {
        HealthText.text = "Health: " + playerHealth.GetCurrentHealthPoints().ToString();
    }

    void UpdateKillStreak()
    {
       killStreakUIObject.SetActive(true);
       KillStreakText.text = "Kill Streak: " + scoreManager.GetEnemyKillCount();
       MultiplierText.text = scoreManager.GetTierMultiplier().ToString() + "x";
    }

    void UpdateAbilityText()
    {
        AbilityCooldownText.text = playerSM.GetCanUseSpecialAbility() ? "Special Ability Active!" : "Ability Cooldown: " + Mathf.RoundToInt(playerSM.GetCurrentSpecialCooldown()) + "s"; 
    }

    float opacity = 0;
    void FadeOut()
    {
        if (fadeOut) { opacity += Time.deltaTime; opacity = Mathf.Clamp01(opacity); }

        fadeOutImage.color = new Color(0, 0, 0, opacity);
    }

}
