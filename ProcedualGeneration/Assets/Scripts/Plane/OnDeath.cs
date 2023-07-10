using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnDeath : MonoBehaviour
{
    public GameObject Camera;
    public Image img;
    void OnTriggerEnter(Collider other)
    {
        Camera.transform.parent = null;
        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        img.gameObject.SetActive(true);
        StartCoroutine(FadeImage());
    }
 
    IEnumerator FadeImage()
    {
       
        
        // loop over 1 second backwards
        for (float i = 0; i <= 4; i += Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }
    
}
