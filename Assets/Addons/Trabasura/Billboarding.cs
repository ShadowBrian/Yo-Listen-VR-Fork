using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    Vector3 camDir;
    Vector3 startScale;

    public Transform parent;
    void Start()
    {
        startScale = transform.localScale;
        parent = transform.parent;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(!parent)
        {
            Destroy(gameObject);
            return;
        }

        

        Vector3 dirBetween =  transform.position - Camera.main.transform.position;
        if(dirBetween.magnitude < 1)
        {
            dirBetween = dirBetween.normalized;
            transform.position = Camera.main.transform.position + dirBetween;
        }
        else{
            transform.position = parent.position + Vector3.up;
        }

        camDir = Camera.main.transform.forward;
        camDir.y = 0;

        float distance = Mathf.Clamp(Vector3.Distance(Camera.main.transform.position, transform.position),1,100);
        transform.localScale = startScale * distance;
        transform.rotation = Quaternion.LookRotation(camDir);
    }
}
