using UnityEngine;

public class C1Attack2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Health enemyHealth = other.gameObject.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.ChangeHealthBy(-8);
            int EHcurrentHealthPoints = enemyHealth.GetCurrentHealthPoints();
            Debug.Log(EHcurrentHealthPoints);
        }
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 kickDirection = (other.transform.position - transform.position).normalized;
            rb.AddForce(kickDirection * 5f, ForceMode.Impulse);
        }

        
    }
}
