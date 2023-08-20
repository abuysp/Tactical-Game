using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap
{
    private Grid<TilemapObject> grid;
    private GridSpriteManager gridSpriteManager;
    private Sprites sprites;

    public Tilemap(int width, int height, float cellSizeX, float cellSizeY, Vector3 originPosition, List<UnitScriptableObject> unitObjects, Sprites sprites, List<TilemapObject.TilemapSprite> tilemapSprites,  bool showDebug = true)
    {
        grid = new Grid<TilemapObject>(width, height, cellSizeX, cellSizeY, originPosition, (Grid<TilemapObject> g, int x, int y) => new TilemapObject(g, x, y), showDebug);

        this.sprites = sprites;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int index = i * height + j;
                SetTileMapSprite(i, j, tilemapSprites[index]);
            }
        }

        // for (int i = 0; i < width; i++)
        // {
        //     for (int j = 0; j < height; j++)
        //     {
        //         SetUnitObject(i, j, unitObjects[i * height + j]);
        //     }
        // }

        gridSpriteManager = new GridSpriteManager(cellSizeX, cellSizeY, grid, this.sprites);
    }

    public void EditSprite(Vector3 mouseWorldPosition, TilemapObject.TilemapSprite tilemapSprite)
    {
        grid.GetXY(mouseWorldPosition, out int x, out int y);
        gridSpriteManager.EditSprite(x, y, tilemapSprite);
    }

    public void Debug(Vector3 mousePos)
    {
        grid.GetXY(mousePos, out int x, out int y);
        UnityEngine.Debug.Log(grid.GetWorldPosition(x, y).ToString());
    }
    
    public void SetUnitObject(Vector3 worldPosition, UnitScriptableObject unitObject)
    {
        TilemapObject tilemapObject = grid.GetGridObject(worldPosition);
        if (tilemapObject != null)
        {
            tilemapObject.SetUnitObject(unitObject);
        }
    }

    public void SetUnitObject(int height, int width, UnitScriptableObject unitObject)
    {
        TilemapObject tilemapObject = grid.GetGridObject(height, width);
        if (tilemapObject != null)
        {
            tilemapObject.SetUnitObject(unitObject);
        }
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

    // public void SetTilemapVisual(GridVisual gridVisual)
    // {
    //     gridVisual.SetGrid(grid);
    // }
    
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
        private UnitScriptableObject unitObject;
        private Unit unit;
        private TilemapSprite tilemapSprite;

        public TilemapObject(Grid<TilemapObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetUnitObject(UnitScriptableObject unitObject)
        {
            this.unitObject = unitObject;
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
