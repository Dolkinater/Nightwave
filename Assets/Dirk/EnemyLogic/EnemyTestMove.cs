using UnityEngine;
using UnityEngine.AI;

public class EnemyTestMove : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
    }
}
