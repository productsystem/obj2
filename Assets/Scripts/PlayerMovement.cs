using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 10f;
    public bool canJump;
    public float colliderOffset = 0.5f;
    private Rigidbody2D rb;

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
        if(Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            CheckJump();
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
                rb.MovePosition(ray[i].point);
                rb.gravityScale = -rb.gravityScale;
                transform.rotation = Quaternion.Euler(0,0,transform.eulerAngles.z + 180);
            }
        }
    }
}
