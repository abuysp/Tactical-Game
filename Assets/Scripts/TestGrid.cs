using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class TestGrid : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Grid<StringGridObject> grid;
    
    void Start()
    {
        grid = new Grid<StringGridObject>(5, 5, 1.5f, new Vector3(-4, 6), (Grid<StringGridObject> g, int x, int y) => new StringGridObject(g, x, y));
    }

    private void Update()
    {
        Vector3 position = Tools.GetMouseWorldPositionWithZ(mainCamera);
        // if (Input.GetMouseButtonDown(0))
        // {
        //     grid.SetGridObject(Tools.GetMouseWorldPositionWithZ(mainCamera), true);
        // }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetGridObject(Tools.GetMouseWorldPositionWithZ(mainCamera)));
        }
        
        if(Input.GetKeyDown(KeyCode.A)) {grid.GetGridObject(position).AddLetter("A");}
        if(Input.GetKeyDown(KeyCode.S)) {grid.GetGridObject(position).AddLetter("S");}
        if(Input.GetKeyDown(KeyCode.D)) {grid.GetGridObject(position).AddLetter("D");}
        
        if(Input.GetKeyDown(KeyCode.Alpha1)) {grid.GetGridObject(position).AddLetter("1");}
        if(Input.GetKeyDown(KeyCode.Alpha2)) {grid.GetGridObject(position).AddLetter("2");}
        if(Input.GetKeyDown(KeyCode.Alpha3)) {grid.GetGridObject(position).AddLetter("3");}
    }
}

public class StringGridObject
{
    private string letters;
    private string numbers;

    private Grid<StringGridObject> grid;
    private int x;
    private int y;

    public StringGridObject(Grid<StringGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        letters = "";
        numbers = "";
    }

    public void AddLetter(string letter)
    {
        letters += letter;
        grid.TriggerGridObjectChanged(x, y);
    }
    
    public void AddNumber(string number)
    {
        numbers += number;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString()
    {
        return letters + "\n" + numbers;
    }
}
