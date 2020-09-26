using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MctsPlanner
{
    private static MctsPlanner instance = null;
    public static MctsPlanner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MctsPlanner(100000);
            }
            return instance;
        }
    }


    int maxIterations;

    public List<(int i, int j)> excluded = new List<(int i, int j)>();

    private MctsPlanner(int i)
    {
        this.maxIterations = i;
    }

    //get the best move
    public (int i, int j) GetMove(BoardState game)
    {
        MctsNode rootNode = new MctsNode(null, (-1, -1), game, excluded);
        for (int i = 0; i < maxIterations; i++)
        {
            BoardState gameCopy = game.DeepCopy();
            MctsNode node = Select(rootNode, gameCopy);
            node = node.Expand(gameCopy);
            Reward reward = RollOut(gameCopy);
            node.BackPropagate(reward);
        }
        MctsNode mostVisitedChild = rootNode.GetMostVisited();
        return mostVisitedChild.bridge;
    }

    ////get a list of best moves
    //public List<(int i, int j)> GetMoves(BoardState game)
    //{
    //    MctsNode rootNode = new MctsNode(null, (-1, -1), game, excluded);
    //    for (int i = 0; i < maxIterations; i++)
    //    {
    //        BoardState gameCopy = game.DeepCopy();
    //        MctsNode node = Select(rootNode, gameCopy);
    //        node = node.Expand(gameCopy);
    //        Reward reward = RollOut(gameCopy);
    //        node.BackPropagate(reward);
    //    }
    //    List<MctsNode> mostVisited = rootNode.GetMostVisitedList();
    //    List<(int i, int j)> moves = new List<(int i, int j)>();
    //    for (int i = 0; i < mostVisited.Count; i++)
    //    {
    //        moves.Add(mostVisited[i].bridge);
    //    }
    //    return moves;
    //}

    //get a list of best moves
    public IEnumerator GetMoves(BoardState game, Action<List<(int i, int j)>> callback)
    {
        MctsNode rootNode = new MctsNode(null, (-1, -1), game, excluded);
        for (int i = 0; i < maxIterations; i++)
        {
            BoardState gameCopy = game.DeepCopy();
            MctsNode node = Select(rootNode, gameCopy);
            node = node.Expand(gameCopy);
            Reward reward = RollOut(gameCopy);
            node.BackPropagate(reward);
            if (i % 1000 == 0)
            {
                yield return null;
            }
        }
        List<MctsNode> mostVisited = rootNode.GetMostVisitedList();
        List<(int i, int j)> moves = new List<(int i, int j)>();
        for (int i = 0; i < mostVisited.Count; i++)
        {
            moves.Add(mostVisited[i].bridge);
        }
        callback(moves);
    }

    public MctsNode Select(MctsNode node, BoardState game)
    {
        while (!(node.unexplored.Count > 0) && GameEvaluate.Instance.CheckWin(game) == 0)
        {
            node = node.Select();
            (int i, int j) move = node.bridge;
            if (move != (-1, -1))
                game.MakeMove((move.i, move.j, (sbyte)(0 - game.lastMove.player)));
        }
        return node;
    }

    public Reward RollOut(BoardState game)
    {
        while (GameEvaluate.Instance.CheckWin(game) == 0)
        {
            var move = GameAI.GetRandomMove(game);
            game.MakeMove((move.i, move.j, (sbyte)(0 - game.lastMove.player)));
        }

        if (GameEvaluate.Instance.CheckWin(game) == 1)
            return new Reward(1, -1);
        else if (GameEvaluate.Instance.CheckWin(game) == -1)
            return new Reward(-1, 1);
        return new Reward(0, 0);
    }
}
