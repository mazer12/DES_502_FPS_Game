using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCharacterController : MonoBehaviour
{
    [Header("Base setup")]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 15.0f;
    private float gravity = 20.0f;
    public float mouseSensitivity;
    private float verticalLookRotation;
    public GameObject cameraHolder;

    [Header("Animator")]
    public Animator anim;
    public bool isJumpingAnim;
    public bool isGroundedAnim;


    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    [SerializeField]
    private float cameraYOffset = 0.4f;
    //private float cameraZOffset = -1.0f;
    private Camera playerCamera;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


    }
    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }
    void Update()
    {
        Look();
        bool isRunning = false;

        // Press Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // We are grounded, so recalculate move direction based on axis
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedZ = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedZ);

        // Handle Animations
        if (moveDirection != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
        float inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);
        float speed = inputMagnitude * walkingSpeed;
        if (isRunning)
        {
            inputMagnitude = inputMagnitude * 2;
        }
        anim.SetFloat("Input Magnitude", inputMagnitude, 0.1f, Time.deltaTime);

        if (characterController.isGrounded)
        {
            anim.SetBool("isGrounded", true);
            anim.SetBool("isFalling", false);
            anim.SetBool("isJumping", false);

        }
        else
        {
            anim.SetBool("isGrounded", false);
            anim.SetBool("isFalling", true);
        }

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
            anim.SetBool("isJumping", true);
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Hold left Alt to glide
        if (Input.GetKey(KeyCode.LeftAlt) && !characterController.isGrounded)
        {
            gravity = 5.0f;
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else if (!characterController.isGrounded)
        {
            gravity = 20.0f;
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            gravity = 20.0f;
        }



        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Unlock Curser
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
