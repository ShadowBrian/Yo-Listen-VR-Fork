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
    public float bob;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        defaultHeadPos = transform.GetChild(0).localPosition;
    }

    Vector3 defaultHeadPos;

    public GrabInteractable grabbedObject = null;
    // Update is called once per frame

    public Interactable lastInteractable;
    public bool lastInteractOn = false;
    public Color lastInteractColor = Color.white;

    public AudioSource interactAudio, failInteractAudio;

    public IEnumerator DOScreenShake()
    {
        for(float t = 0; t < 0.5f; t+= Time.deltaTime)
        {
            GetComponentInChildren<Camera>().transform.localPosition = Random.insideUnitSphere * 0.25f;
            yield return null;
        }
    }

    public void ScreenShake()
    {
        StartCoroutine(DOScreenShake());
    }
    
    void Update()
    {
        if(GameManager.instance.resetting) return;
        float idealScale = InputHeld()?0.05f:1;
        if(GetComponent<PlayerMovement>().isGrounded && GetComponent<PlayerMovement>().movementInput.magnitude > 0)
        {
            bob += Time.deltaTime * 20;
            transform.GetChild(0).position += Vector3.up * Mathf.Sin(bob) * 0.02f;
        }
        else{
            transform.GetChild(0).localPosition = Vector3.Lerp(transform.GetChild(0).localPosition, defaultHeadPos, Time.deltaTime * 5);
        }
        timeScale = Mathf.Lerp(timeScale, idealScale, Time.deltaTime*10/idealScale);
        // float movementMagnitude = Vector3.ClampMagnitude(GetComponent<PlayerMovement>().movementInput, 1).magnitude * 2;
        Time.timeScale = timeScale;
        var interactable = CheckForInteractable();
        if(interactable != lastInteractable)
        {
            if(lastInteractable != null)
            {
                if(lastInteractable.GetComponentInParent<Outline>())
                {
                    lastInteractable.GetComponentInParent<Outline>().enabled = lastInteractOn;
                    lastInteractable.GetComponentInParent<Outline>().OutlineColor = lastInteractColor;
                }
            }
            lastInteractable = interactable;
            if(interactable)
            {
                if(lastInteractable.GetComponentInParent<Outline>())
                {
                    lastInteractOn = lastInteractable.GetComponentInParent<Outline>().enabled;
                    lastInteractColor = lastInteractable.GetComponentInParent<Outline>().OutlineColor;
                    lastInteractable.GetComponentInParent<Outline>().enabled = true;
                    lastInteractable.GetComponentInParent<Outline>().OutlineColor = (lastInteractable).CheckIfInteractable()?Color.white:Color.red;
                }
            }
        }
        if(interactable && interactable.CheckIfInteractable() && UnityXRInputBridge.instance.GetButtonDown(XRButtonMasks.gripButton, XRHandSide.LeftHand))
        {
            Debug.Log("Click");
            interactAudio.Play();
            interactable.OnInteract();
        }
        else if(interactable && UnityXRInputBridge.instance.GetButtonDown(XRButtonMasks.gripButton, XRHandSide.LeftHand))
        {
            failInteractAudio.Play();
        }

        if(grabbedObject)
        {
            grabbedObject.GetComponent<Rigidbody>().velocity = (grabLocation.position - grabbedObject.transform.position) * 10;
        }
        
    }

    private void OnDestroy() {
        if(lastInteractable != null)
        {
            if(lastInteractable.GetComponent<Outline>())
            {
                lastInteractable.GetComponent<Outline>().enabled = false;
            }
        }
        lastInteractable =null;
    }

    public void GrabObject(GrabInteractable grab)
    {
        if(grabbedObject) grabbedObject.Release();
        grabbedObject = grab;
        ToggleColliders(grab.transform, true);
    }

    public void ToggleColliders(Transform origin, bool toggle)
    {
        for(int i = 0; i < origin.childCount; i++)
        {
            ToggleColliders(origin.GetChild(i), toggle);
        }
        if(origin.GetComponent<Collider>())
        {
            Physics.IgnoreCollision(GetComponent<CharacterController>(), origin.GetComponent<Collider>(), toggle);
        }
    }

    public void ReleaseObject(GrabInteractable grab)
    {
        grabbedObject = null;
        ToggleColliders(grab.transform, false);
    }

    public Interactable CheckForInteractable()
    {
        GameObject gameObject = GetRaycast();
        if(gameObject && gameObject.GetComponentInParent<Interactable>())
        {
            return gameObject.GetComponentInParent<Interactable>();
        }
        return null;
    }

    public GameObject GetRaycast()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.GetChild(0).Find("LHand").transform.position, transform.GetChild(0).Find("LHand").transform.forward - transform.GetChild(0).Find("LHand").transform.up, out hit))
        {
            if(Vector3.Distance(hit.point, transform.GetChild(0).Find("LHand").transform.position) < range)
            {
                return hit.transform.gameObject;
            }

        }
        return null;
    }

    public bool InputHeld()
    {
        return false && Input.GetKey(KeyCode.LeftShift);
    }
}
