using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState
{
    public sbyte[,] board;
    public int boardSize;
    public int winCondition;
    private List<(int i, int j)> remainingMoves;
    public bool gameEnd = false;
    public (int i, int j) lastMove;

    public BoardState(int boardSize)
    {
        this.boardSize = boardSize;
        board = new sbyte[boardSize, boardSize];

        remainingMoves = new List<(int i, int j)>();
        lastMove = (-1, -1);

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                remainingMoves.Add((i, j));
            }
        }
    }

    public List<(int i, int j)> GetRemainingMoves()
    {
        return remainingMoves;
    }

    public void MakeMove((int i, int j, sbyte player) move)
    {
        lastMove = (move.i, move.j);
        board[move.i, move.j] = move.player;
        remainingMoves.Remove((move.i, move.j));
        if (remainingMoves.Count == 0)
        {
            gameEnd = true;
        }
    }


    public BoardState DeepCopy()
    {
        var copy = new BoardState(boardSize);
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                copy.board[i, j] = this.board[i, j];
            }
        }

        copy.remainingMoves = new List<(int i, int j)>();
        foreach (var move in this.remainingMoves)
        {
            copy.remainingMoves.Add(move);
        }

        return copy;
    }

    public BoardState GetNewState((int i, int j, sbyte player) move)
    {
        var copy = this.DeepCopy();
        copy.MakeMove(move);
        return copy;

    }

}
