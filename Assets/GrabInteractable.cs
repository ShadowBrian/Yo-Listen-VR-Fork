using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrabInteractable : Interactable
{
    bool grabbed = false;

    public override void OnInteract()
    {
        PlayerController.instance.GrabObject(this);
        grabbed = true;
    }

    private void Update() {
        if(grabbed)
        {
            if(Input.GetMouseButtonDown(1))
            {
                grabbed = false;
                PlayerController.instance.ReleaseObject(this);
            }
        }
    }
}
