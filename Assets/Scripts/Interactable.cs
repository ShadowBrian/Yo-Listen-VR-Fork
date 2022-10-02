using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Interactable : MonoBehaviour
{

    public UnityEvent mEvent;
    public bool interactable = true;
    public virtual void OnInteract()
    {
        mEvent.Invoke();
    }

    public virtual bool CheckIfInteractable(){
        return interactable;
    }
}
