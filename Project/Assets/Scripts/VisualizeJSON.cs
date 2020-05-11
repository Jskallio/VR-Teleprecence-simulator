using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class VisualizeJSON : MonoBehaviour
{
    //Tracking visualization

    public static VisualizeJSON Instance;

    private void Awake()
    {
        Instance = this;
    }
    public Camera camera;

    //Tracking points
    public TrackingContainer trackingPoints = new TrackingContainer();

    //Line renderers for each section
    public List<LineRenderer> lines = new List<LineRenderer>();

    //Collision points
    public List<GameObject> collisions = new List<GameObject>();
    //Prefab for line
    public GameObject linePrefab;
    //Colors for joystick and balance board sections
    public Color JoystickColor, WiiColor;
    //Wanted file
    public string file;
    //lineWidht based on zoom
    public AnimationCurve lineWidth;
    public float lastSize;

    public GameObject collisionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        lines = new List<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //if camera has zoomed then change line widths
        if(lastSize!=camera.orthographicSize)
        {
            //Calculate wanted line width
            float w = lineWidth.Evaluate(camera.orthographicSize);
            lastSize = camera.orthographicSize;

            //Loop through lines and change widths
            if(lines.Count>0)
            {
                foreach (LineRenderer l in lines)
                {
                    l.widthMultiplier = w;
                }
            }
            
            
        }
    }

    public void clearLines()
    {
        //Destroys all lines and collision points
        foreach(LineRenderer r in lines)
        {
            Destroy(r.gameObject);
        }
        foreach(GameObject g in collisions)
        {
            Destroy(g);
        }

        //Clear lists
        collisions = new List<GameObject>();
        lines = new List<LineRenderer>();
    }

    public void loadTracking(string filename)
    {
        //Simple function for loading tracking data
        using (StreamReader sr = new StreamReader(filename))
        {
            trackingPoints = JsonUtility.FromJson<TrackingContainer>(sr.ReadToEnd());
        }
    }

    public void drawLines()
    {
        //Draw lines from current tracking file
        GameObject currentLine = Instantiate(linePrefab);
        LineRenderer line = currentLine.GetComponent<LineRenderer>();

        //Add starting line
        lines.Add(line);
        line.startColor = JoystickColor;
        line.endColor = JoystickColor;
        List<Vector3> positions = new List<Vector3>();
        
        //Loop through tracking events
        foreach(TrackingEvent e in trackingPoints.points)
        {
            switch(e.type)
            {
                case TrackingEventType.Start: //From start we only need position
                    positions.Add(e.pos.toVector());
                    break;
                case TrackingEventType.Change://When changing we create new line object and select color according to new control method
                    positions.Add(e.pos.toVector());

                    //Set positions to the line Renderer
                    line.positionCount = positions.Count;
                    line.SetPositions(positions.ToArray());
                    
                    currentLine = Instantiate(linePrefab);
                    line = currentLine.GetComponent<LineRenderer>();
                    lines.Add(line);
                    if(e.data=="JOY")
                    {
                        line.startColor = JoystickColor;
                        line.endColor = JoystickColor;
                    }
                    else
                    {
                        line.startColor = WiiColor;
                        line.endColor = WiiColor;
                    }
                    positions = new List<Vector3>();
                    positions.Add(e.pos.toVector());

                    break;
                case TrackingEventType.Crash://When crashing we create collision object on the position
                    GameObject o = Instantiate(collisionPrefab, e.pos.toVector(), Quaternion.identity);
                    collisions.Add(o);
                    break;
                case TrackingEventType.End: //In the end add last point to the line and end the line
                    positions.Add(e.pos.toVector());
                    Debug.Log("END");
                    Debug.Log(positions.Count);
                    line.positionCount = positions.Count;
                    line.SetPositions(positions.ToArray());
                    return;
                case TrackingEventType.Position: // On position events add new point
                    positions.Add(e.pos.toVector());
                    break;

            }
        }
    }
}
