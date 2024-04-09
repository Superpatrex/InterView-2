using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerCamera : MonoBehaviour
{
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas.worldCamera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
