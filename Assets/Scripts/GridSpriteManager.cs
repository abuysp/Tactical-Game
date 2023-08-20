using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

public class GridSpriteManager
{
    private List<SpriteObject> spriteObjects = new List<SpriteObject>();
    private Grid<Tilemap.TilemapObject> grid;
    private Sprites sprites; 
    private float cellSizeX = 2f, cellSizeY = 1.5f;

    public GridSpriteManager( float cellSizeX, float cellSizeY, Grid<Tilemap.TilemapObject> grid, Sprites sprites)
    {
        this.grid = grid;
        this.sprites = sprites;
        this.cellSizeX = cellSizeX;
        this.cellSizeY = cellSizeY;
        InitializeList(grid.GetWidth(), grid.GetHeight());
    }

    public void InitializeList(int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = x * height + y;
                Vector3 worldPosition = grid.GetWorldPosition(x, y);
                Tilemap.TilemapObject.TilemapSprite tilemapSprite = grid.GetGridObject(x, y).GetTilemapSprite();
                Sprite sprite = GetSprite(x, y, tilemapSprite);
                spriteObjects.Add(new SpriteObject(x, y, worldPosition, sprite, cellSizeX, cellSizeY));
            }
        }
        
        InitializeVisualization();
    }

    public void InitializeVisualization()
    {
        foreach (var spriteObject in spriteObjects)
        {
            spriteObject.gameObject = new GameObject
            {
                transform =
                {
                    position = spriteObject.worldPosition,
                    localScale = new Vector3(30, 30)
                }
            };
            
            spriteObject.gameObject.name = spriteObject.x.ToString() + " - " + spriteObject.y.ToString();

            spriteObject.spriteRenderer = spriteObject.gameObject.AddComponent<SpriteRenderer>();
            spriteObject.AssignSprite();
        }
    }

    public void EditSprite(int x, int y, Tilemap.TilemapObject.TilemapSprite tilemapSprite)
    {
        Sprite sprite = GetSprite(x, y, tilemapSprite);
        spriteObjects[x * grid.GetHeight() + y].AssignSprite(sprite);
    }

    private Sprite GetSprite(int x, int y, Tilemap.TilemapObject.TilemapSprite tilemapSprite)
    {
        if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.None) return null;

        if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.Ground)
        {
            if ((x + y) % 2 == 0) // Light
                return sprites.GetRandomSprite("LightGrass");
            // Dark
            return sprites.GetRandomSprite("DarkGrass");
        }

        if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.Path)
        {
            if ((x + y) % 2 == 0) // Light
                return sprites.GetRandomSprite("LightPath");
            // Dark
            {
                return sprites.GetRandomSprite("DarkPath");
            }
        }

        return null;

    }

    public class SpriteObject
    {
        public int x;
        public int y;
        public Vector3 worldPosition;
        public Sprite sprite;
        public GameObject gameObject;
        public SpriteRenderer spriteRenderer;

        public SpriteObject(int x, int y, Vector3 worldPosition, Sprite sprite, float cellSizeX, float cellSizeY)
        {
            this.x = x;
            this.y = y;
            this.worldPosition = worldPosition + new Vector3(1f * cellSizeX, 1f * cellSizeY) * .5f;
            this.sprite = sprite;
        }

        public void AssignSprite()
        {
            spriteRenderer.sprite = sprite;
        }
        public void AssignSprite(Sprite inSprite)
        {
            sprite = inSprite;
            spriteRenderer.sprite = sprite;
        }
    }
}
