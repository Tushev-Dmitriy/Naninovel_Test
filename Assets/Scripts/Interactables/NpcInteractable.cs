using UnityEngine;

public class NpcInteractable : InteractableBase
{
    protected override void Interact ()
    {
        var state = questStateService.State;

        if (state.isGameFinished)
        {
            Debug.Log("NPC: Thanks again.");
            return;
        }

        if (state.hasQuestItem)
        {
            questStateService.FinishGame();
            Debug.Log("NPC: Thanks for the item.");
            return;
        }

        if (!state.talkedToNpc)
        {
            questStateService.MarkTalkedToNpc();
            Debug.Log("NPC: Please bring me the item from the safe.");
            return;
        }

        if (state.checkedSafe && !state.hasKey)
        {
            Debug.Log("NPC: The key may be on Location 3.");
            return;
        }

        Debug.Log("NPC: How is it going?");
    }
}
