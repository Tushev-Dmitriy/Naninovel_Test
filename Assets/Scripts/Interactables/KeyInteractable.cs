using UnityEngine;

public class KeyInteractable : InteractableBase
{
    protected override void Interact ()
    {
        PlayScript("KeyInteract");
    }
}
