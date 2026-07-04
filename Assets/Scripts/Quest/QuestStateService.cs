using System;
using UnityEngine;

public class QuestStateService : MonoBehaviour
{
    [SerializeField] private QuestState state = new QuestState();

    public event Action StateChanged;

    public QuestState State => state;

    private void Start ()
    {
        NotifyChanged();
    }

    public void MarkTalkedToNpc ()
    {
        if (state.talkedToNpc) return;

        state.talkedToNpc = true;
        NotifyChanged();
    }

    public void MarkCheckedSafe ()
    {
        if (state.checkedSafe) return;

        state.checkedSafe = true;
        NotifyChanged();
    }

    public void GiveKey ()
    {
        if (state.hasKey) return;

        state.hasKey = true;
        NotifyChanged();
    }

    public void RemoveKey ()
    {
        if (!state.hasKey) return;

        state.hasKey = false;
        NotifyChanged();
    }

    public void GiveQuestItem ()
    {
        if (state.hasQuestItem) return;

        state.hasQuestItem = true;
        NotifyChanged();
    }

    public void FinishGame ()
    {
        if (state.isGameFinished) return;

        state.isGameFinished = true;
        NotifyChanged();
    }

    public string GetInventoryText ()
    {
        if (state.hasQuestItem) return "Quest item";
        if (state.hasKey) return "Key";
        return "Empty";
    }

    private void NotifyChanged ()
    {
        StateChanged?.Invoke();
    }
}
