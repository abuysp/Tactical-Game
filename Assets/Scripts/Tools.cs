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
}
