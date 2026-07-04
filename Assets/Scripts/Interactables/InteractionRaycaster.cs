using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionRaycaster : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    private void Awake ()
    {
        if (!targetCamera) targetCamera = GetComponent<Camera>();
        if (!targetCamera) targetCamera = Camera.main;
    }

    private void Update ()
    {
        if (Mouse.current == null) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;
        if (!targetCamera) return;

        var ray = targetCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out var hit)) return;

        var interactable = hit.collider.GetComponent<InteractableBase>();

        if (!interactable)
            interactable = hit.collider.GetComponentInParent<InteractableBase>();

        if (interactable)
            interactable.TryInteract();
    }
}
