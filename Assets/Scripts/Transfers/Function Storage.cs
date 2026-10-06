using UnityEngine;

public struct LevelRankTier
{
    public int tierMinScore;
    public char tierGrade;

}

public class FunctionStorage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void RankCalc(PlayerData playerData)
    {
        int finalPlayerScore = playerData.tempScore;

        //subscribed to the level end event

        float finalTimeScore = playerData.timeTaken;


    }

    
    //pass to the level data its a personalized tier
    public LevelRankTier[] levelTiers;

    // honestly should be stored in each level data file so that it can be quickly compared
   


   // For the level manager the on level quit function the level manger should

}
