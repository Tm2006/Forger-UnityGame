using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private InputSystem_Actions input;
    private Rigidbody rb;
    private Vector2 moveInput;

    void Awake()
    {
        input = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();     
    }

    void OnEnable()
    {
        input.Player.Enable();
    }

    void OnDisable()
    {
        input.Player.Disable();
        moveInput = Vector2.zero;
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0); 
    }
    
    void OnDestroy()
    {
        input.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = input.Player.Move.ReadValue<Vector2>();    
    }

    void FixedUpdate()
    {
        Vector3 direction = transform.forward * moveInput.y + transform.right * moveInput.x;
        direction = Vector3.ClampMagnitude(direction, 1f);
        Vector3 velocity = direction * speed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }
}
