using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkJarObject : GrabInteractable
{
    public bool milkFilled = true;
    public GameObject fairy;
    public static MilkJarObject instance;
    public override void Start()
    {
        instance = this;
        base.Start();
    }
    public override bool GetLastInteraction()
    {
        return !gameObject.name.Contains("Fairy") && base.GetLastInteraction();
    }
    public override void OnGrabInteract()
    {
        base.OnGrabInteract();
        if(milkFilled)
        {
            milkFilled = false;
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else if(PlayerController.instance.GetRaycast() == fairy)
        {

        }
    }
}
