using UnityEngine;

public class KeyInteractable : InteractableBase
{
    protected override void Interact ()
    {
        var state = questStateService.State;

        if (state.hasKey || state.hasQuestItem)
        {
            Debug.Log("Key: Already collected.");
            return;
        }

        questStateService.GiveKey();
        gameObject.SetActive(false);
        Debug.Log("Key: Collected.");
    }
}
