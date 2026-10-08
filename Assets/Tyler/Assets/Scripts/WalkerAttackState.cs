using UnityEngine;

public class WalkerAttackState : EnemyBaseState
{
    private readonly WalkerController walker;

    private const float PunchDuration = 0.3f;
    private const float RecoveryDuration = 0.5f;
    private const float ParryCueDuration = 0.1f;

    private float elapsedTime;
    private bool punchStarted;

    private Vector3 punchDirection;
    private float punchSpeed;

    public WalkerAttackState(WalkerController walker)
        : base("Attack")
    {
        this.walker = walker;
    }

    public override void Enter()
    {
        elapsedTime = 0f;
        punchStarted = false;
        punchSpeed = 0f;
        punchDirection = Vector3.zero;

        walker.StopMoving();
        walker.SetParryCue(false);
        walker.SetAttackPhase("Wind-Up");
    }

    public override void UpdateLogic()
    {
        float previousTime = elapsedTime;
        elapsedTime += Time.deltaTime;

        float punchStartTime = walker.windUpTime;
        float recoveryStartTime =
            punchStartTime + PunchDuration;

        float attackEndTime =
            recoveryStartTime + RecoveryDuration;

        // Turn toward the player until the punch begins.
        if (previousTime < punchStartTime)
        {
            walker.FacePlayer();
        }

        // Show the marker only near the end of the wind-up.
        float cueStartTime = Mathf.Max(
            0f,
            punchStartTime - ParryCueDuration
        );

        bool showCue =
            elapsedTime >= cueStartTime &&
            elapsedTime < punchStartTime;

        walker.SetParryCue(showCue);

        // Calculate how much of this frame belongs to Punch.
        float punchStep = GetPhaseStep(
            previousTime,
            elapsedTime,
            punchStartTime,
            recoveryStartTime
        );

        if (punchStep > 0f)
        {
            if (!punchStarted)
            {
                BeginPunch();
            }

            walker.MoveAttack(
                punchDirection * punchSpeed * punchStep
            );
        }

        // Calculate how much of this frame belongs to Recovery.
        float recoveryStep = GetPhaseStep(
            previousTime,
            elapsedTime,
            recoveryStartTime,
            attackEndTime
        );

        if (recoveryStep > 0f)
        {
            float recoveryTimeBefore = Mathf.Clamp(
                previousTime - recoveryStartTime,
                0f,
                RecoveryDuration
            );

            float recoveryTimeAfter = Mathf.Clamp(
                elapsedTime - recoveryStartTime,
                0f,
                RecoveryDuration
            );

            // Average speed over this part of the recovery.
            float averageProgress =
                (recoveryTimeBefore + recoveryTimeAfter) /
                (2f * RecoveryDuration);

            float recoverySpeed =
                punchSpeed * (1f - averageProgress);

            walker.MoveAttack(
                punchDirection * recoverySpeed * recoveryStep
            );
        }

        if (elapsedTime >= attackEndTime)
        {
            walker.ChangeState(walker.FollowState);
            return;
        }

        if (elapsedTime < punchStartTime)
        {
            walker.SetAttackPhase("Wind-Up");
        }
        else if (elapsedTime < recoveryStartTime)
        {
            walker.SetAttackPhase("Punch");
        }
        else
        {
            walker.SetAttackPhase("Recovery");
        }
    }

    private void BeginPunch()
    {
        punchStarted = true;

        // Lock direction and speed at the start of the punch.
        punchDirection = walker.transform.forward;
        punchDirection.y = 0f;
        punchDirection.Normalize();

        punchSpeed = walker.DistanceToPlayer * 0.9f;

        walker.SetParryCue(false);
    }

    private float GetPhaseStep(
        float frameStart,
        float frameEnd,
        float phaseStart,
        float phaseEnd)
    {
        float overlapStart = Mathf.Max(frameStart, phaseStart);
        float overlapEnd = Mathf.Min(frameEnd, phaseEnd);

        return Mathf.Max(0f, overlapEnd - overlapStart);
    }

    public override void Exit()
    {
        walker.SetParryCue(false);
        walker.StopMoving();
        walker.SetAttackPhase("None");
        walker.StartAttackRecharge();
    }
}