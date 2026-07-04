using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    [SerializeField] protected QuestStateService questStateService;

    protected virtual void Awake ()
    {
        if (!questStateService) questStateService = FindFirstObjectByType<QuestStateService>();
    }

    public void TryInteract ()
    {
        if (!enabled || !gameObject.activeInHierarchy) return;
        if (!questStateService) return;

        Interact();
    }

    protected abstract void Interact ();
}
