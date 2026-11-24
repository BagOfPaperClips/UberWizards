using UnityEngine;
using UnityEngine.EventSystems;

public class WandTest1 : MonoBehaviour
{
    [SerializeField] GameObject wiz;
    [SerializeField] Rigidbody2D rb;
    public float moveSpeed = 6f;
    private Vector2 moveInput;
    private float moveDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("YAW: "+Aurdino.newYaw);

        if (Aurdino.newYaw < -4)
        {
            moveDir = -1;
            Debug.Log("move LEFT");
        }
        else if (Aurdino.newYaw > 4)
        {
            moveDir = 1;
            Debug.Log("move RIGHT");
        }
        else if(Aurdino.newYaw > -3 && Aurdino.newYaw < 3)
        {
            moveDir = 0;
        }
        moveInput = new Vector2(moveDir, 0).normalized;
    }
    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
