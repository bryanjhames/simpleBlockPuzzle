using System.Collections.Generic;
using UnityEngine;

public class GhostVisualizer : MonoBehaviour
{
    public Sprite ghostSprite;
    private List<GameObject> ghostCells = new List<GameObject>();

    public void ShowGhost(BlockDataSO blockData, Vector3 worldPos)
    {
        ClearGhost();

        Vector2 boardOrigin = new Vector2(-4f, -4f); // 9x9 board center
        Vector2 relativePos = (Vector2)worldPos - boardOrigin;
        Vector2Int boardPos = Vector2Int.RoundToInt(relativePos);

        // Check overall placement validity first
        bool valid = BoardPlacement.Instance.CanPlaceAt(blockData, boardPos);

        foreach (var cell in blockData.cells)
        {
            Vector2Int pos = boardPos + cell;

            // Only draw ghost if inside board bounds
            if (pos.x >= 0 && pos.x < 9 && pos.y >= 0 && pos.y < 9)
            {
                Vector3 ghostWorldPos = new Vector3(pos.x - 4f, pos.y - 4f, -1f);
                GameObject ghostCell = new GameObject("GhostCell");
                ghostCell.transform.position = ghostWorldPos;
                ghostCell.transform.localScale = Vector3.one * 0.9f;

                var sr = ghostCell.AddComponent<SpriteRenderer>();
                sr.sprite = ghostSprite;

                Color c = valid ? new Color(1f, 1f, 1f, 0.3f) : new Color(1f, 0f, 0f, 0.3f);
                sr.color = c;

                sr.sortingLayerName = "Blocks";
                sr.sortingOrder = -1;

                ghostCells.Add(ghostCell);
            }
        }
    }

    public void ClearGhost()
    {
        foreach (var go in ghostCells)
        {
            Destroy(go);
        }
        ghostCells.Clear();
    }
}
