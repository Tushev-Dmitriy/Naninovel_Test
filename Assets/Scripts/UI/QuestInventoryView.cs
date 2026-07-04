using TMPro;
using UnityEngine;

public class QuestInventoryView : MonoBehaviour
{
    [SerializeField] private QuestStateService questStateService;
    [SerializeField] private TextMeshProUGUI inventoryValueText;

    private void Awake ()
    {
        ResolveReferences();
    }

    private void OnEnable ()
    {
        if (questStateService) questStateService.StateChanged += RefreshView;
        RefreshView();
    }

    private void OnDisable ()
    {
        if (questStateService) questStateService.StateChanged -= RefreshView;
    }

    public void RefreshView ()
    {
        if (inventoryValueText && questStateService)
            inventoryValueText.text = questStateService.GetInventoryText();
    }

    private void ResolveReferences ()
    {
        if (!questStateService) questStateService = FindFirstObjectByType<QuestStateService>();
        if (!inventoryValueText) inventoryValueText = transform.Find("InventoryValueText")?.GetComponent<TextMeshProUGUI>();
    }
}
