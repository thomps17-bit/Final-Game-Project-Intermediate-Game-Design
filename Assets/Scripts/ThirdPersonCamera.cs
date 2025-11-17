using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;          // Drag your player here in the inspector
    public Vector3 offset = new Vector3(0, 3, -6); // Camera position relative to player
    public float smoothSpeed = 10f;   // How smoothly camera follows
    public float rotationSpeed = 5f;  // How fast camera rotates with mouse

    private float yaw = 0f;
    private float pitch = 15f;        // Slight downward tilt

    void LateUpdate()
    {
        if (!target) return;

        // Mouse input for camera orbit
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -20f, 60f); // Limit looking too far up/down

        // Calculate rotation from yaw/pitch
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Apply offset behind the player
        Vector3 desiredPos = target.position + rotation * offset;

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smoothSpeed);

        // Always look at the player
        transform.LookAt(target.position + Vector3.up * 1.5f); // aim at chest height
    }
}
