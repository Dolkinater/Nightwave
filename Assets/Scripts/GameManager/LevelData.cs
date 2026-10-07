using System.Collections.Generic;
using UnityEngine;

//Each Level should have its own Level Data
//Bare Bones ATM but it should contain all the parameters that you would need to make a level 
// IE enemy count, checkpoints, what scene the level is on.
//This makes the Game Manager and future Level Manager Script less cluttered and more of a plug and play system

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    //Prolly best used with a dedicated level manager object to handle this info 
    //how Long this level should take to complete
    public float completionTimer;
    
    // The amount of enemies in the level
    // Needs to be a list of enemies and types tbh
    public List<GameObject> enemies;
    
    //List of the Gameobjects with the Checkpoints scripts attached
    public List<GameObject> checkpoints;
    
}
