using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FPS_Controller : MonoBehaviour
{
    float v = 1/60f;
    new Transform transform;
    void Start()
    {
        transform = GetComponent<Transform>();
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += (Vector3.forward * v);
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.position += (Vector3.left * v);
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.position += (Vector3.back * v);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.position += (Vector3.right * v);
        }
        if(Input.GetKey(KeyCode.Space))
        {
            transform.position += (Vector3.up * v);
        }
        if(Input.GetKey(KeyCode.LeftControl))
        {
            transform.position += (Vector3.down * v);
        }
    }
}