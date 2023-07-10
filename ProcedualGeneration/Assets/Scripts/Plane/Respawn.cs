using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Respawn : MonoBehaviour
{
    public GameObject plane;
    public GameObject Camera;
    public int startHeight;
    public Image img;

    
    public void NewGameButton()
    {
        plane.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Camera.transform.parent = plane.transform;
        
        plane.transform.position = new Vector3(plane.transform.position.x,startHeight,plane.transform.position.z);
        plane.transform.rotation = new Quaternion(0,0,0,0);
        
        img.gameObject.SetActive(false);
        img.color = new Color(1,1,1,0);
    }
}
