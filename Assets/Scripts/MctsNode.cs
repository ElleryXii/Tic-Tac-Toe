//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Random = System.Random;

//public class MctsNode : MonoBehaviour
//{
//    List<(int i, int j)> excludedMoves;
//    MctsNode parent;
//    int numSim = 0;
//    Reward reward;
//    LinkedList<MctsNode> children = new LinkedList<MctsNode>();
//    LinkedList<(int i, int j)> unexplored;
//    (int i, int j) bridge;   //the move from the parent state to this state
//    bool player;

//    public MctsNode(MctsNode parent, (int i, int j) move, BoardState state, List<(int i, int j)> excludedMoves)
//    {
//        this.excludedMoves = excludedMoves;
//        this.parent = parent;
//        bridge = move;
//        this.player = state.player;
//        unexplored = state.GetMoves(player);
//        for (int i = 0; i < excludedMoves.Count; i++)
//        {
//            if (unexplored.get(i).isequal(excludedMoves[i]))
//                unexplored.Remove(i);
//        }
//        reward = new Reward(0, 0);
//    }

//    //select the node max uctvalue
//    MctsNode select()
//    {
//        MctsNode selectedNode = this;
//        double max = int.MinValue;
//        foreach (MctsNode child in children)
//        {
//            double uctValue = getUctValue(child);
//            if (uctValue > max)
//            {
//                max = uctValue;
//                selectedNode = child;
//            }
//        }
//        return selectedNode;
//    }

//    //get the uctvalue
//    double getUctValue(MctsNode child)
//    {
//        double uctValue;
//        if (child.numSim == 0)
//            uctValue = 1;
//        else
//            uctValue = (1.0 * child.getReward(player)) / (child.numSim * 1.0) + (Math.Sqrt(2.0 * (Math.Log(numSim * 1.0) / child.numSim)));
//        Random r = new Random();
//        uctValue += (r.NextDouble() / 10000000);
//        return uctValue;
//    }

//    //expend an unexplored node
//    MctsNode expand(BoardState state)
//    {
//        if (!(unexplored.Count > 0))
//            return this;
//        Random random = new Random();
//        int moveIndex = random.Next(unexplored.Count);
//        (int i, int j) move = unexplored.Remove(moveIndex);
//        state.MakeMove(move);
//        MctsNode child = new MctsNode(this, move, state, excludedMoves);
//        children.AddLast(child);
//        return child;
//    }

//    //update the reward of this and parent
//    void backPropagate(Reward reward)
//    {
//        this.reward.addReward(reward);
//        this.numSim++;
//        if (parent != null)
//            parent.backPropagate(reward);
//    }

//    //get the most mostVisited node among the children
//    MctsNode getMostVisited()
//    {
//        int max = 0;
//        MctsNode mostVisited = null;
//        foreach (MctsNode child in children)
//        {
//            if (child.numSim > max)
//            {
//                mostVisited = child;
//                max = child.numSim;
//            }
//        }
//        return mostVisited;
//    }


//    //get a list of equally most mostVisited node among the children
//    LinkedList<MctsNode> getMostVisitedList()
//    {
//        LinkedList<MctsNode> mostVisited = new LinkedList<MctsNode>();
//        int max = 0;
//        foreach (MctsNode child in children)
//        {
//            if (child.numSim == max)
//            {
//                mostVisited.AddLast(child);
//            }
//            else if (child.numSim > max)
//            {
//                mostVisited.Clear();
//                mostVisited.AddLast(child);
//                max = child.numSim;
//            }
//        }
//        return mostVisited;
//    }

//    double getReward(bool player)
//    {
//        return reward.GetReward(player);
//    }

//    public override string ToString()
//    {

//        return string.Format("numsim: {0}, reward: {1}", numSim, reward.ToString());
//    }



//    public class Reward
//    {
//        (int playerOne, int playerTwo) rewards;
//        public Reward(int reward0, int reward1)
//        {
//            rewards = (reward0, reward1);
//        }

//        (int playerOne, int playerTwo) GetReward()
//        {
//            return rewards;
//        }

//        public int GetReward(bool player)
//        {
//            if (player)
//                return rewards.playerOne;
//            return rewards.playerTwo;
//        }

//        public void addReward(Reward reward)
//        {
//            rewards.playerOne += reward.GetReward(true);
//            rewards.playerTwo += reward.GetReward(false);
//        }

//        public override string ToString()
//        {
//            return string.Format("({0}, {1})", rewards.playerOne, rewards.playerTwo);
//        }
//    }
//}
