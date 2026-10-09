using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class EnemyStateMachine : StateMachine
{
    #region components

    [HideInInspector] public UnityEngine.AI.NavMeshAgent agent;

    [SerializeField] ParticleSystem parryParticle;
    public ParticleSystem GetParryParticle() { return parryParticle; }

    Animator animator;
    public Animator GetAnimator() { return animator; }

    ManualAudioCall audioCall;
    public ManualAudioCall GetAudioCall() { return audioCall; }

    #endregion

    #region variables

    [Header("Movement")]
    [Min(0f)] public float movementSpeed = 3.5f;
    public float GetBackOffSpeed() { return movementSpeed * .75f; }

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

    [Header("Attacking Values")]
    [Min(0f)] public float attackFrequency = 1f; // how fast this enemy checks to attack when entering attackRange
    [Min(0f)] public float attackRange = 4f; // distance to stop following begin attacking player
    [Min(0f)] public float backOffRange = 1f; // distance to walk away from player

    #endregion

    #region checks

    bool referencesAssigned
    {
        get
        {
            return player != null &&
            eyes != null &&
            sightTarget != null;
        }
    }

    public bool HasLineOfSight()
    {
        if (!referencesAssigned)
        {
            Debug.LogWarning("Position references are unassigned, returning as false");
            return false;
        }

        Debug.DrawLine(
            eyes.position,
            sightTarget.position,
            hasLineOfSight ? Color.green : Color.red
        );

        return !Physics.Linecast(
            eyes.position,
            sightTarget.position,
            obstacleLayers,
            QueryTriggerInteraction.Ignore
        );
    }

    public float DistanceToPlayer() 
    {
        if (!referencesAssigned)
        {
            Debug.LogWarning("Position references are unassigned, returning as false");
            return Mathf.Infinity;
        }

        return Vector3.Distance(
            transform.position,
            player.position
        );
    }

    public bool CanPerformActions =>
        isActiveAndEnabled &&
        GetCurrentState() != null &&
        GetCurrentState() != KnockbackState;

    public bool AgentReady =>
        agent != null &&
        agent.isActiveAndEnabled &&
        agent.isOnNavMesh;

    #endregion

    #region states

    public EnemyStateIdle IdleState { get; private set; }
    public override StatesBase GetInitialState() { return IdleState; }

    public EnemyStateFollow FollowingState { get; private set; }
    public EnemyStateKnockback KnockbackState { get; private set; }

    public virtual StatesBase GetAttackState() { return null; }

    public EnemyStateDead DeadState { get; private set; }
    public override StatesBase GetDeathState() { return DeadState; }

    #endregion

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        audioCall = GetComponentInChildren<ManualAudioCall>();
    }
    
    public override void InstantiateStates()
    {
        base.InstantiateStates();

        IdleState = new EnemyStateIdle(this);
        FollowingState = new EnemyStateFollow(this);
        KnockbackState = new EnemyStateKnockback(this);
        DeadState = new EnemyStateDead(this);
    }

    public override void InstantiateValues()
    {
        base.InstantiateValues();
    }

    public void ApplyKnockback(
        Vector3 knockbackDirection,
        float knockbackForce,
        float knockbackDuration)
    {
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
