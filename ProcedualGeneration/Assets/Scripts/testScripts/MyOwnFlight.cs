using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyOwnFlight : MonoBehaviour
{
    public GameObject viewer;
    float throttle;
    float directionRelativeToAxis;
    Quaternion axisOfRotation;
    int rotationConst = 1;

    float degrees;

    int upDown;
    int upDownConst = 1;
    int leftRight;
    int leftRightConst = 1;

    int lowerBound = -100;
    int upperBound = 100;
    
    [SerializeField]
    int turn;
    
    // Start is called before the first frame update
    void Start()
    {
        viewer.transform.position = new Vector3(0,20,0);
        throttle = 1;
        //degrees = 0;
        turn = 0;
        upDown = 0;
        leftRight = 0;
    }
    // Update is called once per frame
    void Update()
    {
        viewer.transform.position += transform.TransformDirection (Vector3.forward * throttle * 0.1f);
        viewer.transform.Rotate(new Vector3((float)upDown/100f,(float)leftRight/100f,(float)turn/100f), Space.Self);


        if(Input.GetKey("w"))
        {
            throttle = Mathf.Clamp01(throttle + 0.01f);
        }
        if(Input.GetKey("s"))
        {
            throttle = Mathf.Clamp01(throttle - 0.01f);
        }
        

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            turn = Mathf.Clamp(turn + rotationConst, lowerBound, upperBound);
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            turn = Mathf.Clamp(turn - rotationConst, lowerBound, upperBound);
        }
        if(turn > 0 && !(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            turn = Mathf.Clamp(turn - rotationConst, lowerBound, upperBound);
        }
        if(turn < 0 && !(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            turn = Mathf.Clamp(turn + rotationConst, lowerBound, upperBound);
        }



        if(Input.GetKey(KeyCode.UpArrow))
        {
            upDown = Mathf.Clamp(upDown + upDownConst, lowerBound, upperBound);
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            upDown = Mathf.Clamp(upDown - upDownConst, lowerBound, upperBound);
        }
        if(upDown > 0 && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
        {
            upDown = Mathf.Clamp(upDown - upDownConst, lowerBound, upperBound);
        }
        if(upDown < 0 && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
        {
            upDown = Mathf.Clamp(upDown + upDownConst, lowerBound, upperBound);
        }

        if(Input.GetKey("d"))
        {
            leftRight = Mathf.Clamp(leftRight + leftRightConst, lowerBound, upperBound);
        }
        if(Input.GetKey("a"))
        {
            leftRight = Mathf.Clamp(leftRight - leftRightConst, lowerBound, upperBound);
        }
        if(leftRight > 0 && !(Input.GetKey("a") || Input.GetKey("d")))
        {
            leftRight = Mathf.Clamp(leftRight - leftRightConst, lowerBound, upperBound);
        }
        if(leftRight < 0 && !(Input.GetKey("a") || Input.GetKey("d")))
        {
            leftRight = Mathf.Clamp(leftRight + leftRightConst, lowerBound, upperBound);
        }
    }
}
