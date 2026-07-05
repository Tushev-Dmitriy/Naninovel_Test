using System.Collections.Generic;
using UnityEngine;

public class TicTacToeAi
{
    private static readonly int[][] WinningLines =
    {
        new[] { 0, 1, 2 },
        new[] { 3, 4, 5 },
        new[] { 6, 7, 8 },
        new[] { 0, 3, 6 },
        new[] { 1, 4, 7 },
        new[] { 2, 5, 8 },
        new[] { 0, 4, 8 },
        new[] { 2, 4, 6 }
    };

    public int ChooseMove (IReadOnlyList<TicTacToeMark> board)
    {
        var winningMove = FindLineMove(board, TicTacToeMark.O);
        if (winningMove >= 0) return winningMove;

        var blockingMove = FindLineMove(board, TicTacToeMark.X);
        if (blockingMove >= 0) return blockingMove;

        if (board[4] == TicTacToeMark.None) return 4;

        var emptyCells = new List<int>();
        for (var i = 0; i < board.Count; i++)
            if (board[i] == TicTacToeMark.None)
                emptyCells.Add(i);

        return emptyCells.Count > 0 ? emptyCells[Random.Range(0, emptyCells.Count)] : -1;
    }

    private int FindLineMove (IReadOnlyList<TicTacToeMark> board, TicTacToeMark mark)
    {
        foreach (var line in WinningLines)
        {
            var markCount = 0;
            var emptyIndex = -1;

            for (var i = 0; i < line.Length; i++)
            {
                var cellIndex = line[i];
                if (board[cellIndex] == mark)
                    markCount++;
                else if (board[cellIndex] == TicTacToeMark.None)
                    emptyIndex = cellIndex;
            }

            if (markCount == 2 && emptyIndex >= 0)
                return emptyIndex;
        }

        return -1;
    }
}
