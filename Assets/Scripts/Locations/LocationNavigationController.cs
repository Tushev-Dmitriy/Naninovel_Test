using Naninovel;
using UnityEngine;
using UnityEngine.UI;

public class LocationNavigationController : MonoBehaviour
{
    [SerializeField] private GameObject location1Root;
    [SerializeField] private GameObject location2Root;
    [SerializeField] private GameObject location3Root;
    [SerializeField] private Button location1Button;
    [SerializeField] private Button location2Button;
    [SerializeField] private Button location3Button;
    [SerializeField] private GameObject keyObject;
    [SerializeField] private QuestStateService questStateService;

    private void Awake ()
    {
        ResolveReferences();
        BindButtons();
        RefreshView();
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

    public void ShowLocation1 ()
    {
        PlayScript("Location1");
    }

    public void ShowLocation2 ()
    {
        PlayScript("Location2");
    }

    public void ShowLocation3 ()
    {
        PlayScript("Location3");
    }

    private void RefreshView ()
    {
        var locationId = questStateService ? questStateService.GetCurrentLocation() : 1;

        if (location1Root) location1Root.SetActive(locationId == 1);
        if (location2Root) location2Root.SetActive(locationId == 2);
        if (location3Root) location3Root.SetActive(locationId == 3);
        if (keyObject && questStateService)
            keyObject.SetActive(!questStateService.HasKey() && !questStateService.HasQuestItem());
    }

    private void ResolveReferences ()
    {
        if (!location1Root) location1Root = transform.Find("LocationsRoot/Location1Root")?.gameObject;
        if (!location2Root) location2Root = transform.Find("LocationsRoot/Location2Root")?.gameObject;
        if (!location3Root) location3Root = transform.Find("LocationsRoot/Location3Root")?.gameObject;
        if (!keyObject) keyObject = transform.Find("LocationsRoot/Location3Root/KeyPlaceholder")?.gameObject;
        if (!location1Button) location1Button = transform.Find("GameplayUIRoot/GameplayCanvas/NavigationPanel/Location1Button")?.GetComponent<Button>();
        if (!location2Button) location2Button = transform.Find("GameplayUIRoot/GameplayCanvas/NavigationPanel/Location2Button")?.GetComponent<Button>();
        if (!location3Button) location3Button = transform.Find("GameplayUIRoot/GameplayCanvas/NavigationPanel/Location3Button")?.GetComponent<Button>();
        if (!questStateService) questStateService = GetComponent<QuestStateService>();
    }

    private void BindButtons ()
    {
        if (location1Button)
        {
            location1Button.onClick.RemoveListener(ShowLocation1);
            location1Button.onClick.AddListener(ShowLocation1);
        }

        if (location2Button)
        {
            location2Button.onClick.RemoveListener(ShowLocation2);
            location2Button.onClick.AddListener(ShowLocation2);
        }

        if (location3Button)
        {
            location3Button.onClick.RemoveListener(ShowLocation3);
            location3Button.onClick.AddListener(ShowLocation3);
        }
    }

    private void PlayScript (string scriptName)
    {
        if (!Engine.Initialized) return;

        var player = Engine.GetService<IScriptPlayer>();
        if (player == null) return;

        player.Stop();
        player.PreloadAndPlayAsync(scriptName).Forget();
    }
}
