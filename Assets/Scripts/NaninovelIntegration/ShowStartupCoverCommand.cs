using Naninovel;

[CommandAlias("showStartupCover")]
public class ShowStartupCoverCommand : Command
{
    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        StartupCover.Instance?.Show();
        return UniTask.CompletedTask;
    }
}