using Naninovel;
using UnityEngine;

public class NaninovelBootstrap : MonoBehaviour
{
    [SerializeField] private string startupScriptName = "Main";

    private async void Start ()
    {
        await RuntimeInitializer.InitializeAsync();

        if (string.IsNullOrWhiteSpace(startupScriptName)) return;

        var player = Engine.GetService<IScriptPlayer>();
        if (player == null) return;

        player.Stop();
        await player.PreloadAndPlayAsync(startupScriptName);
    }
}
