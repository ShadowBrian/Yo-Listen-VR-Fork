using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFollower : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(RendererController.instance)   
        {
            GetComponent<Canvas>().worldCamera=RendererController.instance.transform.GetChild(0).GetComponent<Camera>();
        }
    }

}
