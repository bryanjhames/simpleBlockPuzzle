using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableBlock : MonoBehaviour
{
    private BlockDataSO data;
    private Vector3 offset;
    private Vector3 originalPosition;

    private bool isDragging = false;

    public Sprite whiteSquareSprite;
    public BlockSpawner spawner;

    public void Initialize(BlockDataSO blockData)
    {
        data = blockData;
        GenerateVisual();
    }

    private GhostVisualizer ghostVisualizer;

    void Start()
    {
        ghostVisualizer = FindObjectOfType<GhostVisualizer>();
    }

    void GenerateVisual()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        float cellSpacing = 1f;

        List<Vector3> cellPositions = new List<Vector3>();

        foreach (var cell in data.cells)
        {
            GameObject square = new GameObject("Cell");
            square.transform.SetParent(transform);
            Vector3 cellPos = new Vector3(cell.x * cellSpacing, cell.y * cellSpacing, -1);
            square.transform.localPosition = cellPos;
            square.transform.localScale = Vector3.one * 0.9f;

            var sr = square.AddComponent<SpriteRenderer>();
            sr.sprite = data.blockSprite;
            sr.color = Color.white;

            sr.sortingLayerName = "Blocks";
            sr.sortingOrder = 0;

            cellPositions.Add(cellPos);
        }

        if (TryGetComponent<BoxCollider2D>(out var existing))
            Destroy(existing);

        Bounds bounds = new Bounds(cellPositions[0], Vector3.zero);
        foreach (var pos in cellPositions)
            bounds.Encapsulate(pos);

        BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
        box.offset = (Vector2)bounds.center;
        box.size = (Vector2)bounds.size + Vector2.one * 0.5f;
    }

    void OnMouseDown()
    {
        isDragging = true;
        originalPosition = transform.position;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mouseWorld.x, mouseWorld.y, 0);

        SetSortingOrder(10);
        StartCoroutine(ScaleUpSmooth());
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = new Vector3(mouseWorld.x, mouseWorld.y, 0) + offset;
            transform.position = targetPos;

            ghostVisualizer.ShowGhost(data, transform.position);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        ghostVisualizer.ClearGhost();
        SetSortingOrder(0);

        if (BoardPlacement.Instance.TryPlaceBlock(data, transform.position))
        {
            spawner.BlockPlaced(this);
            AudioManager.Instance.PlayBlockPlaced();
            Destroy(gameObject);
        }
        else
        {
            transform.position = originalPosition;
            StartCoroutine(ScaleDownSmooth());
        }
    }

    public BlockDataSO GetBlockData()
    {
        return data;
    }

    void SetSortingOrder(int order)
    {
        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.sortingOrder = order;
        }
    }

    private IEnumerator ScaleUpSmooth()
    {
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.one;
        float duration = 0.2f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t / duration);
            yield return null;
        }

        transform.localScale = targetScale;
    }

    private IEnumerator ScaleDownSmooth()
    {
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.one * 0.5f;
        float duration = 0.2f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t / duration);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
