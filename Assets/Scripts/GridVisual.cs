// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEditor;
// using UnityEngine;
// using Utility;
// using Random = UnityEngine.Random;
//
// public class GridVisual : MonoBehaviour
// {
//     private struct UVCoordinates
//     {
//         public Vector2 uv00;
//         public Vector2 uv11;
//     }
//
//     [SerializeField] private MeshRenderer meshRenderer;
//     [SerializeField] private MeshFilter meshFilter;
//     private Grid<Tilemap.TilemapObject> grid;
//     private Mesh mesh;
//     private Vector3[] meshVertices;
//     private Vector2[] meshUv;
//     private int[] meshTriangles;
//     private bool updateMesh, editMesh;
//     private int editX, editY;
//     float textureWidth;
//     float textureHeight;
//
//     private void Awake()
//     {
//         mesh = new Mesh();
//         meshFilter.mesh = mesh;
//
//         Texture texture = meshRenderer.material.mainTexture;
//         textureWidth = texture.width;
//         textureHeight = texture.height;
//     }
//
//     public void SetGrid(Grid<Tilemap.TilemapObject> grid)
//     {
//         this.grid = grid;
//         UpdateTilemapVisual();
//
//         grid.OnGridObjectChanged += Grid_OnGridValueChanged;
//         grid.OnGridObjectEdited += Grid_OnGridValueEdited;
//     }
//
//     private void Grid_OnGridValueChanged(object sender, Grid<Tilemap.TilemapObject>.OnGridObjectChangedEventArgs e)
//     {
//         updateMesh = true;
//     }
//
//     private void Grid_OnGridValueEdited(object sender, Grid<Tilemap.TilemapObject>.OnGridObjectEditedEventArgs e)
//     {
//         editMesh = true;
//         editX = e.x;
//         editY = e.y;
//     }
//
//     private void LateUpdate()
//     {
//         if (updateMesh)
//         {
//             updateMesh = false;
//             UpdateTilemapVisual();
//         }
//         if (editMesh)
//         {
//             editMesh = false;
//             // EditCellVisual(editX, editY);
//         }
//     }
//
//     private void UpdateTilemapVisual()
//     {
//         MeshTools.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out meshVertices, out meshUv, out meshTriangles);
//
//         for (int x = 0; x < grid.GetWidth(); x++) {
//             for (int y = 0; y < grid.GetHeight(); y++) {
//                 int index = x * grid.GetHeight() + y;
//                 float cellSizeX, cellSizeY;
//                 grid.GetCellSize(out cellSizeX, out cellSizeY);
//                 Vector3 quadSize = new Vector3(1 * cellSizeX, 1 * cellSizeY);
//
//                 Tilemap.TilemapObject gridObject = grid.GetGridObject(x, y);
//                 Tilemap.TilemapObject.TilemapSprite tilemapSprite = gridObject.GetTilemapSprite();
//
//                 Vector2 gridValueUV00, gridValueUV11;
//
//                 // UVCoordinates uvCoordinates = GetCoordinateUvs(x, y, tilemapSprite);
//
//                 // gridValueUV00 = uvCoordinates.uv00;
//                 // gridValueUV11 = uvCoordinates.uv11;
//                 //
//                 // if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.None)
//                 // {
//                 //     quadSize = Vector3.zero;
//                 // }
//                 //
//                 // MeshTools.AddToMeshArrays(meshVertices, meshUv, meshTriangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridValueUV00, gridValueUV11);
//             }
//         }
//
//         mesh.vertices = meshVertices;
//         mesh.uv = meshUv;
//         mesh.triangles = meshTriangles;
//     }
//
//     // private void EditCellVisual(int width, int height)
//     // {
//     //     float cellSizeX, cellSizeY;
//     //     grid.GetCellSize(out cellSizeX, out cellSizeY);
//     //     Vector3 quadSize = new Vector3(1 * cellSizeX, 1 * cellSizeY);
//     //     Vector3 pos = grid.GetWorldPosition(width, height) + quadSize * .5f;
//     //     
//     //     Tilemap.TilemapObject gridObject = grid.GetGridObject(width, height);
//     //     Tilemap.TilemapObject.TilemapSprite tilemapSprite = gridObject.GetTilemapSprite();
//     //
//     //     Vector2 gridValueUV00, gridValueUV11;
//     //
//     //     UVCoordinates uvCoordinates = GetCoordinateUvs(width, height, tilemapSprite);
//     //
//     //     gridValueUV00 = uvCoordinates.uv00;
//     //     gridValueUV11 = uvCoordinates.uv11;
//     //
//     //     if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.None)
//     //     {
//     //         quadSize = Vector3.zero;
//     //     }
//     //
//     //     Mesh newMesh = MeshTools.CreateMesh(pos, 0f, quadSize, gridValueUV00, gridValueUV11);
//     //
//     //     int index = width * grid.GetHeight() + height;
//     //     
//     //     for (int i = 0; i < 6; i++)
//     //     {
//     //         if (i < 4)
//     //         {
//     //             meshVertices[index + i] = newMesh.vertices[i];
//     //             meshUv[index + i] = newMesh.uv[i];
//     //         }
//     //         meshTriangles[index + i] = newMesh.triangles[i];
//     //     }
//     //     
//     //     mesh.vertices = meshVertices;
//     //     mesh.uv = meshUv;
//     //     mesh.triangles = meshTriangles;
//     // }
//     //
//     // private UVCoordinates GetCoordinateUvs(int x, int y, Tilemap.TilemapObject.TilemapSprite tilemapSprite)
//     // {
//     //     var returnVal = new UVCoordinates();
//     //
//     //     if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.None)
//     //     {
//     //         returnVal.uv00 = Vector2.zero;
//     //         returnVal.uv11 = Vector2.zero;
//     //     }
//     //     else
//     //     {
//     //         // UVCoordinates uvCoords = uvCoordinatesDictionary[tilemapSprite];
//     //         if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.Ground)
//     //         {
//     //             if ((x + y) % 2 == 0) // Light
//     //             {
//     //                 Constants.TilemapSpriteUV spriteUv = GetRandomUv("LightGrassUvs");
//     //                 returnVal.uv00 = new Vector2(spriteUv.uv00Pixels.x / textureWidth,
//     //                     spriteUv.uv00Pixels.y / textureHeight);
//     //                 returnVal.uv11 = new Vector2(spriteUv.uv11Pixels.x / textureWidth,
//     //                     spriteUv.uv11Pixels.y / textureHeight);
//     //             }
//     //             else // Dark
//     //             {
//     //                 Constants.TilemapSpriteUV spriteUv = GetRandomUv("DarkGrassUvs");
//     //                 returnVal.uv00 = new Vector2(spriteUv.uv00Pixels.x / textureWidth,
//     //                     spriteUv.uv00Pixels.y / textureHeight);
//     //                 returnVal.uv11 = new Vector2(spriteUv.uv11Pixels.x / textureWidth,
//     //                     spriteUv.uv11Pixels.y / textureHeight);
//     //             }
//     //         }
//     //         else if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.Path)
//     //         {
//     //             if ((x + y) % 2 == 0) // Light
//     //             {
//     //                 Constants.TilemapSpriteUV spriteUv = GetRandomUv("LightPathUvs");
//     //                 returnVal.uv00 = new Vector2(spriteUv.uv00Pixels.x / textureWidth,
//     //                     spriteUv.uv00Pixels.y / textureHeight);
//     //                 returnVal.uv11 = new Vector2(spriteUv.uv11Pixels.x / textureWidth,
//     //                     spriteUv.uv11Pixels.y / textureHeight);
//     //             }
//     //             else // Dark
//     //             {
//     //                 Constants.TilemapSpriteUV spriteUv = GetRandomUv("DarkPathUvs");
//     //                 returnVal.uv00 = new Vector2(spriteUv.uv00Pixels.x / textureWidth,
//     //                     spriteUv.uv00Pixels.y / textureHeight);
//     //                 returnVal.uv11 = new Vector2(spriteUv.uv11Pixels.x / textureWidth,
//     //                     spriteUv.uv11Pixels.y / textureHeight);
//     //             }
//     //         }
//     //         else
//     //         {
//     //             returnVal.uv00 = Vector2.zero;
//     //             returnVal.uv11 = Vector2.zero;
//     //         }
//     //     }
//     //
//     //     return returnVal;
//     // }
//     //
//     // private Constants.TilemapSpriteUV GetRandomUv(string targetDict)
//     // {
//     //     return Constants.SpriteUvs[targetDict].ElementAt(Random.Range(0, Constants.SpriteUvs[targetDict].Count)).Value;
//     // }
// }
