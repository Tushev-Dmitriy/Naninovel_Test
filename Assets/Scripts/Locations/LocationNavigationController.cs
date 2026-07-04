using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocationNavigationController : MonoBehaviour
{
    [SerializeField] private GameObject location1Root;
    [SerializeField] private GameObject location2Root;
    [SerializeField] private GameObject location3Root;
    [SerializeField] private Transform gameplayUiRoot;

    private TMP_FontAsset fontAsset;

    private void Awake ()
    {
        ResolveReferences();
        BuildLocationView(location1Root, "Location 1", new Color(0.22f, 0.35f, 0.58f), new Color(0.95f, 0.75f, 0.45f), "NPC");
        BuildLocationView(location2Root, "Location 2", new Color(0.22f, 0.48f, 0.33f), new Color(0.85f, 0.85f, 0.9f), "SAFE");
        BuildLocationView(location3Root, "Location 3", new Color(0.53f, 0.29f, 0.2f), new Color(0.96f, 0.86f, 0.35f), "KEY");
        BuildNavigationView();
        ShowLocation1();
    }

    public void ShowLocation1 ()
    {
        SetActiveLocation(LocationId.Location1);
    }

    public void ShowLocation2 ()
    {
        SetActiveLocation(LocationId.Location2);
    }

    public void ShowLocation3 ()
    {
        SetActiveLocation(LocationId.Location3);
    }

    private void SetActiveLocation (LocationId locationId)
    {
        if (location1Root) location1Root.SetActive(locationId == LocationId.Location1);
        if (location2Root) location2Root.SetActive(locationId == LocationId.Location2);
        if (location3Root) location3Root.SetActive(locationId == LocationId.Location3);
    }

    private void ResolveReferences ()
    {
        if (!location1Root) location1Root = transform.Find("LocationsRoot/Location1Root")?.gameObject;
        if (!location2Root) location2Root = transform.Find("LocationsRoot/Location2Root")?.gameObject;
        if (!location3Root) location3Root = transform.Find("LocationsRoot/Location3Root")?.gameObject;
        if (!gameplayUiRoot) gameplayUiRoot = transform.Find("GameplayUIRoot");
        fontAsset = TMP_Settings.defaultFontAsset;
    }

    private void BuildLocationView (GameObject locationRoot, string locationTitle, Color backgroundColor, Color objectColor, string objectLabel)
    {
        if (!locationRoot || locationRoot.transform.Find("LocationCanvas")) return;

        var canvasObject = new GameObject("LocationCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasObject.transform.SetParent(locationRoot.transform, false);

        var canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 0;

        var scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);

        var background = CreateImage("Background", canvasObject.transform, backgroundColor, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        background.raycastTarget = false;

        var title = CreateText("LocationTitle", canvasObject.transform, locationTitle, 48, TextAlignmentOptions.Center, Color.white);
        var titleRect = title.rectTransform;
        titleRect.anchorMin = new Vector2(0.5f, 1f);
        titleRect.anchorMax = new Vector2(0.5f, 1f);
        titleRect.pivot = new Vector2(0.5f, 1f);
        titleRect.anchoredPosition = new Vector2(0f, -60f);
        titleRect.sizeDelta = new Vector2(520f, 70f);

        var objectPlaceholder = CreateImage($"{objectLabel}Placeholder", canvasObject.transform, objectColor, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(220f, 220f));
        objectPlaceholder.rectTransform.anchoredPosition = new Vector2(0f, -20f);

        var objectText = CreateText($"{objectLabel}Label", objectPlaceholder.transform, objectLabel, 32, TextAlignmentOptions.Center, new Color(0.12f, 0.12f, 0.12f));
        objectText.rectTransform.anchorMin = Vector2.zero;
        objectText.rectTransform.anchorMax = Vector2.one;
        objectText.rectTransform.offsetMin = Vector2.zero;
        objectText.rectTransform.offsetMax = Vector2.zero;
    }

    private void BuildNavigationView ()
    {
        if (!gameplayUiRoot || gameplayUiRoot.Find("NavigationCanvas")) return;

        var canvasObject = new GameObject("NavigationCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasObject.transform.SetParent(gameplayUiRoot, false);

        var canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;

        var scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);

        var panel = CreateImage("NavigationPanel", canvasObject.transform, new Color(0f, 0f, 0f, 0.45f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(680f, 120f));
        panel.rectTransform.anchoredPosition = new Vector2(0f, 40f);

        CreateNavigationButton(panel.transform, "Location1Button", "Location 1", new Vector2(-220f, 0f), ShowLocation1);
        CreateNavigationButton(panel.transform, "Location2Button", "Location 2", new Vector2(0f, 0f), ShowLocation2);
        CreateNavigationButton(panel.transform, "Location3Button", "Location 3", new Vector2(220f, 0f), ShowLocation3);
    }

    private void CreateNavigationButton (Transform parent, string objectName, string label, Vector2 anchoredPosition, UnityEngine.Events.UnityAction onClick)
    {
        var buttonObject = new GameObject(objectName, typeof(RectTransform), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(parent, false);

        var buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
        buttonRect.pivot = new Vector2(0.5f, 0.5f);
        buttonRect.anchoredPosition = anchoredPosition;
        buttonRect.sizeDelta = new Vector2(180f, 52f);

        var image = buttonObject.GetComponent<Image>();
        image.color = new Color(0.22f, 0.22f, 0.22f, 0.95f);

        var button = buttonObject.GetComponent<Button>();
        var colors = button.colors;
        colors.normalColor = new Color(1f, 1f, 1f, 1f);
        colors.highlightedColor = new Color(0.92f, 0.92f, 0.92f, 1f);
        colors.pressedColor = new Color(0.75f, 0.75f, 0.75f, 1f);
        colors.selectedColor = new Color(0.92f, 0.92f, 0.92f, 1f);
        colors.disabledColor = new Color(0.6f, 0.6f, 0.6f, 1f);
        button.colors = colors;
        button.onClick.AddListener(onClick);

        var text = CreateText("Label", buttonObject.transform, label, 26, TextAlignmentOptions.Center, Color.white);
        text.rectTransform.anchorMin = Vector2.zero;
        text.rectTransform.anchorMax = Vector2.one;
        text.rectTransform.offsetMin = Vector2.zero;
        text.rectTransform.offsetMax = Vector2.zero;
    }

    private Image CreateImage (string objectName, Transform parent, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 sizeDelta)
    {
        var imageObject = new GameObject(objectName, typeof(RectTransform), typeof(Image));
        imageObject.transform.SetParent(parent, false);

        var rectTransform = imageObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.pivot = pivot;

        if (anchorMin == Vector2.zero && anchorMax == Vector2.one)
        {
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
        else
        {
            rectTransform.sizeDelta = sizeDelta;
        }

        var image = imageObject.GetComponent<Image>();
        image.color = color;
        return image;
    }

    private TextMeshProUGUI CreateText (string objectName, Transform parent, string textValue, float fontSize, TextAlignmentOptions alignment, Color color)
    {
        var textObject = new GameObject(objectName, typeof(RectTransform), typeof(TextMeshProUGUI));
        textObject.transform.SetParent(parent, false);

        var text = textObject.GetComponent<TextMeshProUGUI>();
        text.text = textValue;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = color;

        if (fontAsset) text.font = fontAsset;

        return text;
    }
}
