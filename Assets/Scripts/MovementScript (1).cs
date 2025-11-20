using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class RootMotionMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speedMultiplier = 1.0f;
    public float rotationSpeed = 10f;
    public float jumpSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Air Settings")]
    public float airControl = 0.5f;  // 0 = no control, 1 = full control
    public float airDecay = 1.5f;    // how fast momentum fades when no input

    private CharacterController controller;
    private Animator animator;
    private Camera mainCamera;

    private Vector3 moveDirection;
    private Vector3 lastGroundedMove;
    private float verticalSpeed;
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
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Build camera-relative direction
        Vector3 camForward = mainCamera.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = mainCamera.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 inputDir = (v * camForward + h * camRight);

        if (inputDir.sqrMagnitude > 1f)
            inputDir.Normalize();

        inputDir *= speedMultiplier;

        if (isGrounded)
        {
            // Normal grounded movement
            moveDirection = inputDir;
            lastGroundedMove = moveDirection;

            // Jump
            if (Input.GetButtonDown("Jump"))
            {
                verticalSpeed = jumpSpeed;
                isGrounded = false;
            }
        }
        else
        {
            // --- AIR MOVEMENT LOGIC ---
            if (inputDir.magnitude > 0.01f)
            {
                // Air control: steer toward the input direction
                moveDirection = Vector3.Lerp(moveDirection, inputDir, airControl * Time.deltaTime);
            }
            else
            {
                // No input: fade momentum gradually
                moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, airDecay * Time.deltaTime);
            }
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
            verticalSpeed = -0.5f;
        }
    }

    void MoveCharacter()
    {
        Vector3 velocity = moveDirection;
        velocity.y = verticalSpeed;

        controller.Move(velocity * Time.deltaTime);
    }

    void UpdateAnimator()
    {
        Vector3 horizontal = moveDirection;
        horizontal.y = 0;

        animator.SetFloat("Speed", Mathf.Clamp01(horizontal.magnitude));
        animator.SetBool("IsGrounded", isGrounded);
    }

    void OnAnimatorMove()
    {
        // Root motion ONLY when grounded
        if (animator && isGrounded)
        {
            Vector3 delta = animator.deltaPosition;
            delta.y = 0f;
            controller.Move(delta);
        }
    }
}
