using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TicTacToeController : MonoBehaviour
{
    public event Action<TicTacToeResult> Finished;

    public TicTacToeResult Result => result;

    private TMP_Text statusText;
    private Button retryButton;
    private Button[] cellButtons;
    private TMP_Text[] cellLabels;
    private readonly TicTacToeMark[] board = new TicTacToeMark[9];
    private readonly TicTacToeAi ai = new TicTacToeAi();
    private TicTacToeResult result;

    private void Awake ()
    {
        EnsureEventSystem();

        statusText = transform.Find("StatusText")?.GetComponent<TMP_Text>();
        retryButton = transform.Find("RetryButton")?.GetComponent<Button>();

        var boardTransform = transform.Find("Board");
        cellButtons = boardTransform != null
            ? boardTransform.GetComponentsInChildren<Button>(true)
                .OrderBy(button => button.name, StringComparer.Ordinal)
                .ToArray()
            : Array.Empty<Button>();

        cellLabels = cellButtons
            .Select(button => button.GetComponentInChildren<TMP_Text>(true))
            .ToArray();

        for (var i = 0; i < cellButtons.Length; i++)
        {
            var cellIndex = i;
            cellButtons[i].onClick.AddListener(() => OnCellClicked(cellIndex));
        }

        if (retryButton != null)
            retryButton.onClick.AddListener(ResetBoard);
    }

    private void OnEnable ()
    {
        ResetBoard();
    }

    public void ResetBoard ()
    {
        Array.Clear(board, 0, board.Length);
        result = TicTacToeResult.None;

        if (cellButtons == null || cellLabels == null) return;

        for (var i = 0; i < cellButtons.Length; i++)
        {
            cellButtons[i].interactable = true;
            if (cellLabels[i] != null)
                cellLabels[i].text = string.Empty;
        }

        if (retryButton != null)
            retryButton.gameObject.SetActive(false);

        RefreshStatus();
    }

    public void BeginSession ()
    {
        gameObject.SetActive(true);
        ResetBoard();
    }

    private void OnCellClicked (int cellIndex)
    {
        if (result != TicTacToeResult.None) return;
        if (board[cellIndex] != TicTacToeMark.None) return;
        if (cellLabels == null || cellIndex >= cellLabels.Length) return;
        if (cellLabels[cellIndex] == null) return;

        ApplyMove(cellIndex, TicTacToeMark.X);

        result = EvaluateResult();
        if (result != TicTacToeResult.None)
        {
            SetBoardInteractable(false);
            Finished?.Invoke(result);
        }
        else
            MakeAiMove();

        RefreshStatus();
    }

    private TicTacToeResult EvaluateResult ()
    {
        var winningLines = new[]
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

        foreach (var line in winningLines)
        {
            var first = board[line[0]];
            if (first == TicTacToeMark.None) continue;
            if (first == board[line[1]] && first == board[line[2]])
                return first == TicTacToeMark.X ? TicTacToeResult.XWins : TicTacToeResult.OWins;
        }

        return board.All(mark => mark != TicTacToeMark.None) ? TicTacToeResult.Draw : TicTacToeResult.None;
    }

    private void RefreshStatus ()
    {
        if (statusText == null) return;

        switch (result)
        {
            case TicTacToeResult.XWins:
                statusText.text = "Вы победили";
                break;
            case TicTacToeResult.OWins:
                statusText.text = "Поражение. Попробуйте снова";
                break;
            case TicTacToeResult.Draw:
                statusText.text = "Ничья. Попробуйте снова";
                break;
            default:
                statusText.text = "Ваш ход";
                break;
        }

        if (retryButton != null)
            retryButton.gameObject.SetActive(result == TicTacToeResult.OWins || result == TicTacToeResult.Draw);
    }

    private void SetBoardInteractable (bool interactable)
    {
        if (cellButtons == null) return;

        for (var i = 0; i < cellButtons.Length; i++)
            cellButtons[i].interactable = interactable && board[i] == TicTacToeMark.None;
    }

    private void MakeAiMove ()
    {
        var moveIndex = ai.ChooseMove(board);
        if (moveIndex < 0) return;

        ApplyMove(moveIndex, TicTacToeMark.O);
        result = EvaluateResult();

        if (result != TicTacToeResult.None)
        {
            SetBoardInteractable(false);
            Finished?.Invoke(result);
        }
    }

    private void ApplyMove (int cellIndex, TicTacToeMark mark)
    {
        board[cellIndex] = mark;
        cellLabels[cellIndex].text = mark.ToString();
        cellButtons[cellIndex].interactable = false;
    }

    private void EnsureEventSystem ()
    {
        if (EventSystem.current != null) return;

        var eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.AddComponent<EventSystem>();

        #if ENABLE_INPUT_SYSTEM && INPUT_SYSTEM_AVAILABLE
        eventSystemObject.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
        #else
        eventSystemObject.AddComponent<StandaloneInputModule>();
        #endif
    }
}
