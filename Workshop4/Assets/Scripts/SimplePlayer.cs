using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayer : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    public Transform spawnPoint;
    public LayerMask groundLayer;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 7f;
    public float mouseSensitivity = 2f;

    [Header("Ground Check")]
    public float groundCheckDistance = 1.1f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpPressed;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        LookAround();
    }

    void FixedUpdate()
    {
        MovePlayer();
        Jump();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpPressed = true;
        }
    }

    void LookAround()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        transform.Rotate(Vector3.up * mouseX);
    }

    void MovePlayer()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move *= moveSpeed;

        Vector3 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(move.x, currentVelocity.y, move.z);
    }

    void Jump()
    {
        if (jumpPressed && IsGrounded())
        {
            Vector3 currentVelocity = rb.linearVelocity;
            rb.linearVelocity = new Vector3(currentVelocity.x, jumpForce, currentVelocity.z);
        }

        jumpPressed = false;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    public void Respawn()
    {
        rb.linearVelocity = Vector3.zero;

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }
    }
}