using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using Utility;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSizeX;
    private float cellSizeY;
    private Vector3 originPosition;

     public int GetWidth() {
         return width;
     }

     public int GetHeight() {
         return height;
     }

     public void GetCellSize(out float x, out float y)
     {
         x = cellSizeX;
         y = cellSizeY;
     }

    private TGridObject[,] gridArray;

    public Grid(int width, int height, float cellSizeX, float cellSizeY, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject, bool showDebug = true){
        this.width = width;
        this.height = height;
        this.cellSizeX = cellSizeX;
        this.cellSizeY = cellSizeY;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width,height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }
        
        if(showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = Tools.CreateWorldText(text: gridArray[x, y]?.ToString(),
                        localPosition: GetWorldPosition(x, y) + new Vector3(cellSizeX, cellSizeY) * .5f, fontSize: 40,
                        color: Color.white, textAnchor: TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
        }       
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public void SetGridObject(int x, int y, TGridObject targetValue)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = targetValue;
            TriggerGridObjectChanged(x, y);
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject targetValue)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y ,targetValue);
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition-originPosition).x / cellSizeX);
        y = Mathf.FloorToInt((worldPosition-originPosition).y / cellSizeY);
    }

    public Vector3 GetWorldPosition(int x, int y){
        return new Vector3(x * cellSizeX, y * cellSizeY) + originPosition;
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }
}
