using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnPlay : MonoBehaviour
{
    // Start is called before the first frame update
    public bool inverse;
    void Start()
    {
        if(inverse)
        {
        gameObject.SetActive(true);
        }
        else
        {
        gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
