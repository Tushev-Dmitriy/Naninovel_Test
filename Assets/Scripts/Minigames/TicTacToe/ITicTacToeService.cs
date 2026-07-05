using Naninovel;

public interface ITicTacToeService : IEngineService
{
    UniTask PlayAsync (AsyncToken asyncToken = default);
}
