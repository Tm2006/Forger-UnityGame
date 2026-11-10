using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 0.15f;
    private float pitch = 0f;

    private InputSystem_Actions input;
    
    private Camera playerCamera;

    void Awake()
    {
        input = new InputSystem_Actions();
        playerCamera = GetComponentInChildren<Camera>();
    }

    void OnEnable()
    {
        input.Player.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        input.Player.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnDestroy()
    {
        input.Dispose();
    }


    // Update is called once per frame
    void Update()
    {
       Vector2 mouseDelta = input.Player.Look.ReadValue<Vector2>();
       float mouseX = mouseDelta.x * mouseSensitivity;
       float mouseY = mouseDelta.y * mouseSensitivity;

       transform.Rotate(Vector3.up * mouseX);
       pitch -= mouseY;
       pitch = Mathf.Clamp(pitch, -90f, 90f);
       playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
