using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class Tools
    {
        public static TextMesh CreateWorldText(string text, Vector3 localPosition, Color color, int fontSize = 50,
            Transform parent = null, TextAnchor textAnchor = TextAnchor.UpperLeft,
            TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
        {
            GameObject textObject = new GameObject("WorldText", typeof(TextMesh));
            Transform textTransform = textObject.transform;
            textTransform.localScale = new Vector3(0.1f, 0.1f, 0);
            textTransform.SetParent(parent, false);
            textTransform.localPosition = localPosition;
            TextMesh textMesh = textObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }

        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0;
            return vec;
        }

        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
        
    }
    public class Constants
    {
	    public static Dictionary<String, Dictionary<String, TilemapSpriteUV>> SpriteUvs =
		    new Dictionary<string, Dictionary<string, TilemapSpriteUV>>
		    {
			    {
				    "LightGrassUvs",
				    new Dictionary<String, TilemapSpriteUV>
				    {
					    {
						    "LightGrass",
						    new TilemapSpriteUV
						    {
							    tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Ground,
							    uv00Pixels = new Vector2Int(40, 75),
							    uv11Pixels = new Vector2Int(59, 89)
						    }
					    },
					    {
						    "LightShortGrass",
						    new TilemapSpriteUV
						    {
							    tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Ground,
							    uv00Pixels = new Vector2Int(0, 75),
							    uv11Pixels = new Vector2Int(19, 89)
						    }
					    }
				    }
			    },
			    {
				    "DarkGrassUvs",
				    new Dictionary<String, TilemapSpriteUV>
				    {
					    {
						    "DarkGrass",
						    new TilemapSpriteUV
						    {
							    tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Ground,
							    uv00Pixels = new Vector2Int(60, 75),
							    uv11Pixels = new Vector2Int(79, 89)
						    }
					    },
					    {
						    "DarkShortGrass",
						    new TilemapSpriteUV
						    {
							    tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Ground,
							    uv00Pixels = new Vector2Int(20, 75),
							    uv11Pixels = new Vector2Int(39, 89)
						    }
					    }
				    }
			    },
			    {
				    "LightPathUvs",
				    new Dictionary<String, TilemapSpriteUV>
				    {
					    {
						    "LightPath",
						    new TilemapSpriteUV
						    {
							    tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Ground,
							    uv00Pixels = new Vector2Int(100, 75),
							    uv11Pixels = new Vector2Int(119, 89)
						    }
					    }
				    }
			    },
			    {
				    "DarkPathUvs", new Dictionary<String, TilemapSpriteUV>
				    {
					    {
						    "DarkPath",
						    new TilemapSpriteUV
						    {
							    tilemapSprite = Tilemap.TilemapObject.TilemapSprite.Ground,
							    uv00Pixels = new Vector2Int(120, 75),
							    uv11Pixels = new Vector2Int(139, 89)
						    }
					    }
				    }
			    }
		    };

	    public struct TilemapSpriteUV
        {
            public Tilemap.TilemapObject.TilemapSprite tilemapSprite;
            public Vector2Int uv00Pixels;
            public Vector2Int uv11Pixels;
        }
    }
}

public static class MeshTools {
    
    private static readonly Vector3 Vector3zero = Vector3.zero;
    private static readonly Vector3 Vector3one = Vector3.one;
    private static readonly Vector3 Vector3yDown = new Vector3(0,-1);

    
    private static Quaternion[] cachedQuaternionEulerArr;
    private static void CacheQuaternionEuler() {
        if (cachedQuaternionEulerArr != null) return;
        cachedQuaternionEulerArr = new Quaternion[360];
        for (int i=0; i<360; i++) {
            cachedQuaternionEulerArr[i] = Quaternion.Euler(0,0,i);
        }
    }
    private static Quaternion GetQuaternionEuler(float rotFloat) {
        int rot = Mathf.RoundToInt(rotFloat);
        rot = rot % 360;
        if (rot < 0) rot += 360;
        //if (rot >= 360) rot -= 360;
        if (cachedQuaternionEulerArr == null) CacheQuaternionEuler();
        return cachedQuaternionEulerArr[rot];
    }


    public static Mesh CreateEmptyMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[0];
        mesh.uv = new Vector2[0];
        mesh.triangles = new int[0];
        return mesh;
    }

    public static void CreateEmptyMeshArrays(int quadCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles) {
		vertices = new Vector3[4 * quadCount];
		uvs = new Vector2[4 * quadCount];
		triangles = new int[6 * quadCount];
    }
        
    public static Mesh CreateMesh(Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11) {
        return AddToMesh(null, pos, rot, baseSize, uv00, uv11);
    }

    public static Mesh AddToMesh(Mesh mesh, Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11) {
        if (mesh == null) {
            mesh = CreateEmptyMesh();
        }
		Vector3[] vertices = new Vector3[4 + mesh.vertices.Length];
		Vector2[] uvs = new Vector2[4 + mesh.uv.Length];
		int[] triangles = new int[6 + mesh.triangles.Length];
            
        mesh.vertices.CopyTo(vertices, 0);
        mesh.uv.CopyTo(uvs, 0);
        mesh.triangles.CopyTo(triangles, 0);

        int index = vertices.Length / 4 - 1;
		//Relocate vertices
		int vIndex = index*4;
		int vIndex0 = vIndex;
		int vIndex1 = vIndex+1;
		int vIndex2 = vIndex+2;
		int vIndex3 = vIndex+3;

        baseSize *= .5f;

        bool skewed = baseSize.x != baseSize.y;
        if (skewed) {
			vertices[vIndex0] = pos+GetQuaternionEuler(rot)*new Vector3(-baseSize.x,  baseSize.y);
			vertices[vIndex1] = pos+GetQuaternionEuler(rot)*new Vector3(-baseSize.x, -baseSize.y);
			vertices[vIndex2] = pos+GetQuaternionEuler(rot)*new Vector3( baseSize.x, -baseSize.y);
			vertices[vIndex3] = pos+GetQuaternionEuler(rot)*baseSize;
		} else {
			vertices[vIndex0] = pos+GetQuaternionEuler(rot-270)*baseSize;
			vertices[vIndex1] = pos+GetQuaternionEuler(rot-180)*baseSize;
			vertices[vIndex2] = pos+GetQuaternionEuler(rot- 90)*baseSize;
			vertices[vIndex3] = pos+GetQuaternionEuler(rot-  0)*baseSize;
		}
		
		//Relocate UVs
		uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
		uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
		uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
		uvs[vIndex3] = new Vector2(uv11.x, uv11.y);
		
		//Create triangles
		int tIndex = index*6;
		
		triangles[tIndex+0] = vIndex0;
		triangles[tIndex+1] = vIndex3;
		triangles[tIndex+2] = vIndex1;
		
		triangles[tIndex+3] = vIndex1;
		triangles[tIndex+4] = vIndex3;
		triangles[tIndex+5] = vIndex2;
            
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;

        //mesh.bounds = bounds;

        return mesh;
    }

    public static void AddToMeshArrays(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11) {
		//Relocate vertices
		int vIndex = index*4;
		int vIndex0 = vIndex;
		int vIndex1 = vIndex+1;
		int vIndex2 = vIndex+2;
		int vIndex3 = vIndex+3;

        baseSize *= .5f;

        bool skewed = baseSize.x != baseSize.y;
        if (skewed) {
			vertices[vIndex0] = pos+GetQuaternionEuler(rot)*new Vector3(-baseSize.x,  baseSize.y);
			vertices[vIndex1] = pos+GetQuaternionEuler(rot)*new Vector3(-baseSize.x, -baseSize.y);
			vertices[vIndex2] = pos+GetQuaternionEuler(rot)*new Vector3( baseSize.x, -baseSize.y);
			vertices[vIndex3] = pos+GetQuaternionEuler(rot)*baseSize;
		} else {
			vertices[vIndex0] = pos+GetQuaternionEuler(rot-270)*baseSize;
			vertices[vIndex1] = pos+GetQuaternionEuler(rot-180)*baseSize;
			vertices[vIndex2] = pos+GetQuaternionEuler(rot- 90)*baseSize;
			vertices[vIndex3] = pos+GetQuaternionEuler(rot-  0)*baseSize;
		}
		
		//Relocate UVs
		uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
		uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
		uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
		uvs[vIndex3] = new Vector2(uv11.x, uv11.y);
		
		//Create triangles
		int tIndex = index*6;
		
		triangles[tIndex+0] = vIndex0;
		triangles[tIndex+1] = vIndex3;
		triangles[tIndex+2] = vIndex1;
		
		triangles[tIndex+3] = vIndex1;
		triangles[tIndex+4] = vIndex3;
		triangles[tIndex+5] = vIndex2;
    }
}
