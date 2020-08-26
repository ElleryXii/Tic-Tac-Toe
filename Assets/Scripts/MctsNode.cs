using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MctsNode
{
    List<(int i, int j)> excludedMoves;
    MctsNode parent;
    int numSim = 0;
    Reward reward;
    public LinkedList<MctsNode> children = new LinkedList<MctsNode>();
    public List<(int i, int j)> unexplored;
    public (int i, int j) bridge;   //the move from the parent state to this state
    bool player;

    public MctsNode(MctsNode parent, (int i, int j) move, BoardState state, List<(int i, int j)> excludedMoves)
    {
        this.excludedMoves = excludedMoves;
        this.parent = parent;
        bridge = move;
        this.player = state.lastMove.player == 1;
        unexplored = state.GetRemainingMoves();
        for (int i = 0; i < excludedMoves.Count; i++)
        {
            if (unexplored[i] == excludedMoves[i])
                unexplored.RemoveAt(i);
        }
        reward = new Reward(0, 0);
    }

    //select the node max uctvalue
    public MctsNode Select()
    {
        MctsNode selectedNode = this;
        double max = int.MinValue;
        foreach (MctsNode child in children)
        {
            double uctValue = GetUctValue(child);
            if (uctValue > max)
            {
                max = uctValue;
                selectedNode = child;
            }
        }
        return selectedNode;
    }

    //get the uctvalue
    double GetUctValue(MctsNode child)
    {
        double uctValue;
        if (child.numSim == 0)
            uctValue = 1;
        else
            uctValue = (1.0 * child.GetReward(player)) / (child.numSim * 1.0) + (Math.Sqrt(2.0 * (Math.Log(numSim * 1.0) / child.numSim)));
        Random r = new Random();
        uctValue += (r.NextDouble() / 10000000);
        return uctValue;
    }

    //expend an unexplored node
    public MctsNode Expand(BoardState state)
    {
        if (!(unexplored.Count > 0))
            return this;
        Random random = new Random();
        int moveIndex = random.Next(unexplored.Count);
        (int i, int j) move = unexplored[moveIndex];
        unexplored.RemoveAt(moveIndex);
        state.MakeMove((move.i, move.j, (sbyte)(0 - state.lastMove.player)));
        MctsNode child = new MctsNode(this, move, state, excludedMoves);
        children.AddLast(child);
        return child;
    }

    //update the reward of this and parent
    public void BackPropagate(Reward reward)
    {
        this.reward.AddReward(reward);
        this.numSim++;
        if (parent != null)
            parent.BackPropagate(reward);
    }

    //get the most mostVisited node among the children
    public MctsNode GetMostVisited()
    {
        int max = 0;
        MctsNode mostVisited = null;
        foreach (MctsNode child in children)
        {
            if (child.numSim > max)
            {
                mostVisited = child;
                max = child.numSim;
            }
        }
        return mostVisited;
    }


    //get a list of equally most mostVisited node among the children
    public List<MctsNode> GetMostVisitedList()
    {
        List<MctsNode> mostVisited = new List<MctsNode>();
        int max = 0;
        foreach (MctsNode child in children)
        {
            if (child.numSim == max)
            {
                mostVisited.Add(child);
            }
            else if (child.numSim > max)
            {
                mostVisited.Clear();
                mostVisited.Add(child);
                max = child.numSim;
            }
        }
        return mostVisited;
    }

    public double GetReward(bool player)
    {
        return reward.GetReward(player);
    }

    public override string ToString()
    {
        return string.Format("numsim: {0}, reward: {1}", numSim, reward.ToString());
    }
}
