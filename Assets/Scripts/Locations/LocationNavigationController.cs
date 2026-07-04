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

    private void Awake ()
    {
        ResolveReferences();
        BindButtons();
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
        if (!location1Button) location1Button = transform.Find("GameplayUIRoot/GameplayCanvas/NavigationPanel/Location1Button")?.GetComponent<Button>();
        if (!location2Button) location2Button = transform.Find("GameplayUIRoot/GameplayCanvas/NavigationPanel/Location2Button")?.GetComponent<Button>();
        if (!location3Button) location3Button = transform.Find("GameplayUIRoot/GameplayCanvas/NavigationPanel/Location3Button")?.GetComponent<Button>();
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
}
