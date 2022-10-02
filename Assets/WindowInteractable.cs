using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowInteractable : ConditionalInteractable
{
    bool shattered = false;
    public override void OnInteract()
    {
        base.OnInteract();
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
        }
        if(!shattered)
        {
            shattered = true;
            var go = Instantiate(Resources.Load<GameObject>("ShatterSound"));
            go.transform.position = transform.position;
            Destroy(go, 1);
        }
        
    }
}
