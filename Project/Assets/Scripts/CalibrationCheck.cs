using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationCheck : MonoBehaviour
{
    public FullCalibration calibration;
    public string state;
    public KeyCode code = KeyCode.Space;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(code))
        {
            calibration.pressedButton(state);
        }
    }
}
