using UnityEngine;


public enum Rank {S, A, B, C, D}
public class RankManager : MonoBehaviour
{

    private Rank currentRank;


    void Awake()
    {
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void AssignRank(Rank rank)
    {
        currentRank = rank;

        switch (Score)
        {
            case >= 100.0f:
                rank = Rank.S;
                break;
            case >= 75.0f and < 100.0f :
                rank = Rank.A;
                break;
            case >=  50.0f and < 75.0f :
                rank = Rank.B;
                break;
            case >= 25.0f and < 50.0f :
                rank = Rank.C;
                break;
            case < 25.0f:
                rank = Rank.D;
                break;

            
            
            
        }
    }
}
