using Naninovel;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHud : MonoBehaviour
{
    private const string KeySpritePath = "Naninovel/Characters/Key/Default";
    private const string QuestItemSpritePath = "Inventory/QuestItem";

    [SerializeField] private Image keyIcon;
    [SerializeField] private Image questItemIcon;

    private ICustomVariableManager variableManager;
    private bool subscribed;

    private void Awake ()
    {
        ApplySprites();
        RefreshVisibility();
    }

    private void OnEnable ()
    {
        TrySubscribe();
        RefreshVisibility();
    }

    private void OnDisable ()
    {
        if (!subscribed || variableManager == null)
            return;

        variableManager.OnVariableUpdated -= HandleVariableUpdated;
        subscribed = false;
    }

    private void Update ()
    {
        if (!subscribed)
            TrySubscribe();
    }

    private void TrySubscribe ()
    {
        if (!Engine.Initialized)
            return;

        variableManager ??= Engine.GetService<ICustomVariableManager>();
        if (variableManager == null || subscribed)
            return;

        variableManager.OnVariableUpdated += HandleVariableUpdated;
        subscribed = true;
        RefreshVisibility();
    }

    private void HandleVariableUpdated (CustomVariableUpdatedArgs args)
    {
        if (args.Name == "hasKey" || args.Name == "hasQuestItem" || args.Name == "isGameFinished")
            RefreshVisibility();
    }

    private void ApplySprites ()
    {
        AssignSprite(keyIcon, KeySpritePath);
        AssignSprite(questItemIcon, QuestItemSpritePath);
    }

    private void RefreshVisibility ()
    {
        if (keyIcon == null || questItemIcon == null)
            return;

        var hasKey = GetBool("hasKey");
        var hasQuestItem = GetBool("hasQuestItem") && !GetBool("isGameFinished");

        keyIcon.gameObject.SetActive(hasKey && keyIcon.sprite != null);
        questItemIcon.gameObject.SetActive(hasQuestItem && questItemIcon.sprite != null);
    }

    private bool GetBool (string variableName)
    {
        if (variableManager == null)
            return false;

        return variableManager.TryGetVariableValue(variableName, out bool value) && value;
    }

    private void AssignSprite (Image target, string resourcePath)
    {
        if (target == null)
            return;

        var sprite = Resources.Load<Sprite>(resourcePath);
        if (sprite == null)
        {
            var texture = Resources.Load<Texture2D>(resourcePath);
            if (texture != null)
                sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        }

        target.sprite = sprite;
        target.preserveAspect = true;
        target.gameObject.SetActive(false);
    }
}
