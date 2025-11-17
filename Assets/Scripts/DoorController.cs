using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public float openAngle = 90f;   // Amount to rotate
    public float speed = 2f;        // Rotation speed

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    public void Interact()
    {
        ToggleDoor();
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor());
    }

    System.Collections.IEnumerator RotateDoor()
    {
        Quaternion targetRot = isOpen ? openRotation : closedRotation;

        while (Quaternion.Angle(transform.rotation, targetRot) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * speed);
            yield return null;
        }

        transform.rotation = targetRot;
    }
}