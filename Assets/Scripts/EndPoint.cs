using System;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Other = " + other.name);

        if (other.gameObject.CompareTag("Player"))
        {
            LevelManager.OnLevelComplete();

            UnityEngine.SceneManagement.SceneManager.LoadScene("EndScene");
        }
        else
        {

            Debug.Log("Tag = " + other.tag);
        }
    }
}
