using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyInteractable : ConditionalInteractable
{
    public override bool CheckIfInteractable()
    {
        if(base.CheckIfInteractable())
        {
            
            return PlayerController.instance.grabbedObject.GetComponent<MilkJarObject>().milkFilled == false;
        }
        return false;
    }

    public override void OnInteract()
    {
        base.OnInteract();
        PlayerController.instance.grabbedObject.GetComponent<MilkJarObject>().gameObject.name = "FairyMilkJar";
    }
    public override bool GetLastInteraction()
    {
        return false;
    }
}
