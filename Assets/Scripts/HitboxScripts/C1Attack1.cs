using UnityEngine;

public class C1Attack1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Health enemyHealth = other.gameObject.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.ChangeHealthBy(-5);
            int EHcurrentHealthPoints = enemyHealth.GetCurrentHealthPoints();
            Debug.Log(EHcurrentHealthPoints);
        }
    }
}
