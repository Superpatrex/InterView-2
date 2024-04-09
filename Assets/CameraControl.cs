using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera OverheadCamera;
    public Camera FloorCamera;
    public Camera SpectatorCamera;
    public Camera MenuCamera;
    public GameObject TeacherGUI;
    public Canvas canvas;

    private static CameraControl ThisInstance;
    private Camera LastCamera;
    private Camera CurrentCamera;
    private float oRotation = -90;
    private float fRotation = -90;

    // Start is called before the first frame update
    void Start()
    {
        OverheadCamera.enabled = false;
        FloorCamera.enabled = false;
        SpectatorCamera.enabled = false;
        MenuCamera.enabled = true;
        CurrentCamera = MenuCamera;
        LastCamera = FloorCamera;
        ThisInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        bool updateGUI = false;

        if (!MenuCamera.enabled)
        {
            //if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    CurrentCamera.enabled = false;
            //    CurrentCamera = SpectatorCamera;
            //    CurrentCamera.enabled = true;
            //}

            //if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    CurrentCamera.enabled = false;
            //    CurrentCamera = OverheadCamera;
            //    canvas.worldCamera = OverheadCamera;
            //    CurrentCamera.enabled = true;
            //}

            //if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
            //{
            //    CurrentCamera.enabled = false;
            //    CurrentCamera = FloorCamera;
            //    canvas.worldCamera = FloorCamera;
            //    CurrentCamera.enabled = true;
            //}
        }

        if (OverheadCamera.enabled)
        {
            updateGUI = true;

            //if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            //{
            //    oRotation += 45f * (Time.deltaTime);
            //}

            //if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
            //{
            //    oRotation -= 45f * (Time.deltaTime);
            //}

            OverheadCamera.transform.position = Quaternion.Euler(0, oRotation, 0) * new Vector3(0, 5.2f, -5.5f);
            OverheadCamera.transform.rotation = Quaternion.LookRotation(SpectatorCamera.transform.position - OverheadCamera.transform.position);
        }

        if (FloorCamera.enabled)
        {
            updateGUI = true;

            //if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
            //{
            //    fRotation += 45f * (Time.deltaTime);
            //    if (fRotation > 0)
            //        fRotation = 0;
            //}

            //if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
            //{
            //    fRotation -= 45f * (Time.deltaTime);
            //    if (fRotation < -180)
            //        fRotation = -180;
            //}

            FloorCamera.transform.position = Quaternion.Euler(0, fRotation, 0) * new Vector3(0, 1.5f, -3.5f);
            FloorCamera.transform.rotation = Quaternion.LookRotation(SpectatorCamera.transform.position - FloorCamera.transform.position + new Vector3(-2, 0, 0));
        }

        if (updateGUI)
        {
            if (CurrentCamera == OverheadCamera)
                TeacherGUI.transform.position = CurrentCamera.transform.position + 2.2f * CurrentCamera.transform.forward;
            else
                TeacherGUI.transform.position = CurrentCamera.transform.position + 1.2f * CurrentCamera.transform.forward;
            TeacherGUI.transform.rotation = CurrentCamera.transform.rotation;
            //if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            //{
            //    OverheadCamera.cullingMask ^= 1 << 9;
            //    FloorCamera.cullingMask ^= 1 << 9;
            //}
        }

        if (MenuCamera.enabled)
            TeacherGUI.transform.position = new Vector3(100, 0, 0);
    }

    public static void GoBack()
    {
        ThisInstance.CurrentCamera.enabled = false;
        ThisInstance.CurrentCamera = ThisInstance.LastCamera;
        ThisInstance.CurrentCamera.enabled = true;
    }

    public static void GoToMenu()
    {
        ThisInstance.LastCamera = ThisInstance.CurrentCamera;
        ThisInstance.CurrentCamera.enabled = false;
        ThisInstance.CurrentCamera = ThisInstance.MenuCamera;
        ThisInstance.CurrentCamera.enabled = true;
    }
}
