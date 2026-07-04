using System;
using Naninovel;
using UnityEngine;

public class QuestStateService : MonoBehaviour
{
    public event Action StateChanged;

    private ICustomVariableManager variableManager;

    private void OnEnable ()
    {
        Engine.OnInitializationFinished += HandleEngineInitialized;

        if (Engine.Initialized)
            HandleEngineInitialized();
    }

    private void OnDisable ()
    {
        Engine.OnInitializationFinished -= HandleEngineInitialized;

        if (variableManager != null)
            variableManager.OnVariableUpdated -= HandleVariableUpdated;
    }

    public int GetCurrentLocation ()
    {
        return GetInt(QuestState.CurrentLocationVariable, 1);
    }

    public bool HasKey ()
    {
        return GetBool(QuestState.HasKeyVariable);
    }

    public bool HasQuestItem ()
    {
        return GetBool(QuestState.HasQuestItemVariable);
    }

    public string GetInventoryText ()
    {
        if (HasQuestItem()) return "Quest item";
        if (HasKey()) return "Key";
        return "Empty";
    }

    private void HandleEngineInitialized ()
    {
        if (variableManager != null)
            variableManager.OnVariableUpdated -= HandleVariableUpdated;

        variableManager = Engine.GetService<ICustomVariableManager>();

        if (variableManager != null)
            variableManager.OnVariableUpdated += HandleVariableUpdated;

        NotifyChanged();
    }

    private void HandleVariableUpdated (CustomVariableUpdatedArgs args)
    {
        if (args.Name.Equals(QuestState.TalkedToNpcVariable, StringComparison.OrdinalIgnoreCase) ||
            args.Name.Equals(QuestState.CheckedSafeVariable, StringComparison.OrdinalIgnoreCase) ||
            args.Name.Equals(QuestState.HasKeyVariable, StringComparison.OrdinalIgnoreCase) ||
            args.Name.Equals(QuestState.HasQuestItemVariable, StringComparison.OrdinalIgnoreCase) ||
            args.Name.Equals(QuestState.IsGameFinishedVariable, StringComparison.OrdinalIgnoreCase) ||
            args.Name.Equals(QuestState.CurrentLocationVariable, StringComparison.OrdinalIgnoreCase))
            NotifyChanged();
    }

    private bool GetBool (string variableName)
    {
        if (variableManager == null) return false;
        if (!variableManager.TryGetVariableValue(variableName, out bool value)) return false;
        return value;
    }

    private int GetInt (string variableName, int fallbackValue)
    {
        if (variableManager == null) return fallbackValue;
        if (!variableManager.TryGetVariableValue(variableName, out int value)) return fallbackValue;
        return value;
    }

    private void NotifyChanged ()
    {
        StateChanged?.Invoke();
    }
}
