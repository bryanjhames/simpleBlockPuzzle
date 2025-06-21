using UnityEngine;

[CreateAssetMenu(fileName = "NewBlockData", menuName = "Block Puzzle/Block Data")]
public class BlockDataSO : ScriptableObject
{
    public string blockName;
    public Vector2Int[] cells;
    public Sprite blockSprite;
}
