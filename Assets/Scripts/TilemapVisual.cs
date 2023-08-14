using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public class TilemapVisual : MonoBehaviour
{
    private struct UVCoordinates
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;
    private Grid<Tilemap.TilemapObject> grid;
    private Mesh mesh;
    private bool updateMesh;
    private Dictionary<Tilemap.TilemapObject.TilemapSprite, UVCoordinates> uvCoordinatesDictionary;
    float textureWidth;
    float textureHeight;

    private void Awake()
    {
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        Texture texture = meshRenderer.material.mainTexture;
        textureWidth = texture.width;
        textureHeight = texture.height;
        

        uvCoordinatesDictionary = new Dictionary<Tilemap.TilemapObject.TilemapSprite, UVCoordinates>();
        // foreach (TilemapSpriteUV tilemapSpriteUV in tilemapSpriteUvsArray)
        // {
        //     uvCoordinatesDictionary[tilemapSpriteUV.tilemapSprite] = new UVCoordinates
        //     {
        //         uv00 = new Vector2(tilemapSpriteUV.uv00Pixels.x / textureWidth, tilemapSpriteUV.uv00Pixels.y / textureHeight),
        //         uv11 = new Vector2(tilemapSpriteUV.uv11Pixels.x / textureWidth, tilemapSpriteUV.uv11Pixels.y / textureHeight),
        //     };
        // }
    }

    public void SetGrid(Grid<Tilemap.TilemapObject> grid)
    {
        this.grid = grid;
        UpdateTilemapVisual();

        grid.OnGridObjectChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, Grid<Tilemap.TilemapObject>.OnGridObjectChangedEventArgs e)
    {
        updateMesh = true;
    }

    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateTilemapVisual();
        }
    }

    private void UpdateTilemapVisual()
    {
        MeshTools.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                int index = x * grid.GetHeight() + y;
                float cellSizeX, cellSizeY;
                grid.GetCellSize(out cellSizeX, out cellSizeY);
                UnityEngine.Vector3 quadSize = new Vector3(1 * cellSizeX, 1 * cellSizeY);

                Tilemap.TilemapObject gridObject = grid.GetGridObject(x, y);
                Tilemap.TilemapObject.TilemapSprite tilemapSprite = gridObject.GetTilemapSprite();

                Vector2 gridValueUV00, gridValueUV11;

                UVCoordinates uvCoordinates = GetCoordinateUvs(x, y, tilemapSprite);

                gridValueUV00 = uvCoordinates.uv00;
                gridValueUV11 = uvCoordinates.uv11;

                MeshTools.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridValueUV00, gridValueUV11);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    private UVCoordinates GetCoordinateUvs(int x, int y, Tilemap.TilemapObject.TilemapSprite tilemapSprite)
    {
        var returnVal = new UVCoordinates();

        if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.None)
        {
            returnVal.uv00 = Vector2.zero;
            returnVal.uv11 = Vector2.zero;
        }
        else
        {
            // UVCoordinates uvCoords = uvCoordinatesDictionary[tilemapSprite];
            if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.Ground)
            {
                if ((x + y) % 2 == 0) // Light
                {
                    Constants.TilemapSpriteUV spriteUv = GetRandomUv("LightGrassUvs");
                    returnVal.uv00 = new Vector2(spriteUv.uv00Pixels.x / textureWidth,
                        spriteUv.uv00Pixels.y / textureHeight);
                    returnVal.uv11 = new Vector2(spriteUv.uv11Pixels.x / textureWidth,
                        spriteUv.uv11Pixels.y / textureHeight);
                }
                else // Dark
                {
                    Constants.TilemapSpriteUV spriteUv = GetRandomUv("DarkGrassUvs");
                    returnVal.uv00 = new Vector2(spriteUv.uv00Pixels.x / textureWidth,
                        spriteUv.uv00Pixels.y / textureHeight);
                    returnVal.uv11 = new Vector2(spriteUv.uv11Pixels.x / textureWidth,
                        spriteUv.uv11Pixels.y / textureHeight);
                }
            }
            else if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.Path)
            {
                if ((x + y) % 2 == 0) // Light
                {
                    Constants.TilemapSpriteUV spriteUv = GetRandomUv("LightPathUvs");
                    returnVal.uv00 = new Vector2(spriteUv.uv00Pixels.x / textureWidth,
                        spriteUv.uv00Pixels.y / textureHeight);
                    returnVal.uv11 = new Vector2(spriteUv.uv11Pixels.x / textureWidth,
                        spriteUv.uv11Pixels.y / textureHeight);
                }
                else // Dark
                {
                    Constants.TilemapSpriteUV spriteUv = GetRandomUv("DarkPathUvs");
                    returnVal.uv00 = new Vector2(spriteUv.uv00Pixels.x / textureWidth,
                        spriteUv.uv00Pixels.y / textureHeight);
                    returnVal.uv11 = new Vector2(spriteUv.uv11Pixels.x / textureWidth,
                        spriteUv.uv11Pixels.y / textureHeight);
                }
            }
            else
            {
                returnVal.uv00 = Vector2.zero;
                returnVal.uv11 = Vector2.zero;
            }
        }

        return returnVal;
    }

    private Constants.TilemapSpriteUV GetRandomUv(string targetDict)
    {
        return Constants.SpriteUvs[targetDict].ElementAt(Random.Range(0, Constants.SpriteUvs[targetDict].Count)).Value;
    }
}
