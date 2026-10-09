using UnityEngine;

public class WalkerPunch : MonoBehaviour
{
    Walker_EnemySM enemy;

    private void Awake()
    {
        enemy = transform.parent.GetComponent<Walker_EnemySM>();
    }

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
                Debug.Log("Parry Attack");
                enemy.ApplyKnockback((transform.position - other.transform.position).normalized, 10f, 1f);
            }
        }
    }
}
