using UnityEngine;

public class ShooterEnemySM : EnemyTracking
{
    public ShooterStateShoot ShootState { get; private set; }

    [Header("Shooter Enemy Values")]
    [SerializeField] GameObject bullet;
    public GameObject GetBullet() { return bullet; }

    private void Awake()
    {
        Debug.Log("I can't add an instantiation for this state without modifying Tyler's code, I need to fix this later");

        Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<ShooterProjectile>().SetMovementDirection(base.player.transform.position - transform.position);
    }
}
