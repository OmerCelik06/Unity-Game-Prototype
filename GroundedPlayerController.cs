using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GroundedPlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float speed = 4.0f;
    public float gravity = -9.81f;

    [Header("Kamera Ayarlarý")]
    public Transform playerCamera;
    public float mouseSensitivity = 2.0f;

    private CharacterController controller;
    private Vector3 velocity;
    private float pitch = 0.0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        transform.Rotate(Vector3.up * mouseX);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}