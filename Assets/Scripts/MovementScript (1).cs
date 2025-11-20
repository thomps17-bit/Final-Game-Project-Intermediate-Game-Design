using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class RootMotionMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speedMultiplier = 1.0f;   // scales horizontal movement
    public float rotationSpeed = 10f;      // rotation smoothing
    public float jumpSpeed = 5f;           // jump strength
    public float gravity = -9.81f;

    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;

    private Vector3 moveDirection; // horizontal movement
    private float verticalSpeed;   // vertical velocity
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        HandleInput();
        HandleRotation();
        ApplyGravity();
        MoveCharacter();
        UpdateAnimator();
    }

    void HandleInput()
    {
        // Only allow horizontal movement when grounded
        if (isGrounded)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 camForward = mainCamera.transform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = mainCamera.transform.right;
            camRight.y = 0;
            camRight.Normalize();

            moveDirection = v * camForward + h * camRight;

            if (moveDirection.sqrMagnitude > 1f)
                moveDirection.Normalize();

            moveDirection *= speedMultiplier;

            // Jump
            if (Input.GetButtonDown("Jump"))
            {
                verticalSpeed = jumpSpeed;
                isGrounded = false;
            }
        }
        else
        {
            // Lock horizontal movement midair
            moveDirection = Vector3.zero;
        }
    }

    void HandleRotation()
    {
        Vector3 horizontalMove = moveDirection;
        horizontalMove.y = 0;

        if (horizontalMove.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalMove);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            verticalSpeed += gravity * Time.deltaTime;
        }
        else if (verticalSpeed < 0)
        {
            verticalSpeed = -0.5f; // small downward force to stick to ground
        }
    }

    void MoveCharacter()
    {
        Vector3 velocity = moveDirection;
        velocity.y = verticalSpeed;

        // Apply movement
        controller.Move(velocity * Time.deltaTime);
    }

    void UpdateAnimator()
    {
        // Root motion speed parameter based on horizontal movement
        Vector3 horizontalMove = moveDirection;
        horizontalMove.y = 0;

        float speedParam = Mathf.Clamp01(horizontalMove.magnitude);
        animator.SetFloat("Speed", speedParam);

        // Set jump parameter
        animator.SetBool("IsGrounded", isGrounded);
    }

    void OnAnimatorMove()
    {
        // Apply root motion for horizontal movement when grounded
        if (animator && isGrounded)
        {
            Vector3 delta = animator.deltaPosition;
            delta.y = 0f;
            controller.Move(delta);
        }
    }
}
