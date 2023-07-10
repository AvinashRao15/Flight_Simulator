using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // Start is called before the first frame update

    //get value of mouse movement then see the change in movement and based on that move the camera
    //linear
    //then move camera 

    float lastXPosition;
    float lastYPosition;

    [SerializeField]
    float deltaX;
    [SerializeField]
    float deltaY;

    [SerializeField]
    float xTotal;
    [SerializeField]
    float yTotal;

    Quaternion rotation;

    PlayerControls controller;
    Vector2 move;

    void Awake()
    {
        rotation = transform.rotation;

        controller = new PlayerControls();

        controller.GamePlay.camera.performed += ctx => move = ctx.ReadValue<Vector2>();
        controller.GamePlay.camera.canceled += ctx => move = Vector2.zero;
    }

    void Start()
    {
        lastXPosition = UnityEngine.Input.GetAxis("Mouse X");
        lastYPosition = UnityEngine.Input.GetAxis("Mouse Y");
        xTotal = 0;
        yTotal = 0;
    }

    void Update()
    {
        if(UnityEngine.Input.GetMouseButton(0) || UnityEngine.Input.GetMouseButton(1))
        {
            deltaX = UnityEngine.Input.GetAxis("Mouse X") - lastXPosition;
            deltaY = UnityEngine.Input.GetAxis("Mouse Y") - lastYPosition;


            lastXPosition = UnityEngine.Input.GetAxis("Mouse X");
            lastYPosition = UnityEngine.Input.GetAxis("Mouse Y");

            deltaY *= -1;

            xTotal += deltaX;
            yTotal += deltaY;


            transform.Rotate(Vector3.up, xTotal, Space.World);
            transform.Rotate(Vector3.right, yTotal);
        }
        if(UnityEngine.Input.GetMouseButton(2) || controller.GamePlay.resetCam.IsPressed())
        {
            transform.localRotation = rotation; 
        }
        
        transform.Rotate(Vector3.up, move.x * -1, Space.World);
        transform.Rotate(Vector3.right, move.y * 1);

        
    }

    void OnEnable()
    {
        controller.GamePlay.Enable();
    }
    void OnDisable()
    {
        controller.GamePlay.Disable();
    }

    
}
