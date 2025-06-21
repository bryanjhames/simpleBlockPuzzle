using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardPlacement : MonoBehaviour
{
    public static BoardPlacement Instance;

    private const int gridSize = 9;
    private int[,] grid = new int[gridSize, gridSize];

    public BlockDataSO defaultFillData;
    private Dictionary<Vector2Int, GameObject> visualBlocks = new Dictionary<Vector2Int, GameObject>();

    [Header("Combo Popup")]
    public GameObject comboPopupPrefab;
    public Transform canvasTransform;

    private void Awake()
    {
        Instance = this;
    }

    public bool TryPlaceBlock(BlockDataSO blockData, Vector3 worldPos)
    {
        Vector2 boardOrigin = new Vector2(-gridSize / 2f + 0.5f, -gridSize / 2f + 0.5f);
        Vector2 relativePos = (Vector2)worldPos - boardOrigin;
        Vector2Int boardPos = Vector2Int.RoundToInt(relativePos);

        List<Vector2Int> targetCells = new List<Vector2Int>();

        foreach (var cell in blockData.cells)
        {
            Vector2Int pos = boardPos + cell;
            if (!IsValid(pos) || grid[pos.x, pos.y] == 1)
                return false;

            targetCells.Add(pos);
        }

        foreach (var pos in targetCells)
        {
            grid[pos.x, pos.y] = 1;
            SpawnBlockVisual(pos, blockData);
        }

        ScoreSystem.Instance.AddPoints(10);
        CheckForLines();
        return true;
    }

    void SpawnBlockVisual(Vector2Int pos, BlockDataSO data)
    {
        GameObject square = new GameObject("PlacedCell");
        square.transform.position = new Vector3(
            pos.x - (gridSize / 2f - 0.5f),
            pos.y - (gridSize / 2f - 0.5f),
            0
        );
        square.transform.localScale = Vector3.one * 0.9f;

        var sr = square.AddComponent<SpriteRenderer>();
        sr.sprite = data.blockSprite;
        sr.color = Color.white;
        sr.sortingLayerName = "Blocks";
        sr.sortingOrder = 0;

        visualBlocks[pos] = square;
    }

    void CheckForLines()
    {
        List<int> fullRows = new List<int>();
        List<int> fullCols = new List<int>();

        for (int y = 0; y < gridSize; y++)
        {
            bool fullRow = true;
            for (int x = 0; x < gridSize; x++)
            {
                if (grid[x, y] != 1)
                {
                    fullRow = false;
                    break;
                }
            }
            if (fullRow) fullRows.Add(y);
        }

        for (int x = 0; x < gridSize; x++)
        {
            bool fullCol = true;
            for (int y = 0; y < gridSize; y++)
            {
                if (grid[x, y] != 1)
                {
                    fullCol = false;
                    break;
                }
            }
            if (fullCol) fullCols.Add(x);
        }

        foreach (var y in fullRows)
        {
            for (int x = 0; x < gridSize; x++)
            {
                grid[x, y] = 0;
                Vector2Int pos = new Vector2Int(x, y);
                if (visualBlocks.ContainsKey(pos))
                {
                    StartCoroutine(ShrinkAndDestroy(visualBlocks[pos]));
                    visualBlocks.Remove(pos);
                }
            }
        }

        foreach (var x in fullCols)
        {
            for (int y = 0; y < gridSize; y++)
            {
                grid[x, y] = 0;
                Vector2Int pos = new Vector2Int(x, y);
                if (visualBlocks.ContainsKey(pos))
                {
                    StartCoroutine(ShrinkAndDestroy(visualBlocks[pos]));
                    visualBlocks.Remove(pos);
                }
            }
        }

        if (fullRows.Count > 0 || fullCols.Count > 0)
        {
            int linesCleared = fullRows.Count + fullCols.Count;
            ScoreSystem.Instance.ApplyMultiplier(linesCleared);
            ShowComboPopup(linesCleared);
            AudioManager.Instance.PlayRowCleared();
        }
    }

    IEnumerator ShrinkAndDestroy(GameObject block)
    {
        float duration = 0.3f;
        Vector3 originalScale = block.transform.localScale;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 0f, time / duration);
            block.transform.localScale = originalScale * scale;
            block.transform.Rotate(0, 0, 200 * Time.deltaTime);
            yield return null;
        }

        Destroy(block);
    }

    void ShowComboPopup(int linesCleared)
    {
        if (comboPopupPrefab != null)
        {
            GameObject popup = Instantiate(comboPopupPrefab, canvasTransform);
            popup.transform.localPosition = Vector3.zero;

            int bonusScore = linesCleared * linesCleared * 10;
            popup.GetComponent<TMP_Text>().text = "+" + bonusScore;

            Destroy(popup, 1.5f);
        }
    }


    bool IsValid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridSize && pos.y >= 0 && pos.y < gridSize;
    }

    public bool CanPlaceBlock(BlockDataSO blockData)
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector2Int boardPos = new Vector2Int(x, y);
                if (CanPlaceAt(blockData, boardPos))
                    return true;
            }
        }
        return false;
    }

    public bool CanPlaceAt(BlockDataSO blockData, Vector2Int boardPos)
    {
        foreach (var cell in blockData.cells)
        {
            Vector2Int pos = boardPos + cell;
            if (!IsValid(pos) || grid[pos.x, pos.y] == 1)
                return false;
        }
        return true;
    }

    public void FillRemainingCellsAnimated(System.Action onComplete = null)
    {
        StartCoroutine(FillRemainingCellsRoutine(onComplete));
    }

    private IEnumerator FillRemainingCellsRoutine(System.Action onComplete)
    {
        List<Vector2Int> emptyCells = new List<Vector2Int>();

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (!visualBlocks.ContainsKey(pos))
                {
                    emptyCells.Add(pos);
                }
            }
        }

        Shuffle(emptyCells);

        foreach (var cell in emptyCells)
        {
            SpawnBlockVisual(cell, defaultFillData);
            yield return new WaitForSeconds(0.05f);
        }

        if (onComplete != null)
            onComplete.Invoke();
    }

    private void Shuffle(List<Vector2Int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Vector2Int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
