using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap
{
    private Grid<TilemapObject> grid;

    public Tilemap(int width, int height, float cellSizeX, float cellSizeY, Vector3 originPosition, bool showDebug = true)
    {
        grid = new Grid<TilemapObject>(width, height, cellSizeX, cellSizeY, originPosition, (Grid<TilemapObject> g, int x, int y) => new TilemapObject(g, x, y), showDebug);
    }

    public void SetTilemapSprite(Vector3 worldPosition, TilemapObject.TilemapSprite tilemapSprite)
    {
        TilemapObject tilemapObject = grid.GetGridObject(worldPosition);
        if (tilemapObject != null)
        {
            tilemapObject.SetTilemapSprite(tilemapSprite);
        }
    }

    public void SetTileMapSprite(int height, int width, TilemapObject.TilemapSprite tilemapSprite)
    {
        TilemapObject tilemapObject = grid.GetGridObject(height, width);
        if (tilemapObject != null)
        {
            tilemapObject.SetTilemapSprite(tilemapSprite);
        }
    }

    public List<Vector3> GetGridCornersAndCenter(int width, int height)
    {
        var returnList = new List<Vector3>();
        
        returnList.Add(grid.GetWorldPosition(0, 0));
        returnList.Add(grid.GetWorldPosition(width, 0));
        returnList.Add(grid.GetWorldPosition(0, height));
        returnList.Add(grid.GetWorldPosition(width, height));
        returnList.Add(grid.GetWorldPosition(grid.GetWidth() / 2, grid.GetHeight() / 2));

        return returnList;
    }

    public void SetTilemapVisual(TilemapVisual tilemapVisual)
    {
        tilemapVisual.SetGrid(grid);
    }
    
    public class TilemapObject
    {
        public enum TilemapSprite
        {
            None,
            Ground,
            Path
        }

        private int x;
        private int y;
        private Grid<TilemapObject> grid;
        private TilemapSprite tilemapSprite;

        public TilemapObject(Grid<TilemapObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetTilemapSprite(TilemapSprite tilemapSprite)
        {
            this.tilemapSprite = tilemapSprite;
            grid.TriggerGridObjectChanged(x, y);
        }

        public override string ToString()
        {
            return tilemapSprite.ToString();
        }

        public TilemapSprite GetTilemapSprite()
        {
            return tilemapSprite;
        }
    }
}
