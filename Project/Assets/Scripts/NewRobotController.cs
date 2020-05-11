using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;
public enum ControllerType { Joystick, Wiiboard };
public class NewRobotController : MonoBehaviour
{
    //This function is for robot controlls
    public static NewRobotController Instance;

    public void Awake()
    {
        Instance = this;
    }
    public float leftTarget, rightTarget;
    public float leftSpeed, rightSpeed;
    //How much either sides speed is allowed to change
    public float delta;

    //Maximum linear and angular speed
    public float maxSpeed, maxAngularSpeed;
    //linear and angular acceleration
    public float acceleration = 1f;
    public float angularAcceleration = 0.1f;

    //Current linear and angular speed
    public float speed;
    public float angularSpeed;

    
    //Current control type
    public ControllerType controllerType;

    //Are we allowed to switch in this scene
    public bool letSwitch = false;

    //Steamvr information about pinch and a button from any controller
    public SteamVR_Action_Boolean grabPinch;
    public SteamVR_Action_Boolean aButton;
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;

    //Time variables for holding down the A button
    float startedHolding;
    public float holdToStart = 3f;
    bool hasStarted;
    bool holding;

    //Where to teleport when run starts
    public Transform StartPoint;

    //UI objects
    public GameObject holdIndicator;
    public Text holdText;

    public RectTransform speedNeedle;
    public Vector2 needleLimits;

    

    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        //Set controls based on current type from Main.cs
        switch(Main.Instance.currentState)
        {
            case Main.State.FirstRun:
                if (Main.Instance.joystickFirst)
                    controllerType = ControllerType.Joystick;
                else
                    controllerType = ControllerType.Wiiboard;
                break;
            case Main.State.SecondRun:
                if (Main.Instance.joystickFirst)
                    controllerType = ControllerType.Wiiboard;
                else
                    controllerType = ControllerType.Joystick;
                break;
            default:
                controllerType = ControllerType.Joystick;
                break;
        }
        rb = GetComponent<Rigidbody>();
        //Add listener for pinch and A button
        if(grabPinch!=null)
        {
            grabPinch.AddOnChangeListener(TriggerPressedOrReleased, inputSource);
        }
        if(aButton!=null)
        {
            aButton.AddOnChangeListener(startRun, inputSource);
        }
        //Let correct scripts to send control data
        if(controllerType == ControllerType.Joystick)
        {
            JoystickController.Instance.allowSending = true;
            Tracking.Instance.wii = false;
            WiiController.Instance.allowSending = false;
        }
        else
        {
            Tracking.Instance.wii = true;
            JoystickController.Instance.allowSending = false;
            WiiController.Instance.allowSending = true;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        //If a button is pressed
        if (aButton.GetState(inputSource))
        {
            if (!hasStarted) //Start timer for holding down the button
            {
                holdIndicator.SetActive(true);
                float timeLeft = holdToStart - (Time.time - startedHolding);
                holdText.text = string.Format("Hold to start the run:\n<b>{0:0.0}</b>", timeLeft);
            }
            if (Time.time > startedHolding + holdToStart && !hasStarted) //If enough time has passed then teleport robot
            {
                hasStarted = true;
                transform.position = StartPoint.position;
                transform.rotation = StartPoint.rotation;
            }
        }
        else
            holdIndicator.SetActive(false);
    }

    private void FixedUpdate()
    {
        //Calculate amount how much both sides has to move
        float l = leftTarget - leftSpeed;
        float r = rightTarget - rightSpeed;
        //Limit acceleration to max change (delta). This is done to make it fair for balance board
        leftSpeed += Mathf.Sign(l) * Mathf.Min(Mathf.Abs(l), delta);
        rightSpeed += Mathf.Sign(r) * Mathf.Min(Mathf.Abs(r), delta);

        //Get average of both sides
        float avg = leftSpeed + rightSpeed;
        avg /= 2;

        //Target speed is calculated from average speed
        float v = (maxSpeed*avg - speed);

        speed += Mathf.Sign(v) * Mathf.Min(Mathf.Abs(v), acceleration);
        //Move robot according to speed
        rb.MovePosition(transform.position + transform.forward * speed);
        //Calculate angle for speed needle
        float angle = needleLimits.x + (Mathf.Abs(speed) / maxSpeed) * (needleLimits.y - needleLimits.x);
        Quaternion ang = Quaternion.Euler(0, 0, -angle);
        speedNeedle.localRotation = ang;

        //Calculate difference between the sides for rotation
        float diff = -(rightSpeed - leftSpeed)/2;
        //Get square of the difference to eliminate small jittering when difference is small
        diff = Mathf.Sign(diff) * (diff * diff);

        //Calculate angular speed
        float a = (maxAngularSpeed * diff - angularSpeed);
        angularSpeed += Mathf.Sign(a) * Mathf.Min(Mathf.Abs(a), angularAcceleration);
        //Rotate the robot
        rb.MoveRotation(transform.rotation * Quaternion.AngleAxis(angularSpeed, transform.up));


    }

    private void startRun(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool Active)
    {
        //Pressing down A starts timer for starting the run
        if(Active)
        {
            startedHolding = Time.time;
        }
        else
        {
            
        }
    }

    private void TriggerPressedOrReleased(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool Active)
    {
        //If pressed trigger then change control type if it's allowed
        if (!letSwitch && !Active)
        {
            Tracking.Instance.change();
            if (controllerType == ControllerType.Joystick)
            {
                controllerType = ControllerType.Wiiboard;
                WiiController.Instance.allowSending = true;
                JoystickController.Instance.allowSending = false;
            }
            else
            {
                WiiController.Instance.allowSending = false;
                JoystickController.Instance.allowSending = true;
                controllerType = ControllerType.Joystick;
            }
                
        }
    }

    
}
