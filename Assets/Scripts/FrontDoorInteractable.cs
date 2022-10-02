using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDoorInteractable : ResetInteractable
{
    public override void OnInteract()
    {
        base.OnInteract();
        FairyController.instance.DoDoorFail();
    }
}
