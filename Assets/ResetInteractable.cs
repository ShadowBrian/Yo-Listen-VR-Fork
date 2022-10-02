using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class ResetInteractable : Interactable
{
    bool lastInteraction;
    
    public virtual void Start()
    {
        GetComponent<Outline>().enabled = false;    
        GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
        GetComponent<Outline>().OutlineWidth = 4;
    }
    public virtual bool GetLastInteraction()
    {
        bool li = lastInteraction;
        lastInteraction = false;
        GetComponent<Outline>().enabled = true;
         
        GetComponent<Outline>().OutlineColor = (Color.white + Color.green) / 2;
        return li;
    }

    public override bool CheckIfInteractable()
    {
        return base.CheckIfInteractable() && GameManager.instance.startGame;
    }

    public override void OnInteract()
    {
        base.OnInteract();
        lastInteraction = true;
        Debug.Log("interacted");
    }
}
