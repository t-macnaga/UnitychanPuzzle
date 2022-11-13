using System.Collections.Generic;
using UnityEngine;

public class PuzzleBoardView : MonoBehaviour
{
    public PuzzleCell cellTemplate;
    public List<PuzzleCell> cellList = new List<PuzzleCell>();

    public void Create(PuzzleBoardModel boardModel)
    {
        foreach (var model in boardModel.cellModelGrid)
        {
            var cell = Instantiate(
                cellTemplate, cellTemplate.transform.parent);
            var rect = cell.transform as RectTransform;
            rect.anchoredPosition = new Vector2(model.position.x * 100F, -model.position.y * 100F);
            cell.Setup(model.index, model.cellType, model.unitType, model.unitModel);
            cellList.Add(cell);
        }
        cellTemplate.gameObject.SetActive(false);
    }

    public PuzzleCell GetCell(int index)
    {
        return cellList[index];
    }
}
