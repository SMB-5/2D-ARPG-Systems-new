using UnityEngine;

public class EnemyWander : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveTime = 2f;
    public float stopTime = 1.5f;
    public float knockbackForce = 5f;

    public int maxHealth = 3;
    private int currentHealth;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 moveDirection;
    private float timer;
    private bool isMoving = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        timer = moveTime;
        ChooseNewDirection();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (isMoving && timer <= 0)
        {
            isMoving = false;
            timer = stopTime;
            rb.linearVelocity = Vector2.zero;
            animator.Play("Enemy1Idle");
        }
        else if (!isMoving && timer <= 0)
        {
            isMoving = true;
            timer = moveTime;
            ChooseNewDirection();
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.linearVelocity = moveDirection * moveSpeed;
        }
    }

    void ChooseNewDirection()
    {
        int randomDirection = Random.Range(0, 4);

        switch (randomDirection)
        {
            case 0:
                moveDirection = Vector2.up;
                animator.Play("Enemy1MoveUp");
                break;
            case 1:
                moveDirection = Vector2.down;
                animator.Play("Enemy1MoveDown");
                break;
            case 2:
                moveDirection = Vector2.left;
                animator.Play("Enemy1MoveLeft");
                break;
            case 3:
                moveDirection = Vector2.right;
                animator.Play("Enemy1MoveRight");
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                Vector2 direction =
                    (collision.transform.position - transform.position).normalized;

                player.ApplyKnockback(direction, knockbackForce);
            }
        }
    }
}