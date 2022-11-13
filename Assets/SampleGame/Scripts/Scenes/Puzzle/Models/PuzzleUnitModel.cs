public class PuzzleUnitModel
{
    public int hp;
    public int maxHp;
    public int turn;
    public int maxTurn;
    public int initialIndex;
    public PuzzleUnitType unitType;
    public bool IsDead => hp <= 0;
    public void Damage(int point)
    {
        hp -= point;
    }
}