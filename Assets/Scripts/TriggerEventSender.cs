using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventSender : MonoBehaviour
{
    public Collider collider;
    public string CheckName = "";
    public Transform parent;

    public UnityEvent mOnEnter, mOnStay, mOnLeave;
    private void Start() {

    }

    private void Update() {
        if(transform.parent)
        {
                    collider = GetComponent<Collider>();
        parent = transform.parent;
        transform.parent = null;
        }
        if(!parent)
        {
            Destroy(gameObject);    
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(CheckNameCommand(CheckName, other.transform))
        {
            mOnEnter.Invoke();
        }
    }

    public bool CheckNameCommand(string name, Transform t)
    {
        if(t.parent)
        {
            return t.name.Contains(name) || CheckNameCommand(name, t.parent);
        }
        return t.name.Contains(name);
    }

    private void OnTriggerExit(Collider other) {
        if(CheckNameCommand(CheckName, other.transform))
        {
            mOnLeave.Invoke();
        }
    }

    private void OnTriggerStay(Collider other) {
        if(CheckNameCommand(CheckName, other.transform))
        {
            mOnStay.Invoke();
        }
    }
}
