using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width, 
        Height,
        FixedRows,
        FixedColumns
    }
    
    [Space]
    [Header("Fit Properties")]
    public FitType fitType;

    [Space]
    public bool fitX;
    public bool fitY;

    
    [Space]
    [EnableIf("CanAdjustRowsManually")] public int rows;
    [EnableIf("CanAdjustColumnsManually")] public int columns;

    [Space]
    public Vector2 cellSize;
    public Vector2 spacing;


    private bool CanAdjustRowsManually()
    {
        return fitType == FitType.FixedRows;
    }
    
    private bool CanAdjustColumnsManually()
    {
        return fitType == FitType.FixedColumns;
    }

    
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        if(transform.childCount == 0)
            return;
        
        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
           
            float sqr = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqr);
            columns = Mathf.CeilToInt(sqr);
        }

        if (fitType == FitType.Width || fitType == FitType.FixedColumns)
        {
           
            rows = Mathf.CeilToInt(transform.childCount / (float) columns);
        }

        if (fitType == FitType.Height || fitType == FitType.FixedRows)
        {
           
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }
        
        
        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / columns) - ((spacing.x / columns) * (columns - 1)) - (padding.left / columns) - (padding.right / columns);
        float cellHeight = (parentHeight / rows) -  ((spacing.y / rows) * (rows - 1)) - (padding.top / rows) - (padding.bottom / rows);

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;
        

        int columnCount = 0;
        int rowCount = 0;
        
        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;
            
            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }


    }

    public override void CalculateLayoutInputVertical()
    {
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
    }
}
