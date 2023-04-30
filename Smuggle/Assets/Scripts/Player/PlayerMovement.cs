using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    [SerializeField] private CharacterController controller;

    public movementType moveType = movementType.none;

    [Header("Ground")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.4f;

    [Header("Player Variables")]
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private bool canMove;
    [SerializeField] private float gravity;

    [SerializeField]private float speed;
    public Vector3 movement;
    public Vector3 velocity;
    private bool jumped;
    
    private bool isGrounded; //on the ground, not in the air
    private bool canSprint = true;

    [Header("Crouching")]
    private bool isCrouching;
    private bool canCrouch = true;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }
    private void Start() {
        speed = defaultSpeed;
    }

    public void FixedUpdate() {
        if (movement == Vector3.zero) moveType = movementType.none;
        else if (speed == defaultSpeed) moveType = movementType.walking;
        else if (speed == sprintSpeed) moveType = movementType.sprinting;
        else if (speed == crouchSpeed) moveType = movementType.crouching;


        if (canMove) {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

            if(isGrounded && velocity.y < 0f) {
                velocity.y = 0f;
            }
            Vector3 m = (transform.right * movement.x + transform.forward * movement.z) * speed;
            controller.Move(m * Time.deltaTime);

            if(jumped && isGrounded) {
                velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
    }

    public void Move(InputAction.CallbackContext context) {
        movement = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);

        
    }

    public void Test(InputAction.CallbackContext context) {
        if (context.action.triggered) {
            Debug.Log("Test");
            Guard g = FindObjectOfType<Guard>();
            //StartCoroutine(g.Patrol());
            StartCoroutine(g.LookAtTarget(this.gameObject.transform));
        }
    }

    public void Jump(InputAction.CallbackContext context) {
        if (context.performed && isGrounded) {
            jumped = context.action.triggered;
        } else if (context.canceled) {
            jumped = false;
        }
            
    }

    public void Sprint(InputAction.CallbackContext context) {
        if (context.performed && canSprint) {
            speed = sprintSpeed;
            canCrouch = false;
        } else if (context.canceled) {
            
            speed = defaultSpeed;
            canCrouch = true;
        }
    }

    public void Crouch(InputAction.CallbackContext context) {
        if (context.action.triggered) {
            if (canCrouch)
                ManageCrouching(isCrouching);
        }
    }

    private void ManageCrouching(bool crouch) {
        StopAllCoroutines();
        if (!crouch) { //begin crouching
            speed = crouchSpeed;
            canSprint = false;
            isCrouching = true;
            controller.height = 1f;
            controller.center = new Vector3(0, -0.5f, 0);
            StartCoroutine(CameraMovement.instance.CameraCrouchHeight());
        } else { //no longer crouching
            speed = defaultSpeed;
            canSprint = true;
            isCrouching = false;
            controller.center = Vector3.zero;
            controller.height = 2f;
            StartCoroutine(CameraMovement.instance.CameraNormalHeight());
        }
    }
}

public enum movementType {
    none,
    walking, 
    sprinting, 
    crouching,
    jumping,
}