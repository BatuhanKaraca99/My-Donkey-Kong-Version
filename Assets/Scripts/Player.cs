using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite climbSprite;
    private int spriteIndex;

    private new Rigidbody2D rigidbody; // rigidbody reference
    private new Collider2D collider; //for size

    private Collider2D[] results; //variable
    private Vector2 direction; //direction reference

    public float moveSpeed = 1f; //movement speed
    public float jumpStrenght = 1f;

    private bool grounded;
    private bool climbing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4]; //maximum overlap
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite),1f/12f,1f/12f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void CheckCollision() //is grounded
    {
        grounded = false;
        climbing = false;

        Vector2 size = collider.bounds.size; //our character's collider size
        size.y += 0.1f;
        size.x /= 2f;
        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, results); //not allocate extra memory
        
        for(int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject; //get the gameobject we hit

            if(hit.layer == LayerMask.NameToLayer("Ground"))
            {
                grounded = hit.transform.position.y < (transform.position.y - 0.5f); //if platform below to us, we are changing pivot point too(for more accurate results)

                Physics2D.IgnoreCollision(collider, results[i], !grounded); //character's collision with top areas' collision
            }
            else if(hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                climbing = true;
            }
        }
    }

    private void Update()
    {
        CheckCollision();

        if (climbing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed; //go up
        }
        else if (grounded && Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpStrenght;
        }
        else
        {
            direction += Physics2D.gravity * Time.deltaTime; //adding gravity
        }

        direction.x = Input.GetAxis("Horizontal") * moveSpeed;
        if (grounded)
        {
            direction.y = Mathf.Max(direction.y, -1f); //which one is bigger(-1> -2,-3,-4...)
        }

        if (direction.x > 0f) //moving right
        {
            transform.eulerAngles = Vector3.zero; //don't rotate y axis
        } 
        else if (direction.x < 0f) //moving left
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f); //rotate 180 degree y axis
        }
    }

    private void FixedUpdate() //for physics
    {
        //rigidbody movement(current + direction with time)
        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime); 
    }

    private void AnimateSprite() 
    {
        if (climbing)
        {
            spriteRenderer.sprite = climbSprite;
        }
        else if(direction.x != 0f) //if moving
        {
            spriteIndex++;

            if (spriteIndex >= runSprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = runSprites[spriteIndex];
        }
    }
}
