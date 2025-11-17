using UnityEngine;

public class Interactor : MonoBehaviour
{
    [Header("Raycast Settings")]
    public Transform interactorSource;   // Camera or origin point
    public float interactRange = 3f;

    [Header("UI Prompt")]
    public GameObject interactPrompt;    // UI Text (Press E)

    IInteractable currentInteractable = null;

    void Update()
    {
        HandleRaycast();
        HandleInput();
    }

    void HandleRaycast()
    {
        currentInteractable = null;

        Ray r = new Ray(interactorSource.position, interactorSource.forward);

        // Raycast
        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
        {
            // Check for interactable
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                currentInteractable = interactObj;
                interactPrompt.SetActive(true);
                return;
            }
        }

        // Nothing interactable → hide prompt
        interactPrompt.SetActive(false);
    }

    void HandleInput()
    {
        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.Interact();
        }
    }

    private void OnDrawGizmos()
    {
        if (interactorSource != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(interactorSource.position, interactorSource.forward * interactRange);
        }
    }
}
