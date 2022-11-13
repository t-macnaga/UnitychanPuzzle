using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RemoveCellsLogic
{
    public List<int> RemovableCellIndices = new List<int>();
    public List<int> TargetEnemyIndices = new List<int>();
    List<int> tempIndices = new List<int>();
    PuzzleUnitType currentUnitType;

    public void Log(PuzzleContext context, ref List<PuzzleLogData> logList)
    {
        Clear();
        VerticalRemovable(context);
        HorizontalRemovable(context);
        ApplyTo(context.Model);
        var removable = RemovableCellIndices;
        if (removable.Count > 0)
        {
            var boardModel = context.Model;
            foreach (var index in RemovableCellIndices)
            {
                var position = context.Model.Get(index).position;
                AddTargetEnemy(boardModel, position, SwipeDirection.HorizontalMinus);
                AddTargetEnemy(boardModel, position, SwipeDirection.HorizontalPlus);
                AddTargetEnemy(boardModel, position, SwipeDirection.VerticalMinus);
                AddTargetEnemy(boardModel, position, SwipeDirection.VerticalPlus);
            }

            LogRemoveCells(context, ref logList);
            if (TargetEnemyIndices.Count > 0)
            {
                TargetEnemyIndices = TargetEnemyIndices.Distinct().ToList();
                var targetEnemies = TargetEnemyIndices.Select(x => (x, boardModel.Get(x).unitModel)).ToList();
                foreach (var targetEnemy in targetEnemies)
                {
                    //TODO: attack point
                    targetEnemy.unitModel.Damage(1);
                }
                var logData = context.Logic.CreateLogData();
                logData.logType = PuzzleLogType.AttackCells;
                foreach (var index in TargetEnemyIndices)
                {
                    var target = context.Logic.CreateLogTarget(index);
                    logData.targetCellList.Add(target);
                }
                logList.Add(logData);
                var deadModels = targetEnemies.Where(x => x.unitModel.IsDead).ToList();
                if (deadModels.Count > 0)
                {
                    RemovableCellIndices = deadModels.Select(x => x.x).ToList();
                    ApplyTo(context.Model);
                    LogRemoveCells(context, ref logList);
                }
            }
        }
    }

    void LogRemoveCells(PuzzleContext context, ref List<PuzzleLogData> logList)
    {
        var logData = context.Logic.CreateLogData();
        logData.logType = PuzzleLogType.RemoveCells;
        logData.targetCellList = RemovableCellIndices
            .Distinct()
            .Select(x => context.Logic.CreateLogTarget(x))
            .ToList();
        logList.Add(logData);
    }

    void AddTargetEnemy(PuzzleBoardModel model, Vector2Int position, SwipeDirection direction)
    {
        var neiborPosition = model.GetNeiborPosition(position, direction);
        if (0 <= neiborPosition.x && neiborPosition.x < model.BoardSize.x &&
            0 <= neiborPosition.y && neiborPosition.y < model.BoardSize.y)
        {
            var neibor = model.cellModelGrid[neiborPosition.y, neiborPosition.x];
            if (neibor.cellType == PuzzleCellType.EnemyCell)
            {
                TargetEnemyIndices.Add(neibor.index);
            }
        }
    }

    void VerticalRemovable(PuzzleContext context)
    {
        var boardModel = context.Model;
        var boardSize = boardModel.BoardSize;
        for (var y = 0; y < boardSize.y; y++)
        {
            currentUnitType = default(PuzzleUnitType);
            tempIndices.Clear();
            for (var x = 0; x < boardSize.x; x++)
            {
                var model = boardModel.cellModelGrid[y, x];
                AddRemovable(model);
            }
            AddTempIndicesToRemovables();
        }
    }

    void HorizontalRemovable(PuzzleContext context)
    {
        var boardModel = context.Model;
        var boardSize = boardModel.BoardSize;
        for (var x = 0; x < boardSize.x; x++)
        {
            currentUnitType = default(PuzzleUnitType);
            tempIndices.Clear();
            for (var y = 0; y < boardSize.y; y++)
            {
                var model = boardModel.cellModelGrid[y, x];
                AddRemovable(model);
            }
            AddTempIndicesToRemovables();
        }
    }

    void ApplyTo(PuzzleBoardModel boardModel)
    {
        foreach (var index in RemovableCellIndices)
        {
            var model = boardModel.Get(index);
            model.cellType = PuzzleCellType.Empty;
            model.unitType = PuzzleUnitType.None;
        }
    }

    public void Clear()
    {
        RemovableCellIndices.Clear();
        tempIndices.Clear();
        TargetEnemyIndices.Clear();
    }

    void AddRemovable(BoardGridNode model)
    {
        if (model.cellType == PuzzleCellType.Empty ||
            model.cellType == PuzzleCellType.EnemyCell)
        {
            AddTempIndicesToRemovables();
            currentUnitType = PuzzleUnitType.None;
        }
        else if (model.cellType == PuzzleCellType.SelfCell)
        {
            if (currentUnitType != model.unitType)
            {
                AddTempIndicesToRemovables();
                currentUnitType = model.unitType;
            }
            tempIndices.Add(model.index);
        }
    }

    void AddTempIndicesToRemovables()
    {
        if (tempIndices.Count >= 3)
        {
            RemovableCellIndices.AddRange(tempIndices);
        }
        tempIndices.Clear();
    }
}