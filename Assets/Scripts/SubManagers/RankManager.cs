using UnityEngine;


public enum Rank {S, A, B, C, D}
public class RankManager : MonoBehaviour
{

    private Rank currentRank;
    
    public PlayerData playerData;

    void Awake()
    {
       // LevelManager.OnLevelEndEvent += AssignRank;
    }
    public void AssignRank(Rank rank)
    {
        currentRank = rank;

        switch (playerData.tempScore)   
        {
            case >= 100:
                rank = Rank.S;
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
