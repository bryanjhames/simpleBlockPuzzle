using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public BlockDataSO[] availableBlocks;
    public GameObject blockPrefab;
    public Transform[] spawnPositions;

    public List<DraggableBlock> activeBlocks = new List<DraggableBlock>();

    public GameObject GameOverPanel;
    public GameOverManager gameOverManager;

    void Start()
    {
        GenerateNewBlocks();
    }

    public void GenerateNewBlocks()
    {
        activeBlocks.Clear();

        for (int i = 0; i < 3; i++)
        {
            BlockDataSO data = availableBlocks[Random.Range(0, availableBlocks.Length)];
            GameObject block = Instantiate(blockPrefab, spawnPositions[i].position, Quaternion.identity, transform);

            // Set smaller scale while inside container
            block.transform.localScale = Vector3.one * 0.5f;

            DraggableBlock draggable = block.GetComponent<DraggableBlock>();
            draggable.Initialize(data);
            draggable.spawner = this;
            activeBlocks.Add(draggable);

            ScoreSystem.Instance.UpdateUI();
        }
    }

    public void BlockPlaced(DraggableBlock block)
    {
        activeBlocks.Remove(block);

        if (activeBlocks.Count > 0)
        {
            if (!HasAnyValidPlacement())
            {
                Debug.Log("GAME OVER!");
                AudioManager.Instance.PlayGameOver();
                BoardPlacement.Instance.FillRemainingCellsAnimated(() =>
                {
                    ScoreSystem.Instance.CheckHighScore();
                   
                    gameOverManager.ShowGameOverScreen(
                        ScoreSystem.Instance.GetCurrentScore(),
                        ScoreSystem.Instance.GetHighScore()
                    );
                });

                return;
            }
        }
        else
        {
            if (!BoardHasAnyValidPlacementForNextBlocks())
            {
                AudioManager.Instance.PlayGameOver();
                BoardPlacement.Instance.FillRemainingCellsAnimated(() =>
                {
                    ScoreSystem.Instance.CheckHighScore();
                   
                    gameOverManager.ShowGameOverScreen(
                        ScoreSystem.Instance.GetCurrentScore(),
                        ScoreSystem.Instance.GetHighScore()
                    );
                });
                return;
            }

            GenerateNewBlocks();
        }
    }

    public bool HasAnyValidPlacement()
    {
        foreach (var draggable in activeBlocks)
        {
            if (BoardPlacement.Instance.CanPlaceBlock(draggable.GetBlockData()))
                return true;
        }
        return false;
    }

    public bool BoardHasAnyValidPlacementForNextBlocks()
    {
        foreach (var blockData in availableBlocks)
        {
            if (BoardPlacement.Instance.CanPlaceBlock(blockData))
                return true;
        }
        return false;
    }

    public void CheckForGameOver()
    {
        foreach (var draggable in activeBlocks)
        {
            if (BoardPlacement.Instance.CanPlaceBlock(draggable.GetBlockData()))
                return;
        }
        Debug.Log("GAME OVER!");
    }
}
