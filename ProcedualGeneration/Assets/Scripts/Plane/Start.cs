using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Start : MonoBehaviour
{
    public Image img;
    // Start is called before the first frame update
    void Awake()
    {
        img.gameObject.SetActive(false);
        img.color = new Color(1,1,1,0);
    }
//     void FixedUpdate()
//     {
//         Debug.Log(img.color);
//     }
}
