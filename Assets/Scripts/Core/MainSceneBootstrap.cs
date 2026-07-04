using UnityEngine;

public class MainSceneBootstrap : MonoBehaviour
{
    [SerializeField] private Transform locationsRoot;
    [SerializeField] private Transform gameplayUiRoot;
    [SerializeField] private Transform location1Root;
    [SerializeField] private Transform location2Root;
    [SerializeField] private Transform location3Root;
    [SerializeField] private Transform minigameRoot;

    public Transform LocationsRoot => locationsRoot;
    public Transform GameplayUiRoot => gameplayUiRoot;
    public Transform Location1Root => location1Root;
    public Transform Location2Root => location2Root;
    public Transform Location3Root => location3Root;
    public Transform MinigameRoot => minigameRoot;
}
