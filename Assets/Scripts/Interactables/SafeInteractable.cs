using UnityEngine;

public class SafeInteractable : InteractableBase
{
    protected override void Interact ()
    {
        var state = questStateService.State;

        if (state.hasQuestItem)
        {
            Debug.Log("Safe: It is already open.");
            return;
        }

        if (!state.hasKey)
        {
            questStateService.MarkCheckedSafe();
            Debug.Log("Safe: It is locked. I need a key.");
            return;
        }

        questStateService.RemoveKey();
        questStateService.GiveQuestItem();
        Debug.Log("Safe: Opened. I found the quest item.");
    }
}
