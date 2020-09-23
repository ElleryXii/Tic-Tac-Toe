using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAI : MonoBehaviour
{

    private static GameAI instance = null;
    private GameAI() { }
    public static GameAI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameAI();
            }
            return instance;
        }
    }

    public IEnumerator GetBestMove(BoardState state, Action<(int i, int j)> OnEnd)
    {
        MctsPlanner.Instance.excluded = new List<(int i, int j)>();
        List<(int i, int j)> moves = state.GetRemainingMoves();
        int remainingMove = moves.Count;
        int depth = 5;
        bool player = state.lastMove.player == -1;

        //blank board, go for the center
        if (remainingMove == state.boardSize * state.boardSize)
        {
            OnEnd((state.boardSize / 2, state.boardSize / 2));
            yield break;
        }

        //use monte carlo tree search
        if (remainingMove > 9)
        {
            //max 3 chances of re-run mcts if no move left after minimax test
            int i = 3;
            while (i > 0)
            {
                //get a list of move candidate use mcts
                //List<(int i, int j)> MctsMoves = MctsPlanner.Instance.GetMoves(state);

                List<(int i, int j)> MctsMoves = null;
                yield return MctsPlanner.Instance.GetMoves(state, (m) => SetMctsMovesList(m, out MctsMoves));

                //for (int k = MctsMoves.Count - 1; k >= 0; k--)
                //{
                //    var move = MctsMoves[i];
                //    int u = MiniMax(state.GetNewState((move.i, move.j, (sbyte)(0 - state.lastMove.player))), depth, int.MinValue, int.MaxValue, !player);
                //    if ((player && u == int.MinValue) || (!player && u == int.MaxValue))
                //    {
                //        MctsPlanner.Instance.excluded.Add(move);
                //        MctsMoves.Remove(move);
                //    }
                //}
                //if there's move remaing, return the first one
                Debug.Log("what? ");
                if (MctsMoves.Count != 0)
                {
                    OnEnd(MctsMoves[0]);
                    yield break;
                }
                i--;
                yield return null;
            }
        }
        //switch to minimax at endgame
        if (remainingMove <= 9)
            depth = 10;

        int bestu = player ? int.MinValue : int.MaxValue;

        var bestMove = moves[0];
        foreach (var move in moves)
        {
            int u = MiniMax(state.GetNewState((move.i, move.j, (sbyte)(0 - state.lastMove.player))), depth, int.MinValue, int.MaxValue, !player);
            if ((player && u >= bestu) || (!player && u <= bestu))
            {
                bestu = u;
                bestMove = move;
            }
            yield return null;
        }

        OnEnd(bestMove);
        yield break;
    }

    private void SetMctsMovesList(List<(int i, int j)> list, out List<(int i, int j)> listRef)
    {
        listRef = list;
    }


    //public static (int i, int j) GetBestMove(BoardState state)
    //{
    //    //If there are fewer than 12 remaining moves, use minimax
    //    if (state.GetRemainingMoves().Count <= 12)
    //    {
    //        return MinMaxDriver(state);
    //    }
    //    else
    //    {
    //        var move = MctsDriver(state.DeepCopy());
    //        if (move == (-1, -1))
    //            return GetRandomMove(state);
    //        else
    //            return move;
    //    }

    //}


    //private static (int i, int j) MctsDriver(BoardState state)
    //{
    //    //max 3 chances of re-run mcts if no move left after minimax test
    //    MctsPlanner.Instance.excluded = new List<(int i, int j)>();

    //    int i = 3;
    //    int depth = 5;
    //    bool player = state.lastMove.player == -1;

    //    while (i > 0)
    //    {
    //        //get a list of move candidate use mcts
    //        List<(int i, int j)> moves = MctsPlanner.Instance.GetMoves(state);

    //        foreach (var move in moves)
    //        {
    //            //test the moves with minimax, make sure they won't fail on the next several steps
    //            int u = MiniMax(state.GetNewState((move.i, move.j, (sbyte)(0 - state.lastMove.player))), depth, int.MinValue, int.MaxValue, !player);
    //            if ((player && u == int.MinValue) || (!player && u == int.MaxValue))
    //            {
    //                MctsPlanner.Instance.excluded.Add(move);
    //                moves.Remove(move);
    //            }
    //        }
    //        //if there's move remaing, return the first one
    //        if (moves.Count != 0)
    //            return moves[0];
    //        Debug.Log("isEmpty");
    //        i--;
    //    }
    //    Debug.Log("isEmpty after 5");
    //    //TODO: No good moves found, use minimax;
    //    return (-1, -1);
    //}



    ////TODO: Should be able to combine this with Minimax
    //private static (int i, int j) MinMaxDriver(BoardState state)
    //{
    //    List<(int i, int j)> moves = state.GetRemainingMoves();
    //    if (moves.Count == 0)
    //    {
    //        return (-1, -1);
    //    }

    //    int depth = 15;
    //    bool player = state.lastMove.player == -1;
    //    int bestu = player ? int.MinValue : int.MaxValue;

    //    var bestMove = moves[0];

    //    //(int i, int j) bestMove = moves[0];

    //    foreach (var move in moves)
    //    {
    //        int u = MiniMax(state.GetNewState((move.i, move.j, (sbyte)(0 - state.lastMove.player))), depth, int.MinValue, int.MaxValue, !player);
    //        if ((player && u >= bestu) || (!player && u <= bestu))
    //        {
    //            bestu = u;
    //            bestMove = move;
    //        }
    //    }
    //    return bestMove;
    //}



    private int MiniMax(BoardState state, int depth, int alpha, int beta, bool maximizing)
    {
        if (GameEvaluate.Instance.CheckWin(state) != 0)
            return GameEvaluate.Instance.GetEval(state);
        if (depth <= 0)
            return GameEvaluate.Instance.GetEval(state);

        List<(int i, int j)> branch = state.GetRemainingMoves();

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



    public static (int i, int j) GetRandomMove(BoardState state)
    {
        var random = new System.Random();
        var rndInt = random.Next(state.GetRemainingMoves().Count);
        //Debug.Log("remaingmoves count: " + state.GetRemainingMoves().Count + " rndInt: " + rndInt);
        return state.GetRemainingMoves()[rndInt];
    }


}
