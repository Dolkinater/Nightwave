using UnityEngine;

public class ShooterProjectile : MonoBehaviour
{
    Rigidbody projectile;

    [SerializeField] float speed;

    float updateSpeed;
    [SerializeField] int damage = 10;

    bool parried = false;

    private void Awake()
    {
        projectile = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, 3f);
        updateSpeed = speed / 10;
    }

    Vector3 direction;
    public void SetMovementDirection(Vector3 _direction)
    {
        direction = _direction;
    }

    private void Update()
    {
        updateSpeed += Time.deltaTime * 5;

        updateSpeed = Mathf.Clamp(updateSpeed, 0, speed);
    }

    private void FixedUpdate()
    {
        projectile.linearVelocity = direction * (parried ? -updateSpeed : updateSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Health a = other.GetComponent<Health>();

            bool attackParried;
            a.ChangeHealthBy(-damage, out attackParried);

            if (attackParried)
            {
                parried = true;
                return;
            }

            Destroy(gameObject);
        }
    }
}
