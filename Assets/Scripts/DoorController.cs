using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DoorController : ConditionalInteractable
{
    public Transform overrideHinge;
    public Vector3 openPos;
    public bool open;

    public override void Start()
    {
        base.Start();
        if(overrideHinge==null) overrideHinge = transform.GetChild(0);
    }

    public void CloseDoor(){
        overrideHinge.DOLocalRotate(new Vector3(0, 0, 0), 0.25f);
    }

    public void OpenDoor()
    {
        overrideHinge.DOLocalRotate(openPos, 0.25f);
    }

    public override void OnInteract()
    {
        if(open)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
        open = !open;
        base.OnInteract();
    }
}
