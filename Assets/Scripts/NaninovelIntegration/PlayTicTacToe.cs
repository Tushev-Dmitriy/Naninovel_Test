using System;
using Naninovel;

namespace Naninovel.Commands
{
    [CommandAlias("playTicTacToe")]
    public class PlayTicTacToe : Command, Command.IForceWait
    {
        public override async UniTask ExecuteAsync (AsyncToken asyncToken = default)
        {
            var ticTacToeService = Engine.GetService<ITicTacToeService>();
            if (ticTacToeService == null)
                throw new InvalidOperationException("ITicTacToeService is not registered in Naninovel engine.");

            await ticTacToeService.PlayAsync(asyncToken);
        }
    }
}
