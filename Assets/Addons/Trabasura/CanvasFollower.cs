using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFollower : MonoBehaviour
{
    public Transform playerhead;
    public static CanvasFollower instance;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        if(RendererController.instance)   
        {
            //GetComponent<Canvas>().worldCamera=RendererController.instance.transform.GetChild(0).GetComponent<Camera>();
        }
    }

    public void Update()
    {
        if(playerhead != null)
        {
            transform.position = playerhead.position + playerhead.forward * 1.243f - playerhead.up * 0.742f;
            transform.rotation = playerhead.rotation;
            if (!playerhead.gameObject.activeInHierarchy)
            {
                Destroy(playerhead.root.gameObject);
            }
        }
        else
        {
            playerhead = FindObjectOfType<MouseLook>().transform.GetChild(0);
        }
    }
}
