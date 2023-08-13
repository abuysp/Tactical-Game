using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitScriptableObject unitObject;

    private void Start()
    {
        Debug.Log(unitObject.health);

        var test = new UnitScriptableObject();
    }
}
