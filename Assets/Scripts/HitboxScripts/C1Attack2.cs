using UnityEngine;

public class C1Attack2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) { return; }
        Health enemyHealth = other.gameObject.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.ChangeHealthBy(-8);
            int EHcurrentHealthPoints = enemyHealth.GetCurrentHealthPoints();
            Debug.Log(EHcurrentHealthPoints);
        }

        EnemyStateMachine sm = other.gameObject.GetComponent<EnemyStateMachine>();

        if (sm != null) {
            Debug.Log("Hit");
        sm.ApplyKnockback((other.transform.position - transform.position).normalized, 10f, 1f);
        
        }
    }
}
