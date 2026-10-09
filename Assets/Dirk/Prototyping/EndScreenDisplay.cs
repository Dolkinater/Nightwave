using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndScreenDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreFinal;

    [SerializeField] TextMeshProUGUI timeFinal;
    [SerializeField] TextMeshProUGUI timeBonusText;

    [SerializeField] TextMeshProUGUI totalScore;

    [SerializeField] Image logo;

    [SerializeField] Sprite sRank, aRank, bRank, cRank, dRank;

    int finalScore;
    int finalTime;
    int totalScoreValue;

    int bonus;

    private void Start()
    {
        finalScore = ScoreManager.playerScore;
        finalTime = Mathf.RoundToInt(Timer.levelTimer);

        scoreFinal.text = "Score: " + finalScore;
        timeFinal.text = "Time: " + finalTime;

        if (finalTime <= 60)
        {
            bonus = 10000;
        }
        else if (finalTime <= 80)
        {
            bonus = 8000;
        }
        else if (finalTime <= 100)
        {
            bonus = 1000;
        }
        else if (finalTime <= 150)
        {
            bonus = 500;
        }

        timeBonusText.text = "Bonus: " + bonus;

        totalScoreValue += finalScore + bonus;

        totalScore.text = "Total: " + totalScoreValue;

        GameManager.ReloadData();

        if (totalScoreValue >= 12000)
        {
            logo.sprite = sRank;
        }
        else if (totalScoreValue >= 9000)
        {
            logo.sprite = aRank;
        }
        else if (totalScoreValue >= 5000)
        {
            logo.sprite = bRank;
        }
        else if (totalScoreValue >= 3000)
        {
            logo.sprite = cRank;
        }
        else
        {
            logo.sprite = dRank;
        }

    }
}
