using Naninovel;

[CommandAlias("hideStartupCover")]
public class HideStartupCoverCommand : Command
{
    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        StartupCover.Instance?.Hide();
        return UniTask.CompletedTask;
    }
}