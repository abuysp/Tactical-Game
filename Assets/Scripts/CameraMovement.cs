using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private TilemapTest tilemapTest;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float zoomStep, minCamSize, maxCamSize;
    private float minXLimit, maxXLimit, minYLimit, maxYLimit, centerX, centerY;

    private Vector3 dragOrigin;

    private void Start()
    {
        var gridPositions = tilemapTest.GetGridCornersAndCenter();
        minXLimit = Mathf.Min(gridPositions[0].x, gridPositions[1].x);
        maxXLimit = Mathf.Max(gridPositions[2].x, gridPositions[3].x);
        minYLimit = Mathf.Min(gridPositions[0].y, gridPositions[2].y);
        maxYLimit = Mathf.Max(gridPositions[1].y, gridPositions[3].y);
        centerX = gridPositions[4].x;
        centerY = gridPositions[4].y;
    }

    // Update is called once per frame
    void Update()
    {
        PanCamera();

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            ZoomIn();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            ZoomOut();
        }
    }
    
    private void PanCamera()
    {
        //Save position of mouse in world space when drag starts, then calculate distance between drag origin and new
        //position. If it's still held down then move the camera by that distance

        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Tools.GetMouseWorldPositionWithZ(mainCamera);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 difference = dragOrigin - Tools.GetMouseWorldPositionWithZ(mainCamera);

            mainCamera.transform.position += difference;

            mainCamera.transform.position = ClampCamera(mainCamera.transform.position);
        }
    }

    private void ZoomIn()
    {
        float newSize = mainCamera.orthographicSize - zoomStep;

        mainCamera.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

        mainCamera.transform.position = ClampCamera(mainCamera.transform.position);
    }
    
    private void ZoomOut()
    {
        float newSize = mainCamera.orthographicSize + zoomStep;

        mainCamera.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

        mainCamera.transform.position = ClampCamera(mainCamera.transform.position);
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        float minX, maxX, minY, maxY;
        float newX, newY;
        
        // BUG: Handle snapping to center when the grid width or height is odd.

        if (camHeight > tilemapTest.GetTilemapHeight() && camWidth > tilemapTest.GetTilemapWidth())
        {
            return new Vector3(centerX, centerY, targetPosition.z);
        }

        if (camHeight > tilemapTest.GetTilemapHeight())
        {
            minX = minXLimit + camWidth;
            maxX = maxXLimit - camWidth;
        
            newX = Mathf.Clamp(targetPosition.x, minX, maxX);

            return new Vector3(newX, centerY, targetPosition.z);
        }
        
        if (camWidth > tilemapTest.GetTilemapWidth())
        {
            minY = minYLimit + camHeight;
            maxY = maxYLimit - camHeight;
        
            newY = Mathf.Clamp(targetPosition.y, minY, maxY);

            return new Vector3(centerX, newY, targetPosition.z);
        }

        minX = minXLimit + camWidth;
        maxX = maxXLimit - camWidth;
    
        minY = minYLimit + camHeight;
        maxY = maxYLimit - camHeight;
        
        newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        newY = Mathf.Clamp(targetPosition.y, minY, maxY);
        
        return new Vector3(newX, newY, targetPosition.z);
    }
}
