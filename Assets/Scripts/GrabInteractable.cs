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
            if(UnityXRInputBridge.instance.GetButtonUp(XRButtonMasks.gripButton, XRHandSide.LeftHand))
            {
                Release();
                UnityXRInputBridge.instance.SetHaptics(0.5f, XRHandSide.LeftHand);
            }
            if(UnityXRInputBridge.instance.GetButtonDown(XRButtonMasks.gripButton, XRHandSide.LeftHand))
            {
                OnGrabInteract();
                UnityXRInputBridge.instance.SetHaptics(1f, XRHandSide.LeftHand);
            }
        }
    }
}
