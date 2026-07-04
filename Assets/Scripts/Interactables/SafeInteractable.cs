using UnityEngine;

public class SafeInteractable : InteractableBase
{
    protected override void Interact ()
    {
        PlayScript("SafeInteract");
    }
}
