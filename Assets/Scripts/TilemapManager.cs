using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class TilemapManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    // [FormerlySerializedAs("tilemapVisual")] [SerializeField] private GridVisual gridVisual;
    [SerializeField] private bool showDebug;
    private Tilemap tilemap;
    private Tilemap.TilemapObject.TilemapSprite tilemapSprite;
    [SerializeField] private int width = 10, height = 10;
    [SerializeField] private Sprites sprites;
    private float cellSizeX = 2f, cellSizeY = 1.5f;

    private void Awake()
    {
        // tilemap = new Tilemap(20, 10, 1f, .75f, new Vector3(-9, -5));
        List<Tilemap.TilemapObject.TilemapSprite> tilemapSprites = new List<Tilemap.TilemapObject.TilemapSprite>();
        
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(i < 3 || i >= width - 3 || j < 3 || j >= height - 3)
                {
                    tilemapSprites.Add(Tilemap.TilemapObject.TilemapSprite.Ground);
                }
                else
                {
                    tilemapSprites.Add(Tilemap.TilemapObject.TilemapSprite.Path);
                }
            }
        }
        
        tilemap = new Tilemap(width, height, cellSizeX, cellSizeY, new Vector3(-9, -5), new List<UnitScriptableObject>(width * height), sprites, tilemapSprites, showDebug);
    }

    private void Start()
    {
        // tilemap.SetTilemapVisual(gridVisual);

        
    }

    public List<Vector3> GetGridCornersAndCenter()
    {
        return tilemap.GetGridCornersAndCenter(width, height);
    }

    public float GetTilemapWidth()
    {
        return width * cellSizeX / 2;
    }

    public float GetTilemapHeight()
    {
        return height * cellSizeY / 2;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Tools.GetMouseWorldPositionWithZ(mainCamera);
            tilemap.EditSprite(mouseWorldPosition, tilemapSprite);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            tilemapSprite = Tilemap.TilemapObject.TilemapSprite.None;
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Ground;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Path;
        }
    }
}

