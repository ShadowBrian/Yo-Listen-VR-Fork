using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalInteractable : ResetInteractable
{
    public string CheckName = "";
    public override bool CheckIfInteractable()
    {
        if(CheckName.Equals("")) return base.CheckIfInteractable();
        return PlayerController.instance.grabbedObject && PlayerController.instance.grabbedObject.name.Contains(CheckName) && base.CheckIfInteractable();
    }
}
