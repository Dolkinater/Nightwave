using System;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            LevelManager.OnLevelComplete();
        }
    }
}
