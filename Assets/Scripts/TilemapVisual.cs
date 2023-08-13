using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utility;

public class TilemapVisual : MonoBehaviour
{
    [Serializable]
    public struct TilemapSpriteUV
    {
        public Tilemap.TilemapObject.TilemapSprite tilemapSprite;
        public Vector2Int uv00Pixels;
        public Vector2Int uv11Pixels;
    }

    private struct UVCoordinates
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    [SerializeField] private TilemapSpriteUV[] tilemapSpriteUvsArray;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;
    private Grid<Tilemap.TilemapObject> grid;
    private Mesh mesh;
    private bool updateMesh;
    private Dictionary<Tilemap.TilemapObject.TilemapSprite, UVCoordinates> uvCoordinatesDictionary;

    private void Awake()
    {
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        Texture texture = meshRenderer.material.mainTexture;
        float textureWidth = texture.width;
        float textureHeight = texture.height;
        

        uvCoordinatesDictionary = new Dictionary<Tilemap.TilemapObject.TilemapSprite, UVCoordinates>();
        foreach (TilemapSpriteUV tilemapSpriteUV in tilemapSpriteUvsArray)
        {
            uvCoordinatesDictionary[tilemapSpriteUV.tilemapSprite] = new UVCoordinates
            {
                uv00 = new Vector2(tilemapSpriteUV.uv00Pixels.x / textureWidth, tilemapSpriteUV.uv00Pixels.y / textureHeight),
                uv11 = new Vector2(tilemapSpriteUV.uv11Pixels.x / textureWidth, tilemapSpriteUV.uv11Pixels.y / textureHeight),
            };
        }
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
                if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.None)
                {
                    gridValueUV00 = Vector2.zero;
                    gridValueUV11 = Vector2.zero;
                    quadSize = Vector2.zero;
                }
                else
                {
                    UVCoordinates uvCoords = uvCoordinatesDictionary[tilemapSprite];
                    gridValueUV00 = uvCoords.uv00;
                    gridValueUV11 = uvCoords.uv11;
                }

                MeshTools.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridValueUV00, gridValueUV11);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
