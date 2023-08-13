using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Units/NewUnit", order = 1)]
public class UnitScriptableObject : ScriptableObject
{
    public int health;
    public int abilityPoints;
    public int attackPower;
    public int defense;
    public int evasion;
    public int moveRange;
    public int attackRange;

    [SerializeField] private Sprite sprite;
}
