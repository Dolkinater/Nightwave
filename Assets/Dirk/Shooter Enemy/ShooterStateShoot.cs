using UnityEngine;

public class ShooterStateShoot : EnemyBaseState
{
    private readonly ShooterEnemySM enemy;
    public ShooterStateShoot(EnemyTracking enemy)
        : base("Following")
    {
        this.enemy = (ShooterEnemySM) enemy;
    }
    
    Transform newRef;

    bool shootTwice = false;

    float recoilTime;

    public override void Enter()
    {
        base.Enter();

        shootTwice = Random.Range(0, 10) < 5;

        recoilTime = Random.Range(1.5f, 2.5f);

        Shoot();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        recoilTime -= Time.deltaTime;

        if (recoilTime <= 0)
        {
            if (shootTwice)
            {
                recoilTime = Random.Range(0.75f, 1.5f);
                Shoot();
                return;
            }

            //change state
        }
    }

    void Shoot()
    {
        //display parry particle
        //display attack particle

        newRef.position = enemy.player.transform.position;

        newRef.transform.position = new Vector3(newRef.transform.position.x, newRef.transform.position.y, enemy.transform.position.z);

        enemy.transform.LookAt(newRef);

        Object.Instantiate(enemy.GetBullet(), enemy.transform.position, Quaternion.identity).GetComponent<ShooterProjectile>().SetMovementDirection(enemy.player.transform.position - enemy.transform.position);
    }
}
