using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Generic.HashSet<(int i, int j)>;

public static class GameAI
{


    public static (int i, int j) GetBestMove(BoardState state)
    {

        HashSet<(int i, int j)> moves = state.GetRemainingMoves();
        if (moves.Count <= 12)
        {
            int depth = 15;
            bool player = state.lastMove.player == -1;
            int bestu = player ? int.MinValue : int.MaxValue;

            //TODO: Rethink about how to use & dispose iterator 
            Enumerator iter = moves.GetEnumerator();
            var bestMove = iter.Current;
            iter.Dispose();

            //(int i, int j) bestMove = moves[0];

            foreach (var move in moves)
            {
                int u = MiniMax(state.GetNewState((move.i, move.j, (sbyte)(0 - state.lastMove.player))), depth, int.MinValue, int.MaxValue, !player);
                if ((player && u >= bestu) || (!player && u <= bestu))
                {
                    bestu = u;
                    bestMove = move;
                }
            }
            return bestMove;
        }
        else
        {
            return GetRandomMove(state);
        }

    }



    private static int MiniMax(BoardState state, int depth, int alpha, int beta, bool maximizing)
    {
        if (GameEvaluate.Instance.CheckWin(state) != 0)
            return GameEvaluate.Instance.GetEval(state);
        if (depth <= 0)
            return GameEvaluate.Instance.GetEval(state);

        HashSet<(int i, int j)> branch = state.GetRemainingMoves();

        //get max value
        if (maximizing)
        {
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



    private static (int i, int j) GetRandomMove(BoardState state)
    {
        //TODO: Rethink about how to use & dispose iterator 
        Enumerator iter = state.GetRemainingMoves().GetEnumerator();
        var random = new System.Random();
        for (int i = 0; i < random.Next(state.GetRemainingMoves().Count); i++)
            iter.MoveNext();
        var move = iter.Current;
        iter.Dispose();

        return move;

    }


}
