using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterviewPassthroughSurface : MonoBehaviour
{
    private OVRPassthroughLayer passthroughLayer;
    public MeshFilter projectionObject;
    MeshRenderer quadOutline;

    private bool showing = false;

    void Start()
    {
        GameObject ovrCameraRig = GameObject.Find("OVRCameraRig");
        if (ovrCameraRig == null)
        {
            Debug.LogError("Scene does not contain an OVRCameraRig");
            return;
        }

        passthroughLayer = ovrCameraRig.GetComponent<OVRPassthroughLayer>();
        if (passthroughLayer == null)
        {
            Debug.LogError("OVRCameraRig does not contain an OVRPassthroughLayer component");
        }

        //passthroughLayer.AddSurfaceGeometry(projectionObject.gameObject, true);

        // The MeshRenderer component renders the quad as a blue outline
        // we only use this when Passthrough isn't visible
        quadOutline = projectionObject.GetComponent<MeshRenderer>();
        quadOutline.enabled = false;
    }

    public void Toggle()
    {
        if (passthroughLayer != null)
        {
            if(showing)
            {
                passthroughLayer.RemoveSurfaceGeometry(projectionObject.gameObject);
                quadOutline.enabled = true;
            }
            else
            {
                passthroughLayer.AddSurfaceGeometry(projectionObject.gameObject, true);
                quadOutline.enabled = false;
            }
            showing = !showing;
        }
    }
}
