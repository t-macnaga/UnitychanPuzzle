using System.Linq;

[System.Serializable]
public class PuzzleQuest
{
    public string name;
    public PuzzleUnitModel[] enemies;

    public PuzzleUnitModel GetEnemy(int index)
    => enemies.FirstOrDefault(x => x.initialIndex == index);
}
