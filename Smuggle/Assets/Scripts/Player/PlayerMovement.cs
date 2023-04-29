using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private CharacterController controller;

    [Header("Ground")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.4f;

    [Header("Player Variables")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private bool canMove;
    [SerializeField] private float gravity;
    
    public Vector3 movement;
    public Vector3 velocity;
    [SerializeField]private bool jumped;
    [SerializeField]private bool isGrounded; //on the ground, not in the air

    public void FixedUpdate() {
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
    public void Jump(InputAction.CallbackContext context) {
        if (context.performed && isGrounded) {
            jumped = context.action.triggered;
        } else if (context.canceled) {
            jumped = false;
        }
            
    }
}
