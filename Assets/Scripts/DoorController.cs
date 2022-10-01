using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DoorController : Interactable
{

    public bool open;
    


    public void CloseDoor(){
        transform.GetChild(0).DOLocalRotate(new Vector3(0, 0, 0), 0.25f);
    }

    public void OpenDoor()
    {
        transform.GetChild(0).DOLocalRotate(new Vector3(0, -90, 0), 0.25f);
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
    }
}
