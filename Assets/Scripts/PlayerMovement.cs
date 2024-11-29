using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 10f;
    public bool canJump;
    public float colliderOffset = 0.5f;
    private Rigidbody2D rb;
    private Vector2 rayDirection;


    // Start is called before the first frame update
    void Start()
    {
        canJump = false;
        rb = GetComponent<Rigidbody2D>();

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        
    }

    void Update()
    {
        rayDirection = (movementSpeed > 0) ? Vector2.right : Vector2.left;
        if(Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            CheckJump();
        }

        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, rayDirection, 0.6f);
        for(int i = 0 ; i < hit.Length; i++)
        {
            if(hit[i].collider.name != "Player")
            {
                movementSpeed = -movementSpeed;
                break;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider != null)
        {
            canJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        canJump = false;
    }

    void CheckJump()
    {
        RaycastHit2D[] ray = Physics2D.RaycastAll(transform.position, transform.up, Mathf.Infinity);
        for(int i = 0 ; i < ray.Length; i++)
        {
            if(ray[i].collider.name != "Player")
            {
                Collider2D playerCollider = GetComponent<Collider2D>();
                Vector2 colliderExtents = playerCollider.bounds.extents;
                Vector2 adjustedPoint = ray[i].point - (Vector2)transform.up * colliderExtents.y;
                rb.MovePosition(adjustedPoint);
                rb.gravityScale = -rb.gravityScale;
                transform.rotation = Quaternion.Euler(0,0,transform.eulerAngles.z + 180);
                break;
            }
        }
    }
}
