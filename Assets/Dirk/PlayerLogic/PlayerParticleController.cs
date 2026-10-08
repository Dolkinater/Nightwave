using UnityEngine;

public class PlayerParticleController : MonoBehaviour
{
    [SerializeField] ParticleSystem dashParticle;
    [SerializeField] ParticleSystem slideParticle;
    [SerializeField] ParticleSystem walkParticle;
    [SerializeField] ParticleSystem jumpParticle;

    [SerializeField] ParticleSystem specialAttackParticle;

    public enum ParticleStates
    {
        dashState,
        slideState,
        walkState,
        jumpState,
        c1_specialState,
        none
    }

    private void Start()
    {
        CallParticle(ParticleStates.none);
    }

    public void CallParticle(ParticleStates particleState)
    {
        if (particleState == ParticleStates.dashState) { dashParticle.Play(); } else { dashParticle.Stop(); }
        if (particleState == ParticleStates.slideState) { slideParticle.Play(); } else { slideParticle.Stop(); }
        if (particleState == ParticleStates.walkState) { walkParticle.Play(); } else { walkParticle.Stop(); }
        if (particleState == ParticleStates.jumpState) { jumpParticle.Play(); } else { jumpParticle.Stop(); }
        if (particleState == ParticleStates.c1_specialState) { specialAttackParticle.Play(); } else { specialAttackParticle.Stop(); }
    }
}
