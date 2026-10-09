using UnityEngine;

public class C1Attack2 : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) { return; }

        particle.Play();
        particle.transform.position = other.ClosestPointOnBounds(transform.position);

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
        sm.ApplyKnockback((other.transform.position - transform.position).normalized, 8f, .75f);
        
        }
    }
}
