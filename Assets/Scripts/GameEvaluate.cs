using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvaluate
{
    private static GameEvaluate instance = null;
    private GameEvaluate() { }
    public static GameEvaluate Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameEvaluate();
            }
            return instance;
        }
    }


    /// <summary>
    /// Get the evaluation of a game state.
    /// </summary>
    /// <returns>
    /// int.MaxValue if player 1 won, int.MinValue if player -1 won, 0 if game ended in a draw, and heuristic value if game has not ended.
    /// </returns>
    public int GetEval(BoardState state)
    {
        int w = CheckWin(state);
        if (w == 1)
            return int.MaxValue;
        if (w == -1)
            return int.MinValue;
        if (w == 2)
            return 0;
        var lastMove = state.lastMove;
        return HeuRow(state, lastMove.i, lastMove.j) + HeuCol(state, lastMove.i, lastMove.j) + HeuDia(state, lastMove.i, lastMove.j) + HeuAntiDia(state, lastMove.i, lastMove.j);
    }


    /// <summary>
    /// Check if game terminates. 
    /// </summary>
    /// <returns>
    /// 0 if the game has not ended, 2 if it ends in a draw, 1 is player 1 (x) win, -1 if player -1 (o) win.
    /// </returns>
    public int CheckWin(BoardState state)
    {
        int rowCount = 0;
        int colCount = 0;
        int diaCount = 0;
        int antiDiaCount = 0;
        var lasti = state.lastMove.i;
        var lastj = state.lastMove.j;
        int p = state.lastMove.player;
        var board = state.board;
        int m = state.winCondition;
        int n = state.boardSize;

        if (p == 0)
            return 0;
        //check row
        int i = lasti;
        while (i < n && board[i, lastj] == p)
        {
            i++;
            rowCount++;
            if (rowCount >= m)
                return p;
        }
        i = lasti - 1;
        while (i >= 0 && board[i, lastj] == p)
        {
            i--;
            rowCount++;
            if (rowCount >= m)
                return p;
        }

        //check column
        int j = lastj;
        while (j < n && board[lasti, j] == p)
        {
            j++;
            colCount++;
            if (colCount >= m)
                return p;
        }
        j = lastj - 1;
        while (j >= 0 && board[lasti, j] == p)
        {
            j--;
            colCount++;
            if (colCount >= m)
                return p;
        }

        //check diagnols
        i = lasti;
        j = lastj;
        while (i < n && j < n && board[i, j] == p)
        {
            i++;
            j++;
            diaCount++;
            if (diaCount >= m)
                return p;
        }
        i = lasti - 1;
        j = lastj - 1;
        while (i >= 0 && j >= 0 && board[i, j] == p)
        {
            i--;
            j--;
            diaCount++;
            if (diaCount >= m)
                return p;
        }

        i = lasti;
        j = lastj;
        while (i < n && j >= 0 && board[i, j] == p)
        {
            i++;
            j--;
            antiDiaCount++;
            if (antiDiaCount >= m)
                return p;
        }
        i = lasti - 1;
        j = lastj + 1;
        while (i >= 0 && j < n && board[i, j] == p)
        {
            i--;
            j++;
            antiDiaCount++;
            if (antiDiaCount >= m)
                return p;
        }
        if (state.GetRemainingMoves().Count <= 0)
            return 2;
        return 0;
    }


    //************heuristics******************//
    int HeuRow(BoardState state, int row, int col)
    {
        var board = state.board;
        int val = 0;
        for (int i = col; i < state.boardSize; i++)
        {
            if (board[row, i] == 0)
                val += board[row, col];
            else if (board[row, i] != board[row, col])
            {
                // val+=heuRowDef(row,i);
                break;
            }
            val += 5 * board[row, i];
        }
        for (int i = col - 1; i >= 0; i--)
        {
            if (board[row, i] == 0)
                val += board[row, col];
            else if (board[row, i] != board[row, col])
                break;
            val += 5 * board[row, i];
        }
        return val;
    }

    int HeuCol(BoardState state, int row, int col)
    {
        int val = 0;
        int p = state.lastMove.player;

        var board = state.board;

        for (int i = row; i < state.boardSize; i++)
        {
            if (board[i, col] == 0)
                val += board[row, col];
            else if (board[i, col] != board[row, col])
                break;
            val += 5 * board[i, col];
        }
        for (int i = row; i >= 0; i--)
        {
            if (board[i, col] == 0)
                val += board[row, col];
            if (board[i, col] != board[row, col])
                break;
            val += 5 * board[i, col];
        }
        return val;
    }

    int HeuDia(BoardState state, int row, int col)
    {
        int val = 0;
        var board = state.board;
        int p = state.lastMove.player;
        int n = state.boardSize;
        var lasti = state.lastMove.i;
        var lastj = state.lastMove.j;
        int i = lasti;
        int j = lastj;
        while (i < n && j < n && board[i, j] == p)
        {
            i++;
            j++;
            val++;
        }
        i = lasti - 1;
        j = lastj - 1;
        while (i >= 0 && j >= 0 && board[i, j] == p)
        {
            i--;
            j--;
            val++;
        }
        return val * 5;
    }

    int HeuAntiDia(BoardState state, int row, int col)
    {
        int val = 0;
        int p = state.lastMove.player;
        var board = state.board;
        var lasti = state.lastMove.i;
        var lastj = state.lastMove.j;
        int n = state.boardSize;

        int i = lasti;
        int j = lastj;
        while (i < n && j >= 0 && board[i, j] == p)
        {
            i++;
            j--;
            val++;
        }
        i = lasti - 1;
        j = lastj + 1;
        while (i >= 0 && j < n && board[i, j] == p)
        {
            i--;
            j++;
            val++;
        }
        return val * 5;
    }


}
