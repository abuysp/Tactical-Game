using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class TilemapTest : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TilemapVisual tilemapVisual;
    [SerializeField] private bool showDebug;
    private Tilemap tilemap;
    private Tilemap.TilemapObject.TilemapSprite tilemapSprite;
    [SerializeField] private int width = 10, height = 10;
    private float cellSizeX = 2f, cellSizeY = 1.5f;

    private void Awake()
    {
        // tilemap = new Tilemap(20, 10, 1f, .75f, new Vector3(-9, -5));
        tilemap = new Tilemap(width, height, cellSizeX, cellSizeY, new Vector3(-9, -5), showDebug);
    }

    private void Start()
    {
        tilemap.SetTilemapVisual(tilemapVisual);

        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(i < 1 || i >= width - 1 || j < 1 || j >= height - 1)
                {
                    tilemap.SetTileMapSprite(i, j, Tilemap.TilemapObject.TilemapSprite.Ground);
                }
                else
                {
                    tilemap.SetTileMapSprite(i, j, Tilemap.TilemapObject.TilemapSprite.Path);
                }
            }
        }
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
            tilemap.SetTilemapSprite(mouseWorldPosition, tilemapSprite);
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

