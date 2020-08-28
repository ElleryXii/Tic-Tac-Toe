using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward
{
    public (int playerOne, int playerTwo) rewards;

    public Reward(int reward0, int reward1)
    {
        rewards = (reward0, reward1);
    }

    public int GetReward(bool player)
    {
        if (player)
            return rewards.playerOne;
        return rewards.playerTwo;
    }

    public void AddReward(Reward reward)
    {
        rewards.playerOne += reward.GetReward(true);
        rewards.playerTwo += reward.GetReward(false);
    }

    public override string ToString()
    {
        return string.Format("({0}, {1})", rewards.playerOne, rewards.playerTwo);
    }
}