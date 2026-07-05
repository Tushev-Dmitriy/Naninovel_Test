using UnityEngine;

public class StartupCover : MonoBehaviour
{
    public static StartupCover Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(true);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
