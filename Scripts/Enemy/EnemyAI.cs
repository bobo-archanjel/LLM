using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float jumpForce = 5f;

    [Header("Obstacle Detection")]
    public Transform lowCheck;   // Child pred nohami
    public Transform highCheck;  // Child vyööie
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;  // Tilemap ground layer

    // Ak sa prek·ûka "zmestÌ" do tohto "okna", tak sk·Ëeme
    // T.j. LowCheck je kolÌzia, ale HighCheck voænÈ
    // Ak aj highCheck je kolÌzne, stena je moc vysok· => otoË sa
    public float maxJumpableObstacleHeight = 1.0f; // NepouûitÈ, ale mohol by si pouûiù Raycast

    private Rigidbody2D rb;
    private Animator anim;

    private float direction = 1f;  // +1 doprava, -1 doæava
    private bool isJumping;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        PatrolMovement();
        UpdateAnimator();
    }

    private void PatrolMovement()
    {
        // Vûdy sa h˝be v smere direction
        float moveX = direction * moveSpeed;
        rb.velocity = new Vector2(moveX, rb.velocity.y);

        // Detekcia "nÌzkej prek·ûky" pred nohami
        bool lowObstacle = Physics2D.OverlapCircle(lowCheck.position, checkRadius, whatIsGround);
        // Detekcia "vysokej prek·ûky" vyööie
        bool highObstacle = Physics2D.OverlapCircle(highCheck.position, checkRadius, whatIsGround);

        if (lowObstacle)
        {
            // Ak je lowCheck blokovan˝ a highCheck voæn˝ => sk·Ëeme
            if (!highObstacle && !isJumping)
            {
                Jump();
            }
            else
            {
                // Znamen· to, ûe aj hore je prek·ûka => stena je prÌliö vysok·
                // OtoË sa do druhej strany
                FlipDirection();
            }
        }

        // Voliteæne: MÙûeö sem pridaù edgeCheck - ak uû nie je zem (raycast dole), otoË sa
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        anim.SetTrigger("Jump");
        isJumping = true;
    }

    private void FlipDirection()
    {
        // ZmenÌme direction na opaËn˝
        direction *= -1;

        // Flip sprite (ak pouûÌvaö scale flipping):
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    private void UpdateAnimator()
    {
        bool isWalking = Mathf.Abs(rb.velocity.x) > 0.1f && !isJumping;
        anim.SetBool("isWalking", isWalking);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Po dopade na zem/tilemap => uû nesk·Ëe
        if ((whatIsGround.value & 1 << collision.gameObject.layer) != 0)
        {
            isJumping = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (lowCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(lowCheck.position, checkRadius);
        }
        if (highCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(highCheck.position, checkRadius);
        }
    }
}
