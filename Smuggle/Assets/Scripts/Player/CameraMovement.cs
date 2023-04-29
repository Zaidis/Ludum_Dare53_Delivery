using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float sensitivity;
    private float xRot = 0f;
    private float x, y;
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void LateUpdate() {
        xRot -= y;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        transform.parent.Rotate(Vector3.up * x);
    }
    /*
    public void CamX(InputAction.CallbackContext context) {
        x = context.ReadValue<float>() * sensitivity / 2;
    }

    public void CamY(InputAction.CallbackContext context) {
        y = context.ReadValue<float>() * sensitivity / 2;
    }*/

    public void CamMovement(InputAction.CallbackContext context) {
        x = context.ReadValue<Vector2>().x * sensitivity / 2;
        y = context.ReadValue<Vector2>().y * sensitivity / 2;
    }

}
