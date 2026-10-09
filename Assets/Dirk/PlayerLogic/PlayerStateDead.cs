using UnityEngine;

public class PlayerStateDead : StatesBase
{
    PlayerStateMachine sm;

    public PlayerStateDead(StateMachine baseSM) : base(baseSM)
    {
        sm = (PlayerStateMachine)baseSM;
    }

    public override void thisStart()
    {
        base.thisStart();

        Object.FindAnyObjectByType<UIManager>().fadeOut = true;

        timer = 1f;
    }

    float timer = 1f;

    public override void thisUpdate()
    {
        base.thisUpdate();

        timer -= Time.deltaTime;

        if (timer > 0) { return; }

        CheckpointLogic.OnRespawn();
    }

}
