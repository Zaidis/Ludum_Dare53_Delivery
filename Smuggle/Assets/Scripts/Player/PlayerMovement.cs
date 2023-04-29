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
    public Vector3 movement;
    [SerializeField]private bool canMove;
    private bool isGrounded; //on the ground, not in the air

    public void FixedUpdate() {
        if (canMove) {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

            Vector3 m = (transform.right * movement.x + transform.forward * movement.z) * speed;
            controller.Move(m * Time.deltaTime);

            if (!isGrounded) {
                Vector3 v = new Vector3(0, -2f, 0);
                controller.Move(v * Time.deltaTime);
            }
        }
    }

    public void Move(InputAction.CallbackContext context) {
        movement = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
    }
}
