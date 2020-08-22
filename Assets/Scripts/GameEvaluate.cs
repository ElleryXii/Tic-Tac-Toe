using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvaluate
{
    //int n, m;
    //int[][] board;
    //int lasti, lastj;
    //int totalmove = 0;
    //bool player;
    //int remainingMove;

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
        return HeuRow(lastMove.i, lastMove.j) + HeuCol(lastMove.i, lastMove.j) + HeuDia(lastMove.i, lastMove.j) + HeuAntiDia(lastMove.i, lastMove.j);
    }

    //check if game terminates. Return 0 is game has not terminated, 2 if it terminates in a draw, 1 is x win, -1 if o win
    public int CheckWin(BoardState state)
    {
        int rowCount = 0;
        int colCount = 0;
        int diaCount = 0;
        int antiDiaCount = 0;
        var lasti = state.lastMove.i;
        var lastj = state.lastMove.j;
        var board = state.board;
        int m = state.winCondition;
        int n = state.boardSize;
        int p = board[lasti, lastj];
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
        int p;

        var board = state.board;


        if (player)
            p = 1;
        else
            p = -1;
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
        int p;
        int n = state.boardSize;
        var lasti = state.lastMove.i;
        var lastj = state.lastMove.j;
        if (player)
            p = 1;
        else
            p = -1;
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
        int p;
        var board = state.board;
        var lasti = state.lastMove.i;
        var lastj = state.lastMove.j;
        int n = state.boardSize;

        if (player)
            p = 1;
        else
            p = -1;
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

    // int heuRowDef(int row, int col){
    //   int count = 0;
    //   int val = 0;
    //   boolean blank = false;
    //   if (lastj>col){
    //     for (int i = col;i>=0;i--){
    //       if (board[row][i]==board[row][col])
    //         count++;
    //       else if (board[row][i]==0){
    //         blank = true;
    //         break;
    //       }
    //       else;
    //     }
    //   }
    //   else{
    //     for (int i = col;i<0;i++){
    //       if (board[row][i]==board[row][col])
    //         count++;
    //       else if (board[row][i]==0){
    //         blank = true;
    //         break;
    //       }
    //       else;
    //     }
    //   }
    //   if (blank){
    //     val = 8*count*board[row][col];
    //   }
    //   else{
    //     val = 2*count*board[row][col];
    //   }
    //   if (count==m-1){
    //     val = 1000*board[row][col];
    //   }
    //   return -1*val;
    // }

}
