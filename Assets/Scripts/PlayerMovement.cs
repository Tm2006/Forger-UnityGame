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
        input.Dispose();
        speed = 0f;
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
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
