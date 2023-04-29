using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
    [Range(0, 1)]
    [SerializeField] private float sensitivity;
    private float xRot = 0f;
    private float x, y;

    [SerializeField] private Transform normalHeight;
    [SerializeField] private Transform crouchHeight;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void LateUpdate() {
        xRot -= y;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        transform.parent.Rotate(Vector3.up * x);
    }
    public void CamMovement(InputAction.CallbackContext context) {
        x = context.ReadValue<Vector2>().x * sensitivity / 2;
        y = context.ReadValue<Vector2>().y * sensitivity / 2;
    }

    public IEnumerator CameraCrouchHeight() {
        var i = 0f;
        float speed = 7f;
        while (i < 1f) {
            i += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(normalHeight.position, crouchHeight.position, i);
            yield return null;
        }
        transform.position = crouchHeight.position;
    }

    public IEnumerator CameraNormalHeight() {
        var i = 0f;
        float speed = 7f;
        while (i < 1f) {
            i += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(crouchHeight.position, normalHeight.position, i);
            yield return null;
        }
        transform.position = normalHeight.position;
    }

}
