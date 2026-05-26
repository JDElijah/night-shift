using UnityEngine;
using UnityEngine.InputSystem;


public interface IInteractable
{
     void Interact();
}

public class Interactor : MonoBehaviour
{

    public Transform InteractorSource;
    public float InteractRange;

    [Header("Input")]
    [SerializeField] private InputActionReference interactAction;

    private void OnEnable()
    {
        interactAction.action.performed += OnInteract;
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.performed -= OnInteract;
        interactAction.action.Disable();
    }


    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);

        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactObj.Interact();
            }
        }
    }
}
