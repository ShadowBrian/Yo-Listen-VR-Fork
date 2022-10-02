using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MouseInteractable : GrabInteractable
{
    public Vector3[] movePosition;
    bool hintReceived = false;
    public override void Start()
    {
        base.Start();
    }

    public Vector3[] keyPositions;

    public override bool CheckIfInteractable()
    {
        return cheeseFound;
    }

    public void Hint()
    {
        if(!hintReceived)
        {
            hintReceived = true;
            StartCoroutine(MouseHint());
        }
    }

    public IEnumerator PathToCheese(GameObject cheese)
    {
        transform.GetComponent<Rigidbody>().DOPause();
        transform.GetComponent<Rigidbody>().DOMove(cheese.transform.position, 0.5f);
        transform.GetComponent<Rigidbody>().DOLookAt(cheese.transform.position, 0.5f);
        yield return new WaitForSeconds(0.5f);
        cheese.SetActive(false);
        if(cheese.GetComponent<GrabInteractable>().grabbed)
        {
            cheese.GetComponent<GrabInteractable>().Release();
            OnInteract();
        }
    }

    public override void Update() {
        CheeseScan();
        base.Update();
    }

    public void KeyGrab()
    {
        Debug.Log("Key Grab");
        StartCoroutine(DoKeyGrab());
    }

    public IEnumerator DoKeyGrab()
    {
        if(grabbed) Release();
        transform.GetComponent<Rigidbody>().DOLookAt(keyPositions[1], 0.25f);
        transform.GetComponent<Rigidbody>().DOLocalPath(keyPositions, 0.5f);
        yield return new WaitForSeconds(0.5f);
        foreach(var col in Physics.OverlapSphere(transform.position, 1.5f))
        {
            if(col.gameObject.name.Contains("Key"))
            {
                col.transform.DOLocalMove(keyPositions[0], 0.5f);
                yield return new WaitForSeconds(0.5f);
                col.GetComponent<GrabInteractable>().OnInteract();
            }
        }
    }

    public bool cheeseFound;

    public void CheeseScan()
    {
        if(cheeseFound || !hintReceived) return;
        foreach(var col in Physics.OverlapSphere(transform.position, 2f))
        {
            if(col.gameObject.name.Contains("Cheese"))
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, col.transform.position - transform.position, out hit))
                {
                    if(hit.transform.gameObject == col.gameObject)
                    {
                        cheeseFound = true;
                        StartCoroutine(PathToCheese(col.gameObject));
                    }
                }
            }
        }
    }


    public IEnumerator MouseHint()
    {
        Debug.Log("MOVE");  
        transform.GetComponent<Rigidbody>().DOLocalPath(movePosition, 1f);
        yield return null;
    }
    
    private void OnDrawGizmosSelected() {
        foreach(var m in movePosition)
        {
            Gizmos.DrawSphere(transform.parent.TransformPoint(m), 0.1f);
        }

        foreach(var m in keyPositions)
        {
            Gizmos.DrawSphere(transform.parent.TransformPoint(m), 0.1f);
        }
    }
}
