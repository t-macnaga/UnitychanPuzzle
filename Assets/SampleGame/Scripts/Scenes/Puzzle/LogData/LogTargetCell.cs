public class LogTargetCell
{
    public int index;
    public PuzzleCellType cellType;
    public PuzzleUnitType unitType;

    public void Clear()
    {
        index = default;
        cellType = default;
        unitType = default;
    }
}