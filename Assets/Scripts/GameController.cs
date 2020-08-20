using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject gridLine;
    [SerializeField]
    private GameObject gridSpace;
    [SerializeField]
    private Transform girdParent;
    [SerializeField]
    private Transform spaceParent;

    public int boardSize;
    const float screenSize = 1080;

    private GridSpace[,] visualGrid;
    private sbyte[,] gameBoard;

    void Start()
    {
        visualGrid = new GridSpace[boardSize, boardSize];
        gameBoard = new sbyte[boardSize, boardSize];
        SetGrids();
    }

    private void SetGrids()
    {
        float gridSize = screenSize / boardSize;
        float position = -screenSize / 2 + gridSize;

        // Set Lines
        for (int i = 0; i < boardSize - 1; i++)
        {

            GameObject line = Instantiate(gridLine);
            line.transform.SetParent(girdParent);
            line.transform.localPosition = new Vector3(position, 0, 0);

            line = Instantiate(gridLine);
            line.transform.SetParent(girdParent);
            line.transform.localPosition = new Vector3(0, position, 0);
            line.transform.Rotate(new Vector3(0, 0, 90));

            position += gridSize;

        }


        // Set Space
        position = -screenSize / 2 + gridSize / 2;
        float y_position = -(screenSize / 2 - gridSize / 2);
        float x_position = position;
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                GameObject space = Instantiate(gridSpace);
                visualGrid[i, j] = space.GetComponent<GridSpace>();
                visualGrid[i, j].controller = this;
                visualGrid[i, j].i = i;
                visualGrid[i, j].j = j;
                space.transform.SetParent(spaceParent);
                space.transform.localPosition = new Vector3(x_position, y_position, 0);

                RectTransform rt = (RectTransform)space.transform;
                rt.sizeDelta = new Vector2(gridSize - 5, gridSize - 5);

                x_position += gridSize;
            }
            y_position += gridSize;
            x_position = position;
        }
    }

    public void MoveMade(int i, int j)
    {
        SetGridInteractable(false);
        MakeMove();

    }

    private void MakeMove()
    {
        SetGridInteractable(true);
    }

    private void SetGridInteractable(bool interactable)
    {
        foreach (var row in visualGrid)
        {
            row.SetInteractable(interactable);
        }
    }


}
