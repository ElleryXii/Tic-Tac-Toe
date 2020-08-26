//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MctsPlanner : MonoBehaviour
//{
//    int maxIterations;
//    List<(int i, int j)> excluded = new List<(int i, int j)>();

//    public MctsPlanner(int i)
//    {
//        this.maxIterations = i;
//    }

//    //get the best move
//    (int i, int j) GetMove(BoardState game)
//    {
//        MctsNode rootNode = new MctsNode(null, null, game, excluded);
//        for (int i = 0; i < maxIterations; i++)
//        {
//            BoardState gameCopy = game.DeepCopy();
//            MctsNode node = select(rootNode, gameCopy);
//            node = node.Expand(gameCopy);
//            Reward reward = rollout(gameCopy);
//            node.BackPropagate(reward);
//        }
//        MctsNode mostVisitedChild = rootNode.GetMostVisited();
//        return mostVisitedChild.bridge;
//    }

//    //get a list of best moves
//    LinkedList<(int i, int j)> GetMoves(BoardState game)
//    {
//        MctsNode rootNode = new MctsNode(null, null, game, excluded);
//        for (int i = 0; i < maxIterations; i++)
//        {
//            TTTState gameCopy = game.copy();
//            MctsNode node = select(rootNode, gameCopy);
//            node = node.Expand(gameCopy);
//            Reward reward = rollout(gameCopy);
//            node.BackPropagate(reward);
//        }
//        LinkedList<MctsNode> mostVisited = rootNode.GetMostVisitedList();
//        LinkedList<Move> moves = new LinkedList<Move>();
//        for (int i = 0; i < mostVisited.size(); i++)
//        {
//            moves.add(mostVisited.get(i).bridge);
//        }
//        return moves;
//    }

//    MctsNode select(MctsNode node, TTTState game)
//    {
//        while (!(node.unexplored.size() > 0) && game.checkWin() == 0)
//        {
//            node = node.Select();
//            Move move = node.bridge;
//            if (move != null)
//                game.makeMove(move);
//        }
//        return node;
//    }

//    Reward rollout(TTTState game)
//    {
//        game.randomPlay();
//        return game.getReward();
//    }
//}
