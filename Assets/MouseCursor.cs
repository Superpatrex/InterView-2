using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    public Vector3 offSet = Vector3.zero;
    public double pushBack = 1.0f;
    public Camera ComputerCamera;
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = (float)(ComputerCamera.nearClipPlane + pushBack);
        Vector3 worldPosition = ComputerCamera.ScreenToWorldPoint(mousePos) + offSet;
        transform.position = worldPosition;
    }

    public void Hide()
    {
       GetComponent<Image>().enabled = false;
    }

    public void Show()
    {
        GetComponent<Image>().enabled = true;
    }
}
