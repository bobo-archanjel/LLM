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
    public Transform highCheck;  // Child vy��ie
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;  // Tilemap ground layer

    // Ak sa prek�ka "zmest�" do tohto "okna", tak sk��eme
    // T.j. LowCheck je kol�zia, ale HighCheck vo�n�
    // Ak aj highCheck je kol�zne, stena je moc vysok� => oto� sa
    public float maxJumpableObstacleHeight = 1.0f; // Nepou�it�, ale mohol by si pou�i� Raycast

    private Rigidbody2D rb;
    private Animator anim;

    private float direction = 1f;  // +1 doprava, -1 do�ava
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
        // V�dy sa h�be v smere direction
        float moveX = direction * moveSpeed;
        rb.velocity = new Vector2(moveX, rb.velocity.y);

        // Detekcia "n�zkej prek�ky" pred nohami
        bool lowObstacle = Physics2D.OverlapCircle(lowCheck.position, checkRadius, whatIsGround);
        // Detekcia "vysokej prek�ky" vy��ie
        bool highObstacle = Physics2D.OverlapCircle(highCheck.position, checkRadius, whatIsGround);

        if (lowObstacle)
        {
            // Ak je lowCheck blokovan� a highCheck vo�n� => sk��eme
            if (!highObstacle && !isJumping)
            {
                Jump();
            }
            else
            {
                // Znamen� to, �e aj hore je prek�ka => stena je pr�li� vysok�
                // Oto� sa do druhej strany
                FlipDirection();
            }
        }

        // Volite�ne: M��e� sem prida� edgeCheck - ak u� nie je zem (raycast dole), oto� sa
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        anim.SetTrigger("Jump");
        isJumping = true;
    }

    private void FlipDirection()
    {
        // Zmen�me direction na opa�n�
        direction *= -1;

        // Flip sprite (ak pou��va� scale flipping):
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
        // Po dopade na zem/tilemap => u� nesk��e
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
