using UnityEngine;

[System.Serializable]
public class BoardGridNode
{
    //TODO: replace unit model
    public PuzzleCellType cellType;
    public PuzzleUnitType unitType;
    public PuzzleUnitModel unitModel;
    public int index;
    public Vector2Int position;
}