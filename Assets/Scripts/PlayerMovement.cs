using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 10f;
    public bool canJump;
    public float colliderOffset = 0.5f;
    private Rigidbody2D rb;
    private Vector2 rayDirection;
    

    void Start()
    {
        canJump = false;
        rb = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
    }

    void Update()
    {
        float localOffset = ((int)transform.rotation.z == 0) ? 0.6f : -0.6f;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - localOffset), -transform.up, 0.2f);
        if(hit.collider == null)
            canJump = false;
        else
            canJump = true;
        
        rayDirection = (movementSpeed > 0) ? Vector2.right : Vector2.left;

        if(Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            CheckJump();
        }

        RaycastHit2D[] hitX = Physics2D.RaycastAll(transform.position, rayDirection, 0.6f);
        for(int i = 0 ; i < hitX.Length; i++)
        {
            if(hitX[i].collider.name != "Player")
            {
                movementSpeed = -movementSpeed;
                break;
            }
        }
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
                rb.rotation += 180;
                break;
            }
        }
    }
}
