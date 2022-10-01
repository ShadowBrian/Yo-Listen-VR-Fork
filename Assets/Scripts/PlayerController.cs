using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    float timeScale = 1;
    bool actionOverride;

    public float range = 2;

    public Transform grabLocation;
    public static PlayerController instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public GrabInteractable grabbedObject = null;
    // Update is called once per frame
    
    void Update()
    {
        float idealScale = InputHeld()?0.05f:1;
        timeScale = Mathf.Lerp(timeScale, idealScale, Time.deltaTime*10/idealScale);
        // float movementMagnitude = Vector3.ClampMagnitude(GetComponent<PlayerMovement>().movementInput, 1).magnitude * 2;
        Time.timeScale = timeScale;
        var interactable = CheckForInteractable();
        if(interactable && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
            interactable.OnInteract();
        }

        if(grabbedObject)
        {
            grabbedObject.GetComponent<Rigidbody>().velocity = (grabLocation.position - grabbedObject.transform.position) * 10;
        }
        
    }

    public void GrabObject(GrabInteractable grab)
    {
        grabbedObject = grab;
        Physics.IgnoreCollision(GetComponent<CharacterController>(), grab.GetComponent<Collider>());
    }

    public void ReleaseObject(GrabInteractable grab)
    {
        grabbedObject = null;
        Physics.IgnoreCollision(GetComponent<CharacterController>(), grab.GetComponent<Collider>());
    }

    public Interactable CheckForInteractable()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.GetChild(0).transform.position, transform.GetChild(0).transform.forward, out hit))
        {
            if(hit.transform.GetComponentInParent<Interactable>() && Vector3.Distance(hit.point, transform.GetChild(0).transform.position) < range)
            {
                return hit.transform.GetComponentInParent<Interactable>();
            }
        }
        return null;
    }

    public bool InputHeld()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
}
