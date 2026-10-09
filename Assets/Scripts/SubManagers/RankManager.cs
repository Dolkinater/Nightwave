using UnityEngine;


public enum Rank {S, A, B, C, D}
public class RankManager : MonoBehaviour
{

    private Rank rank;
    public ScoreManager scoreManager;
    public Timer timer;
    public float timeGoal;
    
    
    public PlayerData playerData;

    void Awake()
    {
       LevelManager.OnLevelEndEvent += AssignRank;
    }
    public void AssignRank(bool competedLevel)
    {
        if (competedLevel)
        {
            switch (scoreManager.GetPlayerScore())   
            {
                case >= 100:
                    if (timer.GetLevelTimer() < timeGoal)
                    {
                        rank = Rank.S;
                    }
                    break;
                case >= 75:
                    rank = Rank.A;
                    break;
                case >=  50:
                    rank = Rank.B;
                    break;
                case >= 25:
                    rank = Rank.C;
                    break;
                case < 25:
                    rank = Rank.D;
                    break;
            
            }
        }
        
    }
}
