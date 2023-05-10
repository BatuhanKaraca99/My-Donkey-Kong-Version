using UnityEngine;

public class Player : MonoBehaviour
{
    private new Rigidbody2D rigidbody; // rigidbody reference
    private Vector2 direction; //direction reference
    public float moveSpeed = 1f; //movement speed

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        direction.x = Input.GetAxis("Horizontal") * moveSpeed;
        //direction.y = Input.GetAxis("Vertical") * moveSpeed;

        if(direction.x > 0f) //moving right
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
}
