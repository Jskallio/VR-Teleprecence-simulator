using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDottedLine : MonoBehaviour
{
    //This script creates dotted line from the path we have given to it
    public List<Vector3> points = new List<Vector3>();
    public float step;
    public Transform target;

    
    // Start is called before the first frame update
    void Start()
    {
        createLine();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createLine()
    {
        //Creates the mesh from the path
        Mesh mesh = new Mesh();

        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;

        //Vertox positions and normals. Normals are used to determine the direction of dots
        List<Vector3> vertexArray = new List<Vector3>();
        List<Vector3> normalArray = new List<Vector3>();
        Vector3 start = points[0];
        Vector3 end = Vector3.zero;
        //Loop through path and create vertices on the way
        for(int i=1; i< points.Count; i++)
        {
            
            end = points[i];
            float dist = (end - start).magnitude;
            Vector3 part = (end - start)/dist;
            if (dist > step)
            {
                for (float f = 0; f < dist; f += step)
                {
                    vertexArray.Add(start + part * f);
                    Debug.Log(start + part * f);
                    normalArray.Add(part);
                }
            }
            start = end;
        }

        //Set vertices to mesh and get their indices correct
        mesh.SetVertices(vertexArray);
        int[] indices = new int[vertexArray.Count];
        for(int i=0; i<indices.Length; i++)
        {
            indices[i] = i;
        }
        //Add normal and indices
        mesh.SetNormals(normalArray);
        mesh.SetIndices(indices, MeshTopology.Points, 0);

        //We create uv list even when we don't use uv
        Vector2[] uv = new Vector2[mesh.vertices.Length];
        mesh.uv = uv;


    }
}
