using UnityEngine;

public class DoorButton : MonoBehaviour, IInteractable
{
    [Header("Linked Door")]
    public DoorController linkedDoor;  // Drag the DoorController parent here

    [Header("Button Visual")]
    public Transform buttonVisual;       // Visual mesh of the button
    public Vector3 pressedOffset = new Vector3(0, -0.1f, 0); // Optional press movement

    private Vector3 originalPosition;

    void Start()
    {
        if (buttonVisual != null)
            originalPosition = buttonVisual.localPosition;
    }

    public void Interact()
    {
        if (linkedDoor != null)
        {
            linkedDoor.ToggleDoor();
            Debug.Log("Button pressed! Door toggled.");

            // Animate the button press
            if (buttonVisual != null)
                StartCoroutine(PressButton());
        }
    }

    System.Collections.IEnumerator PressButton()
    {
        Vector3 pressedPos = originalPosition + pressedOffset;

        // Move down
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 5f;
            buttonVisual.localPosition = Vector3.Lerp(originalPosition, pressedPos, t);
            yield return null;
        }

        // Move back up
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 5f;
            buttonVisual.localPosition = Vector3.Lerp(pressedPos, originalPosition, t);
            yield return null;
        }

        buttonVisual.localPosition = originalPosition;
    }
}
