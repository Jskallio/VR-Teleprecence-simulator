using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Tracking : MonoBehaviour
{
    // this script tracks position of the robot
    public static Tracking Instance;
    // all tracking points
    public TrackingContainer trackingPoints = new TrackingContainer();
    
    
    //How many frames we wait for new position tracking event
    public int framesForTrack;
    private int frame;
    //File where we save the tracking
    public string file;

    public bool wii = false;

    public bool LetChange = false;


    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Get filename from Main.cs
        file = Main.Instance.getFileName();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void change()
    {
        //Track controller change
        TrackingEvent change = new TrackingEvent(TrackingEventType.Change, transform.position, transform.rotation, Time.time);
        wii = !wii;
        change.data = wii ? "WII" : "JOY";

        addTrackingEvent(change);
    }

    public void startTracking()
    {
        //Start tracking with "Start"-event
        addTrackingEvent(new TrackingEvent(TrackingEventType.Start, transform.position, transform.rotation, Time.time));
        TrackingEvent change = new TrackingEvent(TrackingEventType.Change, transform.position, transform.rotation, Time.time);
        change.data = wii ? "WII" : "JOY";

        addTrackingEvent(change);
    }

    public void endTracking()
    {
        //Save tracking to file
        saveTracking(file, true);
        
    }

    private void FixedUpdate()
    {
        //After enough frames track down position and rotation
        frame = (frame + 1) % framesForTrack;
        
        if(frame==framesForTrack-1)
        {
            addTrackingEvent(new TrackingEvent(TrackingEventType.Position, transform.position,transform.rotation,Time.time));
        }
    }

    public void addTrackingEvent(TrackingEvent e)
    {
        //Adds wanted event to trackingPoints
        trackingPoints.points.Add(e);
    }

    public void addTrackingEvent(TrackingEventType type, string data)
    {
        //Adds event with wanted type and data
        addTrackingEvent(new TrackingEvent(type, transform.position, transform.rotation, Time.time));
    }

    public void saveTracking(string filename,bool clear=true)
    {
        //Save tracking data to json file
        string data = JsonUtility.ToJson(trackingPoints,true);
        string destination = Application.persistentDataPath + filename;
        Debug.Log("Saving to: " + destination);
        using (StreamWriter writer = new StreamWriter(destination,false))
        {
            writer.Write(data);
        }
        if (clear)
            trackingPoints = new TrackingContainer();
    }
    
    
}

//Tracking event types
public enum TrackingEventType { Start, End, Crash, Change, Position }

//Container for points
[System.Serializable]
public class TrackingContainer
{
    public List<TrackingEvent> points = new List<TrackingEvent>();
}

//Tracking event class
[System.Serializable]
public class TrackingEvent
{
    public TrackingEventType type;
    public float time;
    public Position pos;
    public Rotation rot;
    public string data;

    public TrackingEvent(TrackingEventType type, float time)
    {
        this.type = type;
        this.time = time;
    }

    public TrackingEvent(TrackingEventType type, Vector3 position, Quaternion rotation, float time)
    {
        this.type = type;
        this.time = time;
        pos = new Position(position);
        rot = new Rotation(rotation);

    }
}

//Class for serializable Vector3
[System.Serializable]
public class Position
{
    public float x, y, z;

    public Position()
    {
    }

    public Position(Vector3 pos)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
    }

    public Vector3 toVector()
    {
        return new Vector3(x, y, z);
    }
}

//Class for serializable Quaternion
[System.Serializable]
public class Rotation
{
    public float x, y, z, w;

    public Rotation() { }

    public Rotation(Quaternion pos)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
        w = pos.w;
    }
}