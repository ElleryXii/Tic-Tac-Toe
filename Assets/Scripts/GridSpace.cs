using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public Button button;
    public Text buttonText;
    public GameController controller;
    public (int i, int j) position;

    public void SetSpace(string playerSide = "X")
    {
        buttonText.text = playerSide;
        button.interactable = false;
        controller.MoveMade(position);
    }

    public void SetAISpace()
    {
        buttonText.text = "O";
    }


    public void SetInteractable(bool interactable)
    {
        button.interactable = buttonText.text != "" ? false : interactable;
    }


}
