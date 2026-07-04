using UnityEngine;

public class NpcInteractable : InteractableBase
{
    protected override void Interact ()
    {
        PlayScript("NpcInteract");
    }
}
