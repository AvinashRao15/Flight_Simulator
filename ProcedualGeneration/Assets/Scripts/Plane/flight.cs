using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class flight : MonoBehaviour
{

    //called atrribute

    //rigidBody

    new Rigidbody rigidbody;

    //velocities
    [Header("Velocities")]
    [SerializeField]
    Vector3 Velocity;

    [SerializeField]
    Vector3 LocalVelocity;

    [SerializeField]
    Vector3 LocalAngularVelocity;
    
    [SerializeField]
    private Vector3 lastVelocity;

    //lift stuff
    [Header("Lift")]
    [SerializeField]
    float flapsLiftPower;

    [SerializeField]
    float flapsAOABias;

    [SerializeField]
    float liftPower;

    [SerializeField]
    AnimationCurve liftAOACurve;

    [SerializeField]
    bool FlapsDeployed;
    

    
    // //aoa
    // [Header("AOA")]
    // [SerializeField]
    // AnimationCurve aoaCurve;

    [SerializeField]
    public float AngleOfAttack { get; private set; }

    [SerializeField]
    public float AngleOfAttackYaw { get; private set; }
    
    
    //drag
    [Header("Drag")]
    [SerializeField]
    float inducedDrag;

    [SerializeField]
    AnimationCurve inducedDragCurve;

    [SerializeField]
    AnimationCurve dragForward;

    [SerializeField]
    AnimationCurve dragBack;

    [SerializeField]
    AnimationCurve dragLeft;

    [SerializeField]
    AnimationCurve dragRight;

    [SerializeField]
    AnimationCurve dragTop;

    [SerializeField]
    AnimationCurve dragBottom;

    [SerializeField]
    Vector3 angularDrag;

    [SerializeField]
    float airbrakeDrag;


    //control variables
    [Header("Control Variables")]
    [SerializeField]
    private float maxThrust;

    [SerializeField]
    public Vector3 LocalGForce { get; private set; }

    [SerializeField]
    float rudderPower;

    [SerializeField]
    AnimationCurve rudderAOACurve;

    [SerializeField]
    AnimationCurve rudderInducedDragCurve;

    //steering
    [Header("Steering")]
    [SerializeField]
    AnimationCurve steeringCurve;

    [SerializeField]
    Vector3 turnSpeed;

    [SerializeField]
    Vector3 turnAcceleration;

    //stats
    

    [Header("Stats")]
    [SerializeField]
    Vector3 thrust;

    [SerializeField]
    Vector3 drag;

    [SerializeField]
    Vector3 liftForce;

    [SerializeField]
    Vector3 yawForce;

    [SerializeField]
    Vector3 steering;

    //control inputs

    [Header("Control inputs")]
    [SerializeField]
    //pitch roll yaw
    Vector3 controlInput;

    [SerializeField]
    private float ThrottleInput;


    [Header("Misc")]
    [SerializeField]
    float startHeight;

    
    public TMP_Text speed;
    public TMP_Text alt;

    [Header("Inputs")]
    public bool autoDetectController;
    public bool controllerOR;  

    PlayerControls controllerInput;  

    Vector2 move;

    float throttle;
    [SerializeField]
    float brake;

    public GameObject Camera;
    public Image img;

    void Awake()
    {
        controllerInput = new PlayerControls();
        
        controllerInput.GamePlay.steer.performed += ctx => move = ctx.ReadValue<Vector2>();
        controllerInput.GamePlay.steer.canceled += ctx => move = Vector2.zero;

        controllerInput.GamePlay.accel.performed += ctx => throttle = ctx.ReadValue<float>();
        controllerInput.GamePlay.accel.canceled += ctx => throttle = 0;

        controllerInput.GamePlay.deccel.performed += ctx => brake = ctx.ReadValue<float>() * airbrakeDrag;
        controllerInput.GamePlay.deccel.canceled += ctx => brake = 0;

    }

    void OnEnable()
    {
        controllerInput.GamePlay.Enable();
    }
    void OnDisable()
    {
        controllerInput.GamePlay.Disable();
    }
    
    void Start()
    {
        Transform transform = GetComponent<Transform>();
        transform.position = new Vector3(0,startHeight,0);
        //transform.Rotate(new Vector3(90,0,0), Space.Self);

        rigidbody = GetComponent<Rigidbody>();
        FlapsDeployed = false;
        
        
    }

    // Update is called once per frame
    
    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        CalculateState(dt);
        CalculateAngleOfAttack();
        CalculateGForce(dt);
        
        if(autoDetectController)
        {
            if(Gamepad.all.Count < 1)
            {
                controllerOR = false;
            }
            else
            {
                controllerOR = true;
            }
        }
        

        if(controllerOR == false)
        {
            Keyboard();
        }
        if(controllerOR == true)
        {
            controller();
        }

        UpdateThrust();
        UpdateLift();
        UpdateSteering(dt);
        UpdateDrag();

        


        speed.text = "Speed: " + ((int)LocalVelocity.z).ToString();
        alt.text = "Alt: " + ((int)transform.position.y).ToString();

    }
   
    void controller()
    {
        if(controllerInput.GamePlay.leftRudder.IsPressed())
        {
            controlInput.y = Mathf.Max(controlInput.y - 0.01f, -1);
        }
        else
        {
            if(controlInput.y < -0.01f)
            {
                controlInput.y += 0.01f;
            }
            else if (controlInput.y < 0)
            {
                controlInput.y = 0;
            }
        }

        if(controllerInput.GamePlay.rightRudder.IsPressed())
        {
            controlInput.y = Mathf.Min(controlInput.y + 0.01f, 1);
        }
        else
        {
            if(controlInput.y > 0.01f)
            {
                controlInput.y -= 0.01f;
            }
            else if (controlInput.y > 0)
            {
                controlInput.y = 0;
            }
        }

        ThrottleInput = throttle;

        controlInput.x = move.y * move.y * move.y;
        controlInput.z = -1 * move.x * move.x * move.x;

        if(controllerInput.GamePlay.B.IsPressed())
        {
            rigidbody.constraints = RigidbodyConstraints.None;
            Camera.transform.parent = transform;
        
            transform.position = new Vector3(transform.position.x,startHeight,transform.position.z);
            transform.rotation = new Quaternion(0,0,0,0);
        
            img.gameObject.SetActive(false);
            img.color = new Color(1,1,1,0);
        }
    }

    

    void Keyboard()
    {
        if(Input.GetKey(KeyCode.W))
        {
            ThrottleInput = Mathf.Min(ThrottleInput+0.01f, 1);
        }
        if(Input.GetKey(KeyCode.S))
        {
            ThrottleInput = Mathf.Max(ThrottleInput-0.01f, -1);
        }

        



        if(Input.GetKey(KeyCode.Q))
        {
            controlInput.y = Mathf.Max(controlInput.y - 0.01f, -1);
        }
        else
        {
            if(controlInput.y < -0.01f)
            {
                controlInput.y += 0.01f;
            }
            else if (controlInput.y < 0)
            {
                controlInput.y = 0;
            }
        }

        if(Input.GetKey(KeyCode.E))
        {
            controlInput.y = Mathf.Min(controlInput.y + 0.01f, 1);
        }
        else
        {
            if(controlInput.y > 0.01f)
            {
                controlInput.y -= 0.01f;
            }
            else if (controlInput.y > 0)
            {
                controlInput.y = 0;
            }
        }

        


        if(Input.GetKey(KeyCode.DownArrow))
        {
            controlInput.x = Mathf.Max(controlInput.x - 0.01f, -1);
        }
        else
        {
            if(controlInput.x < -0.01f)
            {
                controlInput.x += 0.01f;
            }
            else if (controlInput.x < 0)
            {
                controlInput.x = 0;
            }
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            controlInput.x = Mathf.Min(controlInput.x + 0.01f, 1);
        }
        else
        {
            if(controlInput.x > 0.01f)
            {
                controlInput.x -= 0.01f;
            }
            else if (controlInput.x > 0)
            {
                controlInput.x = 0;
            }
        }

        if(Input.GetKey(KeyCode.RightArrow))
        {
            controlInput.z = Mathf.Max(controlInput.z - 0.01f, -1);
        }
        else
        {
            if(controlInput.z < -0.01f)
            {
                controlInput.z += 0.01f;
            }
            else if (controlInput.z < 0)
            {
                controlInput.z = 0;
            }
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            controlInput.z = Mathf.Min(controlInput.z + 0.01f, 1);
        }
        else
        {
            if(controlInput.z > 0.01f)
            {
                controlInput.z -= 0.01f;
            }
            else if (controlInput.z > 0)
            {
                controlInput.z = 0;
            }
        }
    }

    private void CalculateState(float dt)
    {
        var invRotation = Quaternion.Inverse(rigidbody.rotation);
        Velocity = rigidbody.velocity;
        LocalVelocity = invRotation * Velocity;
        LocalAngularVelocity = invRotation * rigidbody.angularVelocity;
        
    }


    private void CalculateAngleOfAttack()
    {
        if(LocalVelocity.sqrMagnitude < 0.1f)
        {
            AngleOfAttack = 0;
            AngleOfAttackYaw = 0;
            return;
        }

        AngleOfAttack = Mathf.Atan2(-LocalVelocity.y,LocalVelocity.z);
        AngleOfAttackYaw = Mathf.Atan2(LocalVelocity.x, LocalVelocity.z);
        
    }

    private void CalculateGForce(float dt)
    {
        var invRotation = Quaternion.Inverse(rigidbody.rotation);
        var acceleration = (Velocity - lastVelocity) /dt;
        LocalGForce = invRotation * acceleration;
        lastVelocity = Velocity;
    }

    void UpdateThrust()
    {
        //print("Thrust: " + (ThrottleInput * maxThrust * Vector3.forward));
        thrust = ThrottleInput * maxThrust * Vector3.forward;
        rigidbody.AddRelativeForce(thrust);
    }

    void UpdateDrag()
    {
        var lv = LocalVelocity;
        var lv2 = lv.sqrMagnitude;

        float airbrakeDrag = brake;
        //float flapsDrag = 0;

        var coefficient = Scale6(
            lv.normalized,
             dragRight.Evaluate(Mathf.Abs(lv.x)), dragLeft.Evaluate(Mathf.Abs(lv.x)),
         dragTop.Evaluate(Mathf.Abs(lv.y)), dragBottom.Evaluate(Mathf.Abs(lv.y)),
         dragForward.Evaluate(Mathf.Abs(lv.z)) + airbrakeDrag //+ flapsDrag
         , dragBack.Evaluate(Mathf.Abs(lv.z)));

        drag = coefficient.magnitude * lv2 * -lv.normalized;
        // print("coefficientMagnitude: " + coefficient.magnitude);
        // print("localVelocity" + lv);
        //print("drag: " + drag);
        rigidbody.AddRelativeForce(drag);
    }

    Vector3 CalculateLift(float angleOfAttack, Vector3 rightAxis, float liftPower, AnimationCurve aoaCurve)
    {
        var liftVelocity = Vector3.ProjectOnPlane(LocalVelocity, rightAxis);    //project velocity onto plane
        var v2 = liftVelocity.sqrMagnitude;                                     //square of velocity

        //lift = velocity^2 * coefficient * liftPower
        //coefficient varies with AOA
        var liftCoefficient = aoaCurve.Evaluate(angleOfAttack * Mathf.Rad2Deg);
        var liftForce = v2 * liftCoefficient * liftPower;

        //lift is perpendicular to velocity
        var liftDirection = Vector3.Cross(liftVelocity.normalized, rightAxis);
        var lift = liftDirection * liftForce;

        //induced drag varies with square of lift coefficient
        var dragForce = liftCoefficient * liftCoefficient;
        var dragDirection = -liftVelocity.normalized;
        var inducedDrag = dragDirection * v2 * dragForce * this.inducedDrag * inducedDragCurve.Evaluate(Mathf.Max(0, LocalVelocity.z));

        return lift + inducedDrag;
    }

    void UpdateLift() {
        if (LocalVelocity.sqrMagnitude < 1f) return;

        float flapsLiftPower = FlapsDeployed ? this.flapsLiftPower : 0;
        float flapsAOABias = FlapsDeployed ? this.flapsAOABias : 0;

        liftForce = CalculateLift(
            AngleOfAttack + (flapsAOABias * Mathf.Deg2Rad), Vector3.right,
            liftPower + flapsLiftPower,
            liftAOACurve
        );

        yawForce = CalculateLift(AngleOfAttackYaw, Vector3.up, rudderPower, rudderAOACurve);

        //liftforce is zero
         //print("lift: " + liftForce);
         //print("yawForce: " + yawForce);
        
        rigidbody.AddRelativeForce(liftForce);

        rigidbody.AddRelativeForce(yawForce);
    }

    float CalculateSteering(float dt, float angularVelocity, float targetVelocity, float acceleration)
    {
        var error = targetVelocity - angularVelocity;
        var accel = acceleration * dt;
        return Mathf.Clamp(error, -accel, accel);
    }
    void UpdateSteering(float dt)
    {
        var speed = Mathf.Max(0, LocalVelocity.z);
        var steeringPower = steeringCurve.Evaluate(speed);


        var targetAV = Vector3.Scale(controlInput, turnSpeed * steeringPower);
        var av = LocalAngularVelocity * Mathf.Rad2Deg;

         var correction = new Vector3(
            CalculateSteering(dt, av.x, targetAV.x, turnAcceleration.x * steeringPower),
            CalculateSteering(dt, av.y, targetAV.y, turnAcceleration.y * steeringPower),
            CalculateSteering(dt, av.z, targetAV.z, turnAcceleration.z * steeringPower)
        );
         //print("steering: " + (correction * Mathf.Deg2Rad, ForceMode.VelocityChange));
         steering = correction * Mathf.Deg2Rad;

        rigidbody.AddRelativeTorque(steering, ForceMode.VelocityChange);
    }

    public static Vector3 Scale6(
        Vector3 value,
        float posX, float negX, 
        float posY, float negY,
        float posZ, float negZ
    )
    {
        Vector3 result = value;

        if(result.x > 0)
        {
            result.x *= posX;

        }
        else if (result.x < 0)
        {
            result.x *= negX;
        }

        if(result.y > 0)
        {
            result.y *= posY;

        }
        else if (result.y < 0)
        {
            result.y *= negY;
        }

        if(result.z > 0)
        {
            result.z *= posZ;

        }
        else if (result.z < 0)
        {
            result.z *= negZ;
        }
        
        return result;
    }
}
