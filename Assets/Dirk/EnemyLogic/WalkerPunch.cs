using UnityEngine;

public class WalkerPunch : MonoBehaviour
{
    [SerializeField] Walker_EnemySM enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }

        PlayerStateMachine sm = other.gameObject.GetComponent<PlayerStateMachine>();

        if (sm != null)
        {
            bool parried = false;
            sm.health?.ChangeHealthBy(-12, out parried);
            //knockback?

            if (enemy && parried)
            {
                enemy.ApplyKnockback((transform.position - other.transform.position).normalized, 10f, 1f);
            }
        }
    }
}
