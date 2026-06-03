using UnityEngine;
using UnityEngine.InputSystem;

/*
    interface IInteractable defines behavior for any object that the player can interact with.

    Any object that implements this interface must provide its own Interact logic. 
    For example: opening a door, toggling a light, picking up an item, etc. 
 */
public interface IInteractable
{
     void Interact();
}

/*
    Interactor.cs handles player interaction by casting a ray from a source point, 
    usually the player camera, and checking whether the object hit implements
    IInteractable.
 */

public class Interactor : MonoBehaviour
{
    [Header("Interaction")]
    public Transform InteractorSource;
    public float InteractRange;

    [Header("Input")]
    [SerializeField] private InputActionReference interactAction;

    private void OnEnable()
    {
        interactAction.action.performed += OnInteract;          // Subscribe to the interact input event when this object becomes active. 
        interactAction.action.Enable();                         // Enable the input action so it can listen for player input.
    }

    private void OnDisable()
    {
        interactAction.action.performed -= OnInteract;          // Unsubscribe from the input event to avoid duplicate callbacks or input being handled after this object is disabled.
        interactAction.action.Disable();                        // Disable the input action when this object is no longer active. 
    }

    // Called when the player presses the interact input.
    // Casts a ray forward and interacts with the object hit, if possible. 
    public void OnInteract(InputAction.CallbackContext context)
    {
        // Ifnore the callback unless this is the performed input phase. 
        if (!context.performed)
        {
            return;
        }

        // Create ray starting from interaction source and pointing forward.
        // Commonly placed on player camera for first-person interaction. 
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);

        // Check if the ray hits an object within the interaction range. 
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            // If object has component that implements IInteractable, call its Interact method.
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactObj.Interact();
            }
        }
    }
}
