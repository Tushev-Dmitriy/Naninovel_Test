using System;
using Naninovel;
using UnityEngine.SceneManagement;

[InitializeAtRuntime]
public class TicTacToeService : ITicTacToeService
{
    private const string SceneName = "TicTacToeScene";
    private const string WinVariableName = "ticTacToeWon";

    private bool playing;

    public UniTask InitializeServiceAsync ()
    {
        return UniTask.CompletedTask;
    }

    public void ResetService () { }

    public void DestroyService () { }

    public async UniTask PlayAsync (AsyncToken asyncToken = default)
    {
        if (playing)
            throw new InvalidOperationException("TicTacToe session is already running.");

        playing = true;
        var inputManager = Engine.GetService<IInputManager>();
        var variableManager = Engine.GetService<ICustomVariableManager>();
        var initialInputState = inputManager != null && inputManager.ProcessInput;
        TicTacToeController controller = null;
        Scene scene = default;
        var completed = false;

        try
        {
            if (inputManager != null)
                inputManager.ProcessInput = false;
            if (variableManager != null)
                variableManager.SetVariableValue(WinVariableName, "false");

            scene = await LoadSceneAsync();
            controller = ResolveController(scene);
            if (controller == null)
                throw new InvalidOperationException("TicTacToeController was not found in TicTacToeScene.");

            controller.Finished += HandleFinished;
            controller.BeginSession();

            while (!completed)
            {
                asyncToken.ThrowIfCanceled();
                await AsyncUtils.WaitEndOfFrameAsync();
            }
        }
        finally
        {
            if (controller != null)
                controller.Finished -= HandleFinished;

            if (scene.IsValid() && scene.isLoaded)
                await SceneManager.UnloadSceneAsync(scene);

            if (inputManager != null)
                inputManager.ProcessInput = initialInputState;

            playing = false;
        }

        void HandleFinished (TicTacToeResult result)
        {
            if (result == TicTacToeResult.XWins)
            {
                if (variableManager != null)
                    variableManager.SetVariableValue(WinVariableName, "true");
                completed = true;
            }
        }
    }

    private async UniTask<Scene> LoadSceneAsync ()
    {
        var scene = SceneManager.GetSceneByName(SceneName);
        if (!scene.IsValid() || !scene.isLoaded)
        {
            await SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            scene = SceneManager.GetSceneByName(SceneName);
        }

        return scene;
    }

    private TicTacToeController ResolveController (Scene scene)
    {
        var roots = scene.GetRootGameObjects();
        for (var i = 0; i < roots.Length; i++)
        {
            var controller = roots[i].GetComponentInChildren<TicTacToeController>(true);
            if (controller != null)
                return controller;
        }

        return null;
    }
}
