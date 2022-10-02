using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class GrabInteractable : ResetInteractable
{
    public bool grabbed {get; private set;}

    public UnityEvent mOnGrabInteract;
    public override void OnInteract()
    {
        if(!grabbed)
        {
            PlayerController.instance.GrabObject(this);
            grabbed = true;
        }
        base.OnInteract();
    }

    public virtual void Release()
    {
        grabbed = false;
        PlayerController.instance.ReleaseObject(this);
    }
    public virtual void OnGrabInteract()
    {
        mOnGrabInteract.Invoke();
    }

    public virtual void Update() {
        if(grabbed)
        {
            if(Input.GetMouseButtonDown(1))
            {
                Release();
            }
            if(Input.GetMouseButtonDown(0))
            {
                OnGrabInteract();
            }
        }
    }
}
