using UnityEngine;
public class ShooterStateShoot : StatesBase
{
    Shooter_EnemySM sm;

    public ShooterStateShoot(StateMachine baseSM) : base(baseSM)
    {
        sm = (Shooter_EnemySM)baseSM;
    }

    bool shootTwice = false;

    float recoilTime;

    public override void thisStart()
    {
        base.thisStart();

        shootTwice = Random.Range(0, 10) < 5;

        recoilTime = Random.Range(1.5f, 2.5f);

        Debug.Log("Shoot");

        Shoot();
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        recoilTime -= Time.deltaTime;

        if (recoilTime <= 0)
        {
            if (shootTwice)
            {
                shootTwice = false;

                recoilTime = Random.Range(0.75f, 1.5f);
                Shoot();
                return;
            }

            sm.ChangeState(sm.IdleState);
        }
    }

    void Shoot()
    {
        //display parry particle
        //display attack particle

        sm.GetParryParticle()?.Play();

        sm.FacePlayer();

        Object.Instantiate(sm.GetBullet(), sm.transform.position, Quaternion.identity).GetComponent<ShooterProjectile>().SetMovementDirection(sm.player.transform.position - sm.transform.position);
    }
}
