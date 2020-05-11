using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class JoystickController : MonoBehaviour
{
    //Basic joystic controls
    public static JoystickController Instance;

    private void Awake()
    {
        Instance = this;
    }

    //Robot controller where to send data
    public NewRobotController robot;


    //Get both hands from steamvr
    public SteamVR_Action_Vector2 leftJoystick, rightJoystick;
    public SteamVR_Input_Sources inputSourceL = SteamVR_Input_Sources.LeftHand;
    public SteamVR_Input_Sources inputSourceR = SteamVR_Input_Sources.RightHand;

    public float maxValue;

    //Only main function can give us permission to send controls
    public bool allowSending;


    // Start is called before the first frame update
    void Start()
    {
    }
    
    public Vector2 limitVector(Vector2 input,float value)
    {
        //Simple clamp function for vector2
        input.y = Mathf.Min(Mathf.Max(-value, input.y), value) * (1/ value);
        input.x = Mathf.Min(Mathf.Max(-value, input.x), value) * (1 / value);
        return input;
    }
    // Update is called once per frame
    void Update()
    {
        float left = 0;
        float right = 0;
        //If there is joysticks available take their position and clamp it   
        if(leftJoystick!= null && rightJoystick!=null)
        {
            Vector2 leftStick = limitVector(leftJoystick.GetAxis(inputSourceL),maxValue);
            Vector2 rightStick = limitVector(rightJoystick.GetAxis(inputSourceR),maxValue);
            
            left = Mathf.Min(1,Mathf.Max(-1, leftStick.y + rightStick.x));
            right = Mathf.Min(1,Mathf.Max(-1, leftStick.y - rightStick.x));
        }
        else
        {
            //If not joysticks present use keyboard controlls. Only for testing
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    right = 0;
                    left = 1f;
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    left = 0;
                    right = 1;
                }
                else
                {
                    left = 1;
                    right = 1;
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    right = 0;
                    left = -1f;
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    left = 0;
                    right = -1;
                }
                else
                {
                    left = -1;
                    right = -1;
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    right = -1;
                    left = 1f;
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    left = -1;
                    right = 1;
                }
                else
                {
                    left = 0;
                    right = 0;
                }
            }
        }
        
        //If allowed to send the give robot the control information
        if(allowSending)
        {
            robot.leftTarget = left;
            robot.rightTarget = right;
        }
        
    }
}
