using Naninovel;
using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    public void TryInteract ()
    {
        if (!enabled || !gameObject.activeInHierarchy) return;

        Interact();
    }

    protected abstract void Interact ();

    protected void PlayScript (string scriptName)
    {
        if (!Engine.Initialized) return;

        var player = Engine.GetService<IScriptPlayer>();
        if (player == null || player.Playing) return;

        player.PreloadAndPlayAsync(scriptName).Forget();
    }
}
