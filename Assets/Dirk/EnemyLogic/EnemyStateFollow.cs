using UnityEngine;

public class EnemyStateFollow : StatesBase
{
	EnemyStateMachine sm;

	public EnemyStateFollow(StateMachine baseSM) : base(baseSM)
	{
		sm = (EnemyStateMachine) baseSM;
    }

    private float timeWithoutSight;
    private float timeForFollowUpdate;
    private float timeAttackPlayer;

    public override void thisStart()
    {
        base.thisStart();

        if (!sm.AgentReady)
        { 
            Debug.LogWarning("NavMesh Agent unavailable or unapplied, returning to idle");
            sm.ChangeState(sm.IdleState);
            return;
        }

        sm.agent.updateRotation = true;

        timeWithoutSight = 0f;
        timeForFollowUpdate = 0f;

        timeAttackPlayer = sm.attackFrequency + Random.Range(-.5f, .5f);

        sm.GetAnimator().Play("follow");
        sm.GetAudioCall().PlayLoopingAudio(true);
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        sm.agent.SetDestination(sm.player.position);

        UpdateLoseTarget();
        UpdatePlayerFollow();
        UpdateAttackPlayer();
    }

    void UpdateLoseTarget() // updates if the player lost the target
    {
        float loseTargetDistance =
            sm.detectionDistance * sm.loseTargetMultiplier;

        if (sm.DistanceToPlayer() > loseTargetDistance)
        {
            sm.ChangeState(sm.IdleState);
            return;
        }

        if (sm.HasLineOfSight())
        {
            timeWithoutSight = 0f;
        }
        else
        {
            timeWithoutSight += Time.deltaTime;

            if (timeWithoutSight >= sm.loseSightDelay)
            {
                sm.ChangeState(sm.IdleState);
                return;
            }
        }
    }

    
    void UpdatePlayerFollow() // updates if the player's navagent is moving and how it moves
    {
        sm.agent.speed = sm.movementSpeed;

        int movementStateValue = -1;

        if (sm.DistanceToPlayer() <= sm.backOffRange) // if the enemy is within attacking range, stop moving
        {
            movementStateValue = 0; // in range, too close
        }
        else if (sm.DistanceToPlayer() <= sm.attackRange)
        {
            movementStateValue = 1; // in range
        }
        else
        {
            movementStateValue = 2; // following
        }

        sm.agent.isStopped = movementStateValue != 2; // if the state is 0 or 2, the agent is moving

        if (movementStateValue == 2)
        {
            MoveTowardsPlayer();
        }
        else if (movementStateValue == 0)
        {
            BackOffFromPlayer();
        }
    }

    
    void MoveTowardsPlayer()
    {
        if (timeForFollowUpdate <= 0f) // timer-based buffer for pathfinding to prevent the path from needing to update every second
        {
            timeForFollowUpdate = .3f;
        }
        else
        {
            timeForFollowUpdate -= Time.deltaTime;
            return;
        }

        if (sm.player == null)
            return;

        if (!sm.CanPerformActions)
            return;

        sm.agent.SetDestination(sm.player.position);
    }

    void BackOffFromPlayer()
    {
        sm.FacePlayer();

        Vector3 awayFromPlayer =
            (sm.transform.position - sm.player.transform.position);

        awayFromPlayer.y = 0f;

        if (awayFromPlayer.sqrMagnitude < 0.0001f)
            awayFromPlayer = -sm.transform.forward;

        awayFromPlayer.Normalize();

       float movementThisFrame = sm.GetBackOffSpeed() * Time.deltaTime;
            
       //     Mathf.Min(
       //     sm.GetBackOffSpeed() * Time.deltaTime,
        //    sm.GetBackOffRange() - sm.DistanceToPlayer()
      //  );

        Vector3 displacement =
            awayFromPlayer * movementThisFrame;

        if (sm.agent.Raycast(
            sm.agent.nextPosition + displacement,
            out UnityEngine.AI.NavMeshHit hit))
        {
            displacement = hit.position - sm.agent.nextPosition;
        }

        sm.agent.Move(displacement);
    }


    void UpdateAttackPlayer() // updates the enemy's logic for attacking the player
    {
        timeAttackPlayer -= Time.deltaTime;

        if (timeAttackPlayer <= 0f)
        {
            if (sm.DistanceToPlayer() <= sm.attackRange)
            {
                sm.ChangeState(sm.GetAttackState());
            }
            else
            {
                timeAttackPlayer = sm.attackFrequency * 0.8f;
            }
        }
    }

    public override void thisEnd()
    {
        base.thisEnd();

        if (sm.AgentReady) // only stop the agent if it's active
        {
            sm.agent.isStopped = true;
            sm.agent.updateRotation = false;
            sm.agent.ResetPath();
        }

        sm.GetAudioCall().PlayLoopingAudio(false);
    }
}
