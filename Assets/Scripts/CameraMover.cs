using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] float axisMovementSpeed = 1;
    [SerializeField, Range(0.01f, 0.9f)] float scrollRate = 0.2f;

    Vector2 prevMousePos, mousePositionDelta;

    Camera mainCamera;

    float pixelsToUnits;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Movement();
        Zoom();
    }

    private void Movement()
    {
        pixelsToUnits = (mainCamera.orthographicSize * mainCamera.aspect * 2) / Screen.width;

        if (Input.GetMouseButton(1))
        {
            mousePositionDelta = prevMousePos - (Vector2)Input.mousePosition;

            transform.position += pixelsToUnits * (Vector3)mousePositionDelta;
        }
        else
        {
            Vector3 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            transform.position += axisMovementSpeed * pixelsToUnits * Time.deltaTime * input;
        }

        prevMousePos = Input.mousePosition;
    }

    void Zoom()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            mainCamera.orthographicSize *= 1 + (-Input.mouseScrollDelta.y * scrollRate);
        }
    }
}
