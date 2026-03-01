using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float inputDeadzone = 0.01f;

    public float knockbackDuration = 0.2f;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movement;
    private string currentAnimation;
    private string lastDirection = "PlayerDown";

    private bool isAttacking = false;
    private string currentAttackAnimation;

    public int attackDamage = 1;

    private bool isKnockedBack = false;
    private float knockbackTimer;
    private Vector2 knockbackVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isKnockedBack)
            return;

        if (isAttacking)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            if (state.IsName(currentAttackAnimation) && state.normalizedTime >= 1f)
            {
                isAttacking = false;
                animator.Play(lastDirection, 0, 0f);
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartAttack();
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        HandleAnimation();
    }

    void FixedUpdate()
    {
        if (isKnockedBack)
        {
            rb.MovePosition(rb.position + knockbackVelocity * Time.fixedDeltaTime);

            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0)
            {
                isKnockedBack = false;
            }

            return;
        }

        if (isAttacking)
            return;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void StartAttack()
    {
        isAttacking = true;
        movement = Vector2.zero;

        currentAttackAnimation = lastDirection.Replace("Player", "PlayerAttack");
        animator.Play(currentAttackAnimation, 0, 0f);
    }

    void HandleAnimation()
    {
        if (movement.sqrMagnitude < inputDeadzone)
        {
            animator.Play(lastDirection, 0, 0f);
            return;
        }

        string newDirection;

        if (Mathf.Abs(movement.y) > Mathf.Abs(movement.x))
            newDirection = movement.y > 0 ? "PlayerUp" : "PlayerDown";
        else
            newDirection = movement.x > 0 ? "PlayerRight" : "PlayerLeft";

        lastDirection = newDirection;

        if (currentAnimation == newDirection)
            return;

        currentAnimation = newDirection;
        animator.Play(newDirection);
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        isKnockedBack = true;
        knockbackTimer = knockbackDuration;
        knockbackVelocity = direction * force;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAttacking)
            return;

        if (other.CompareTag("Enemy"))
        {
            EnemyWander enemy = other.GetComponent<EnemyWander>();

            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }
    }
}