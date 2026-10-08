using UnityEngine;
using System.Collections.Generic;

public class CheckpointLogic : MonoBehaviour
{
    [SerializeField] Transform player;

    static Vector3 storedPlayerPosition;
    static Vector3 storedFacingDirection;
    static int storedScore;

    static List<GameObject> temp_defeatedEnemies; // enemies who are killed during gameplay are added to this list; when respawning, this list will be set to the value of permanent_defeatedEnemies
    public static void KillEnemyOnRespawn(GameObject enemy)
    {
        temp_defeatedEnemies.Add(enemy);
    }
    static List<GameObject> permanent_defeatedEnemies; // once players reach a checkpoint, this list is made equal to temp_defeatedEnemies; when respawning, enemies found in this list will be disabled

    void OnRespawn()
    {
        player.transform.position = storedPlayerPosition;
        player.transform.forward = storedFacingDirection; 
        // current score value is set to the value of storedScore;
        
        foreach (GameObject enemy in permanent_defeatedEnemies)
        {
            enemy.SetActive(false);
            // enemies are disabled at the beginning of the scene if they were previously killed before players received a checkpoint
            // enemies are disabled through SetActive rather than destroyed, as destroying gameobjects is a rather expensive function call
                // however, this may need to be changed in the event that we manually cull the enemies with the SetActive function, which would overwrite this
        }
    }

    void OnCheckPoint() // stores what enemies the player killed, how the player will be placed and rotated when respawning, and the current score value the player has earned
    {
        permanent_defeatedEnemies = temp_defeatedEnemies;

        storedPlayerPosition = transform.position;

        storedFacingDirection = transform.forward; // sets the player's forward rotation (the direction they will face when respawning) to the forward rotation of the checkpoint

        // storedScore = the current value of score the player has earned plus the value of score the player has stored in the multiplier
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if other is player
        // {
        OnCheckPoint();
        // }
    }
}