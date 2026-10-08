using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyTracking : EnemyStateMachine_Deprecated
{
    [Header("References")]
    public Transform player;
    public Transform eyes;
    public Transform sightTarget;

    [Header("Detection")]
    [Min(0.1f)] public float detectionDistance = 10f;
    [Min(1f)] public float loseTargetMultiplier = 1.5f;
    public LayerMask obstacleLayers;

    [Header("Lost Sight")]
    [Min(0f)] public float loseSightDelay = 3f;

    [Header("Knockback Test Values")]
    [Min(0f)] public float knockbackForce = 6f;
    [Min(0.01f)] public float knockbackDuration = 0.5f;

    [Header("Knockback Testing")]
    public bool enableKnockbackTest = true;
    public Key knockbackTestKey = Key.K;
    [Min(0f)] public float knockbackTestRange = 4f;

    [Header("Detection - View During Play")]
    [SerializeField] private bool hasLineOfSight;
    [SerializeField] private float distanceToPlayer;

    public bool HasLineOfSight => hasLineOfSight;
    public float DistanceToPlayer => distanceToPlayer;

    public bool CanPerformActions =>
        isActiveAndEnabled &&
        currentState != null &&
        currentState != KnockbackState;

    public EnemyIdleState IdleState { get; private set; }
    public EnemyFollowingState FollowingState { get; private set; }
    public EnemyKnockbackState KnockbackState { get; private set; }

    private NavMeshAgent agent;

    private bool AgentReady =>
        agent != null &&
        agent.isActiveAndEnabled &&
        agent.isOnNavMesh;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        IdleState = new EnemyIdleState(this);
        FollowingState = new EnemyFollowingState(this);
        KnockbackState = new EnemyKnockbackState(this);
    }

    protected override EnemyBaseState GetInitialState()
    {
        return IdleState;
    }

    protected override void Update()
    {
        if (!AgentReady)
            return;

        bool referencesAssigned =
            player != null &&
            eyes != null &&
            sightTarget != null;

        if (referencesAssigned)
        {
            UpdatePlayerDetection();
            CheckKnockbackTest();
        }
        else
        {
            hasLineOfSight = false;
            distanceToPlayer = Mathf.Infinity;

            if (currentState != KnockbackState)
            {
                ChangeState(IdleState);
                StopMoving();
                return;
            }
        }

        base.Update();
    }

    private void UpdatePlayerDetection()
    {
        distanceToPlayer = Vector3.Distance(
            transform.position,
            player.position
        );

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

    private void CheckKnockbackTest()
    {
        if (!enableKnockbackTest || Keyboard.current == null)
            return;

        if (knockbackTestKey == Key.None)
            return;

        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        if (Keyboard.current[knockbackTestKey].wasPressedThisFrame &&
            hasLineOfSight &&
            distanceToPlayer <= knockbackTestRange)
        {
            Vector3 directionAwayFromPlayer =
                transform.position - player.position;

            ApplyKnockback(
                directionAwayFromPlayer,
                knockbackForce,
                knockbackDuration
            );
        }
    }

    public void ApplyKnockback(
        Vector3 knockbackDirection,
        float knockbackForce,
        float knockbackDuration)
    {
        if (!isActiveAndEnabled || !AgentReady ||
            KnockbackState == null)
        {
            return;
        }

        knockbackDirection.y = 0f;

        if (knockbackDirection.sqrMagnitude < 0.0001f ||
            knockbackForce <= 0f ||
            knockbackDuration <= 0f)
        {
            return;
        }

        knockbackDirection.Normalize();

        KnockbackState.Configure(
            knockbackDirection,
            knockbackForce,
            knockbackDuration
        );

        ChangeState(KnockbackState);
    }

    public void MoveKnockback(Vector3 displacement)
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

    public void FollowPlayer()
    {
        if (!AgentReady || player == null)
            return;

        if (!CanPerformActions)
            return;

        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    public void StopMoving()
    {
        if (!AgentReady)
            return;

        agent.isStopped = true;
        agent.ResetPath();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            detectionDistance
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            detectionDistance * loseTargetMultiplier
        );
    }
}