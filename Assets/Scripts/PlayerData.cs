using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    // The final data that will be stored and send to the the level's level data script
    #region FinalData

    public int finalScore;

    public char topRank;

    #endregion


    #region Uncalculated Data
    // The data that will be modified by the managers to store data and proccess info after level completion 
    public int tempScore;

    public char tempRank;

    public float timeTaken;
    
    #endregion

    
    #region PlayerController
    
    public static PlayerController playerController;
    
    #endregion


}
