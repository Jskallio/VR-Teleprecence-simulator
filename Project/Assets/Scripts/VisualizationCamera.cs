using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizationCamera : MonoBehaviour
{
    //This controlls the camera in visualization scene
    Camera camera;
    public Vector3 startPosition;
    public Vector3 transformStartPosition;

    //camera speed is controlled by curve based on zoom intensity
    public AnimationCurve MULT;
    public float testMult;
    public float ZMULT;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))//Pressing down mouse 1 starts the camera movement
        {
            startPosition = Input.mousePosition;
            transformStartPosition = transform.position;
        }
        else if(Input.GetMouseButton(1))//holding down the mouse button moves the camera
        {
            Vector3 tr = (Input.mousePosition - startPosition);
            transform.position = transformStartPosition + new Vector3(-tr.y,0,tr.x) * MULT.Evaluate(camera.orthographicSize);
        }

        //Mouse scroll controls zoom
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        camera.orthographicSize -= zoom*ZMULT;
    }
}
