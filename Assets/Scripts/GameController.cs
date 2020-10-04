using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject gridLine = null;
    [SerializeField]
    private GameObject gridSpace = null;
    [SerializeField]
    private Transform girdParent = null;
    [SerializeField]
    private Transform spaceParent = null;
    [SerializeField]
    private GameAI gameAI = null;

    public bool auto = false;
    public int winCondition;
    public int boardSize;
    readonly float screenSize = Screen.width;

    private GridSpace[,] visualGrid;
    private BoardState board;

    void Start()
    {
        gridLine.GetComponent<RectTransform>().sizeDelta = new Vector2(5, screenSize);
        visualGrid = new GridSpace[boardSize, boardSize];
        board = new BoardState(boardSize, winCondition);
        SetGrids();
        SetGridInteractable(!auto);
        if (auto)
        {
            MakeMove(1);
        }
    }

    private void SetGrids()
    {
        float gridSize = screenSize / boardSize;
        float position = -screenSize / 2 + gridSize;

        // Set Lines
        for (int i = 0; i < boardSize - 1; i++)
        {
            // Horizontal 
            GameObject line = Instantiate(gridLine);
            line.transform.SetParent(girdParent);
            line.transform.localPosition = new Vector3(position, 0, 0);

            // Vertical
            line = Instantiate(gridLine);
            line.transform.SetParent(girdParent);
            line.transform.localPosition = new Vector3(0, position, 0);
            line.transform.Rotate(new Vector3(0, 0, 90));

            position += gridSize;
        }

        // Set Space
        position = -screenSize / 2 + gridSize / 2;
        float y_position = screenSize / 2 - gridSize / 2;
        float x_position = position;
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                GameObject space = Instantiate(gridSpace);
                visualGrid[i, j] = space.GetComponent<GridSpace>();
                visualGrid[i, j].controller = this;
                visualGrid[i, j].index = (i, j);
                space.transform.SetParent(spaceParent);
                space.transform.localPosition = new Vector3(x_position, y_position, 0);

                RectTransform rt = (RectTransform)space.transform;
                rt.sizeDelta = new Vector2(gridSize - 5, gridSize - 5);

                x_position += gridSize;
            }
            y_position -= gridSize;
            x_position = position;
        }
    }


    public void MoveMade((int i, int j) move, sbyte player)
    {
        SetGridInteractable(player == -1);
        board.MakeMove((move.i, move.j, player));
        if (GameEvaluate.Instance.CheckWin(board) == 0)
        {
            if (player == 1)
            {
                MakeMove(-1);
            }
            else if (auto)
            {
                MakeMove(1);
            }
        }
        else
        {
            SetGridInteractable(false);
        }

    }

    private void MakeMove(sbyte player)
    {
        StartCoroutine(gameAI.GetBestMove(board.DeepCopy(), (move) => FoundMove(move, player)));
    }

    private void FoundMove((int i, int j) AImove, sbyte player)
    {
        visualGrid[AImove.i, AImove.j].SetSpace(player);
    }

    private void SetGridInteractable(bool interactable)
    {
        foreach (var space in visualGrid)
        {
            space.SetInteractable(interactable);
        }
    }
}
