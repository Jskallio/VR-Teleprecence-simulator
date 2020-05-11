using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visualizer_FilePanel : MonoBehaviour
{
    //This is simple script that keeps information about specific file in the system
    public string file;
    public bool selected;
    public Toggle toggle;
    public Text fileText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fileText.text = file;
    }
}
