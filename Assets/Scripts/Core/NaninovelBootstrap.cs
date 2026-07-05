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
        var stateManager = Engine.GetService<IStateManager>();
        if (player == null || stateManager == null) return;

        await stateManager.ResetStateAsync(() =>
        {
            player.Stop();
            return player.PreloadAndPlayAsync(startupScriptName);
        });
    }
}
