using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WalkerController : EnemyStateMachine
{
    [Header("References")]
    public FirstPersonPlayer player;
    public Transform eyes;
    public Transform sightTarget;
    public LayerMask obstacleLayers;
    public GameObject parryCue;

    [Header("Ranges")]
    [Min(0.1f)] public float followingRange = 10f;
    [Min(0.1f)] public float attackRange = 2.5f;
    [Min(1f)] public float loseTargetMultiplier = 1.5f;

    [Header("Movement")]
    [Min(0f)] public float movementSpeed = 3.5f;

    [Header("Lost Sight")]
    [Min(0f)] public float loseSightDelay = 3f;

    [Header("Attack Settings")]
    [Min(0.01f)] public float attackCheckTime = 1f;
    [Min(0.1f)] public float windUpTime = 0.5f;
    [Min(0)] public int damageDealt = 10;
    [Min(0f)] public float postAttackRecharge = 1.5f;

    [Header("Detection - View During Play")]
    [SerializeField] private bool hasLineOfSight;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private bool navigationReady;

    public bool HasLineOfSight => hasLineOfSight;
    public float DistanceToPlayer => distanceToPlayer;

    public float BackingOffSpeed =>
        player != null ? player.moveSpeed * 0.75f : 0f;

    public WalkerIdleState IdleState { get; private set; }
    public WalkerFollowState FollowState { get; private set; }

    public WalkerAttackState AttackState { get; private set; }

    private NavMeshAgent agent;

    [Header("Attack - View During Play")]
    [SerializeField] private string attackPhase = "None";

    private float nextAllowedAttackTime;

    private bool AgentReady =>
        agent != null &&
        agent.isActiveAndEnabled &&
        agent.isOnNavMesh;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        IdleState = new WalkerIdleState(this);
        FollowState = new WalkerFollowState(this);
        AttackState = new WalkerAttackState(this);

        SetParryCue(false);
    }

    protected override EnemyBaseState GetInitialState()
    {
        return IdleState;
    }

    protected override void Update()
    {
        navigationReady = AgentReady;

        bool referencesAssigned =
            player != null &&
            eyes != null &&
            sightTarget != null;

        if (!referencesAssigned)
        {
            hasLineOfSight = false;
            distanceToPlayer = Mathf.Infinity;

            ChangeState(IdleState);
            StopMoving();
            return;
        }

        // Detection still updates if navigation is unavailable.
        UpdatePlayerDetection();

        if (!navigationReady)
            return;

        base.Update();
    }

    private void UpdatePlayerDetection()
    {
        Vector3 toPlayer =
            player.transform.position - transform.position;

        // Measure distance across the ground.
        toPlayer.y = 0f;
        distanceToPlayer = toPlayer.magnitude;

        bool viewBlocked = Physics.Linecast(
            eyes.position,
            sightTarget.position,
            obstacleLayers,
            QueryTriggerInteraction.Ignore
        );

        hasLineOfSight = !viewBlocked;

        Debug.DrawLine(
            eyes.position,
            sightTarget.position,
            hasLineOfSight ? Color.green : Color.red
        );
    }

    public void UpdateFollowMovement()
    {
        if (!AgentReady || player == null)
            return;

        float backingOffRange = attackRange * 0.5f;

        if (distanceToPlayer < backingOffRange)
        {
            BackAway(backingOffRange);
        }
        else if (distanceToPlayer <= attackRange)
        {
            StopMoving();
            FacePlayer();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        agent.speed = movementSpeed;

        // Allow the agent to enter attack range before it
        // finishes braking. Our distance check stops it there.
        agent.stoppingDistance = attackRange * 0.9f;

        agent.updateRotation = true;
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
    }

    private void BackAway(float backingOffRange)
    {
        StopMoving();
        FacePlayer();

        Vector3 awayFromPlayer =
            transform.position - player.transform.position;

        awayFromPlayer.y = 0f;

        // Provide a direction even if both positions overlap.
        if (awayFromPlayer.sqrMagnitude < 0.0001f)
            awayFromPlayer = -transform.forward;

        awayFromPlayer.Normalize();

        // Avoid stepping beyond the backing-off threshold.
        float movementThisFrame = Mathf.Min(
            BackingOffSpeed * Time.deltaTime,
            backingOffRange - distanceToPlayer
        );

        Vector3 displacement =
            awayFromPlayer * movementThisFrame;

        // Stop at the edge of the walkable NavMesh.
        if (agent.Raycast(
            agent.nextPosition + displacement,
            out NavMeshHit hit))
        {
            displacement = hit.position - agent.nextPosition;
        }

        agent.Move(displacement);
    }

    public void FacePlayer()
    {
        Vector3 direction =
            player.transform.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            agent.angularSpeed * Time.deltaTime
        );
    }

    public void StopMoving()
    {
        if (!AgentReady)
            return;

        agent.isStopped = true;
        agent.ResetPath();
        agent.updateRotation = false;
    }

    public bool CanStartAttack()
    {
        return AgentReady &&
               player != null &&
               HasLineOfSight &&
               DistanceToPlayer <= attackRange &&
               Time.time >= nextAllowedAttackTime;
    }

    public void SetAttackPhase(string phase)
    {
        attackPhase = phase;
    }

    public void StartAttackRecharge()
    {
        nextAllowedAttackTime = Time.time + postAttackRecharge;
    }

    public void SetParryCue(bool visible)
    {
        if (parryCue != null)
            parryCue.SetActive(visible);
    }

    public void MoveAttack(Vector3 displacement)
    {
        if (!AgentReady)
            return;

        if (agent.Raycast(
            agent.nextPosition + displacement,
            out NavMeshHit hit))
        {
            displacement = hit.position - agent.nextPosition;
        }

        agent.Move(displacement);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            followingRange
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            followingRange * loseTargetMultiplier
        );

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(
            transform.position,
            attackRange * 0.5f
        );
    }
}