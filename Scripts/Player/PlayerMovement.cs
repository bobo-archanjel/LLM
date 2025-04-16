using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float wallCdr;
    private float horizontalInpput;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;


    public GameObject inventory;
    //private bool canJump = true;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInpput = Input.GetAxis("Horizontal");

        // Otoèenie hráèa nalavo/napravo
        if (horizontalInpput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInpput < -0.01f)
            transform .localScale = new Vector3(-1, 1, 1);

        animator.SetBool("Run", horizontalInpput != 0);
        animator.SetBool("Grounded", isGrounded());

        //print(onWall());

        //wall jump logic
        if (wallCdr > 0.2f)
        {
           

            // Pohyb do¾ava a doprava
            body.velocity = new Vector2(horizontalInpput * speed, body.velocity.y);
            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 3;

            // Skok na jedno stlaèenie medzerníka alebo W
            if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)))
                Jump();
        }
        else
            wallCdr += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventory.activeSelf)
            {
                inventory.SetActive(false);
            }else
                inventory.SetActive(true);
        }
    }

        private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInpput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallCdr = 0;
        }
       
    }

        private void OnCollisionEnter2D(Collision2D collision)
        {
        }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;   
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInpput == 0 && isGrounded() && !onWall();
    }
} 
