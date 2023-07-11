using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroll : MonoBehaviour
{
    // Start is called before the first frame update

    public bool f35;

    PlayerControls controller;

    float lowestDst
    {
        get{
            if(f35)
            {
                return 2;
            }
            else
            {
                return 4;
            }
        }
    }

    public float defDst;

    void Awake()
    {
        controller = new PlayerControls();
    }
    void Start()
    {
        
        transform.localPosition = new Vector3(0,0,-defDst);
    }
    void OnEnable()
    {
        controller.GamePlay.Enable();
    }
    void OnDisable()
    {
        controller.GamePlay.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(2) ||  controller.GamePlay.resetCam.IsPressed())
        {
            transform.localPosition = new Vector3(0,0,-defDst);
        }

        if((Input.GetAxis("Mouse ScrollWheel") > 0 || controller.GamePlay.X.IsPressed()) && transform.localPosition.z < -lowestDst)
        {
            transform.localPosition += new Vector3(0,0,0.1f);
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0 || controller.GamePlay.Y.IsPressed())
        {
            transform.localPosition += new Vector3(0,0, -0.1f);
        }
    }
}
