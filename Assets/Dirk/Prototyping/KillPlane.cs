using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("true");

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStateMachine a = other.gameObject.GetComponent<PlayerStateMachine>();

            if (a)
            {
                a.health.ChangeHealthBy(-9999999);
            }
        }
    }
}
