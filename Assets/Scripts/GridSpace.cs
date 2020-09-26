using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public Button button;
    public Text buttonText;
    public (int i, int j) index;
    public GameController controller = null;

    public void SetSpace(int player = 1)
    {
        buttonText.text = player == 1 ? "X" : "O";
        controller.MoveMade(index, (sbyte)player);
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = buttonText.text != "" ? false : interactable;
    }


}
