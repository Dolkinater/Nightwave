using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CheckpointLogic : MonoBehaviour
{
    [SerializeField] Transform player;

    static Vector3 storedPlayerPosition;
    static Vector3 storedFacingDirection;
    public static int storedScore;

    private void Start()
    {
        player.transform.position = storedPlayerPosition;
        player.transform.forward = storedFacingDirection; 
    }


    public delegate void CheckpointReached();
    public static event CheckpointReached OnCheckpointReached;

    static List<GameObject> temp_defeatedEnemies; // enemies who are killed during gameplay are added to this list; when respawning, this list will be set to the value of permanent_defeatedEnemies
    public static void KillEnemyOnRespawn(GameObject enemy)
    {
        temp_defeatedEnemies.Add(enemy);
    }
    static List<GameObject> permanent_defeatedEnemies; // once players reach a checkpoint, this list is made equal to temp_defeatedEnemies; when respawning, enemies found in this list will be disabled
    
    public static void OnRespawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // current score value is set to the value of storedScore;
        
    }

    void OnCheckPoint() // stores what enemies the player killed, how the player will be placed and rotated when respawning, and the current score value the player has earned
    {
        permanent_defeatedEnemies = temp_defeatedEnemies;

        storedPlayerPosition = transform.position;

        storedFacingDirection = transform.forward; // sets the player's forward rotation (the direction they will face when respawning) to the forward rotation of the checkpoint

        // storedScore = the current value of score the player has earned plus the value of score the player has stored in the multiplier
        //this value is changed in scoremanager when this event is triggered
        OnCheckpointReached?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            OnCheckPoint();
        }
        
    }
}