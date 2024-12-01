using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private AudioClip jumpSound;
    public static int matrices = 0;
    private bool matrixGot;
    public float movementSpeed = 10f;
    public bool canJump;
    public float colliderOffset = 0.5f;
    private Rigidbody2D rb;
    private Vector2 rayDirection;
    public bool flipDir;
    private bool isReloading = false;
    

    void Start()
    {
        matrixGot = false;
        flipDir = false;
        canJump = false;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
    }

    void Update()
    {
        CanJump();
        FlipTiles();
        
        rayDirection = (movementSpeed > 0) ? Vector2.right : Vector2.left;

        if(Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Teleport();
            flipDir = !flipDir;
        }

        Bounce();
    }





    void FlipTiles()
    {
        GameObject[] orang = GameObject.FindGameObjectsWithTag("Orange Flip");
        foreach(var f in orang)
        {
            if(flipDir)
            {
                f.GetComponent<SpriteRenderer>().enabled = false;
                f.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                f.GetComponent<SpriteRenderer>().enabled = true;
                f.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        GameObject[] pink = GameObject.FindGameObjectsWithTag("Pink Flip");
        foreach(var f in pink)
        {
            if(flipDir)
            {
                f.GetComponent<SpriteRenderer>().enabled = true;
                f.GetComponent<BoxCollider2D>().enabled = true;
            }
            else
            {
                f.GetComponent<SpriteRenderer>().enabled = false;
                f.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    void CanJump()
    {
        float localOffset = ((int)transform.rotation.z == 0) ? 0.6f : -0.6f;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - localOffset), -transform.up, 0.2f);
        if(hit.collider == null)
            canJump = false;
        else
            canJump = true;
    }

    void Bounce()
    {
        RaycastHit2D[] hitX = Physics2D.RaycastAll(transform.position, rayDirection, 0.6f);
        for(int i = 0 ; i < hitX.Length; i++)
        {
            if(hitX[i].collider.name != "Player" && !hitX[i].collider.CompareTag("Hazard") && !hitX[i].collider.CompareTag("Goal") && !hitX[i].collider.CompareTag("Matrix"))
            {
                GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Bounce");
                movementSpeed = -movementSpeed;
                break;
            }
        }
    }

    void Teleport()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Jump");
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Exit")
        {
            Application.Quit();
        }
        if(other.CompareTag("Hazard") && !isReloading)
        {
            isReloading = true;
            if(matrixGot)
            {
                matrices--;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if(other.CompareTag("Goal"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if(other.CompareTag("Key"))
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Key");
            GameObject[] locks = GameObject.FindGameObjectsWithTag("Lock Door");
            foreach(var l in locks)
            {
                l.SetActive(false);
            }
            GameObject key = other.gameObject;
            Destroy(key);
        }
        if(other.CompareTag("Matrix"))
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Collect");
            matrices++;
            matrixGot = true;
            Destroy(other.gameObject);
        }   
    }
}
