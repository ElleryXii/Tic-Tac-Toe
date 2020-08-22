﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameAI
{


    public static (int i, int j) GetBestMove(BoardState state)
    {
        return GetRandomMove(state);
    }



    private static (int i, int j) GetRandomMove(BoardState state)
    {
        var random = new System.Random();
        var possibleMoves = state.GetRemainingMoves();
        int index = random.Next(possibleMoves.Count);
        return possibleMoves[index];

    }

    private static int MiniMax(BoardState state, int depth, int alpha, int beta, bool maximizing)
    {
        if (GameEvaluate.Instance.CheckWin(state) != 0)
            return GameEvaluate.Instance.GetEval(state);
        if (depth <= 0)
            return GameEvaluate.Instance.GetEval(state);

        //get max value
        if (maximizing)
        {
            List<(int i, int j)> branch = state.GetRemainingMoves();
            int maxVal = int.MinValue;
            foreach (var move in branch)
            {
                int eval = MiniMax(state.GetNewState((move.i, move.j, 1)), depth - 1, alpha, beta, false);
                maxVal = Math.Max(maxVal, eval);
                alpha = Math.Max(alpha, eval);
                if (beta <= alpha)
                    break;
            }
            return maxVal;
        }
        //get min value
        else
        {
            List<(int i, int j)> branch = state.GetRemainingMoves();
            int minVal = int.MaxValue;
            foreach (var move in branch)
            {
                int eval = MiniMax(state.GetNewState((move.i, move.j, -1)), depth - 1, alpha, beta, true);
                minVal = Math.Min(minVal, eval);
                beta = Math.Min(beta, eval);
                if (beta <= alpha)
                    break;
            }
            return minVal;
        }
    }
}