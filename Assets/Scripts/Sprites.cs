using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprites : MonoBehaviour
{
    [SerializeField] public List<Sprite> LightGrassList;
    [SerializeField] public List<Sprite> DarkGrassList;
    [SerializeField] public List<Sprite> LightPathList;
    [SerializeField] public List<Sprite> DarkPathList;

    public Sprite GetRandomSprite(string targetDict)
    {
        switch (targetDict)
        {
            case "LightGrass":
                return LightGrassList[Random.Range(0, LightGrassList.Count)];
            case "DarkGrass":
                return DarkGrassList[Random.Range(0, DarkGrassList.Count)];
            case "LightPath":
                return LightPathList[Random.Range(0, LightPathList.Count)];
            case "DarkPath":
                return DarkPathList[Random.Range(0, DarkPathList.Count)];
            default:
                return null;
        }
    }
}
