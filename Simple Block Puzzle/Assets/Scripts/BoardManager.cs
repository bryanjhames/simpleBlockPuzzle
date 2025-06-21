using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform boardParent;

    private const int gridSize = 9;
    private float cellSize = 1.0f;

    void Start()
    {
        Application.targetFrameRate = 120;
        CreateGrid();
    }

   void CreateGrid()
    {
        float cellSpacing = 1.0f; // add slight gap

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector2 pos = new Vector2(x * cellSpacing, y * cellSpacing);
                GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity, boardParent);
                cell.transform.localPosition = pos;
            }
        }

        boardParent.localPosition = new Vector3(-gridSize * cellSpacing / 2f + 0.55f, -gridSize * cellSpacing / 2f + 0.55f, 0);
    }

}
