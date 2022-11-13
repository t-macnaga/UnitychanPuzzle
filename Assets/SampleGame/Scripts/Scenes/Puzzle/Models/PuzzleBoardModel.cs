using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzleBoardModel
{
    public Vector2Int BoardSize { get; private set; }
    public BoardGridNode[,] cellModelGrid;
    public List<BoardGridNode> cellModelList = new List<BoardGridNode>();

    public PuzzleBoardModel(PuzzleQuest quest, Vector2Int boardSize)
    {
        BoardSize = boardSize;
        cellModelGrid = new BoardGridNode[(int)boardSize.y, (int)boardSize.x];
        Setup(quest);
    }

    void Setup(PuzzleQuest quest)
    {
        var index = 0;
        var boardSize = BoardSize;
        var unitTypeIndex = 1;
        for (var y = 0; y < boardSize.y; y++)
        {
            for (var x = 0; x < boardSize.x; x++)
            {
                var enemy = quest.GetEnemy(index);
                var isEnemyNode = enemy != null;
                var model = new BoardGridNode
                {
                    cellType = isEnemyNode ? PuzzleCellType.EnemyCell : PuzzleCellType.SelfCell,
                    unitType = (PuzzleUnitType)unitTypeIndex,
                    position = new Vector2Int(x, y),
                    index = index++
                };
                if (isEnemyNode)
                {
                    model.unitType = enemy.unitType;
                    model.unitModel = enemy;
                }
                cellModelGrid[y, x] = model;
                cellModelList.Add(model);
                unitTypeIndex++;
                //TODO: remove test code.
                if (unitTypeIndex > 5)
                {
                    unitTypeIndex = Random.Range(1, 6);
                }
            }
        }
    }

    public BoardGridNode Get(int index)
    {
        return cellModelList[index];
    }

    public Vector2Int GetNeiborPosition(Vector2Int positionA, SwipeDirection direction)
    {
        var positionB = positionA;
        switch (direction)
        {
            case SwipeDirection.HorizontalPlus:
                positionB.x = positionA.x + 1;
                positionB.y = positionA.y;
                break;
            case SwipeDirection.HorizontalMinus:
                positionB.x = positionA.x - 1;
                positionB.y = positionA.y;
                break;
            case SwipeDirection.VerticalPlus:
                positionB.x = positionA.x;
                positionB.y = positionA.y - 1;
                break;
            case SwipeDirection.VerticalMinus:
                positionB.x = positionA.x;
                positionB.y = positionA.y + 1;
                break;
        }
        return positionB;
    }
}
