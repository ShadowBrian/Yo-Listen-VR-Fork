using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FairyController : DialogueReader
{
    public Sprite[] faces;
    public Transform wingA, wingB;
    public SpriteRenderer faceRenderer;
    

    
    public float RotationSpeed;
    public static FairyController instance;
 
     //values for internal use
     protected Quaternion _lookRotation;
    protected Vector3 _direction;

    public ScriptData startGameScript, firstRespawn, doorFailure;

    public ScriptData[] taunts;

    public void BeginGameDialog()
    {
        StartCoroutine(IBeginGame());
    }

    public override void ReadLetter(string letter)
    {
        base.ReadLetter(letter);
        ChangeFace(Random.Range(0, 3));
    }

    public IEnumerator IBeginGame()
    {
        StopCoroutine(ExplodeFaceChange());
        yield return GetComponent<DialogueReader>().ReadScript(startGameScript);
        GameManager.instance.startGame = true;
        StartCoroutine(ExplodeFaceChange());
    }
     
    // Start is called before the first frame update    
    void Start()
    {
        instance = this;
        StartCoroutine(AnimateWings());
        if(GameManager.instance && GameManager.instance.restarts == 1)
        {
            DoFirstRespawn();
        }
        else
        {
            StartCoroutine(ExplodeFaceChange());
            if(Random.value > 0.75f)
            {
                StartScript(taunts[Random.Range(0, taunts.Length)]);
            }
        }
    }

    private void Update() {
        FacePlayer();
    }

    public void ChangeFace(int i) 
    {
        faceRenderer.sprite = faces[i];
    }

    public IEnumerator ExplodeFaceChange()
    {
        yield return new WaitForSeconds(6);
        ChangeFace(2);
        yield return new WaitForSeconds(2);
        ChangeFace(3);
        yield return new WaitForSeconds(1);
        ChangeFace(4);
        StartCoroutine(ChangeColor());
    }
    
    public void DoFirstRespawn()
    {
        StartCoroutine(FirstRespawn());
    }
    
    public IEnumerator FirstRespawn()
    {
        StopCoroutine(ExplodeFaceChange());
        GameManager.instance.startGame = false;
        yield return ReadScript(firstRespawn);
        GameManager.instance.startGame = true;
        StartCoroutine(ExplodeFaceChange());
    }

    public void DoDoorFail()
    {
        StartCoroutine(DoorFail());
    }
    public IEnumerator DoorFail()
    {
        StopCoroutine(ExplodeFaceChange());
        GameManager.instance.startGame = false;
        yield return ReadScript(doorFailure);
        yield return (ChangeColor());
        GameManager.instance.Explode();
        GameManager.instance.Lose("...by trying to take the coward's way out");
        GameManager.instance.startGame = true;
        //Todo BlowUp animation;
    }

    public Renderer r;

    public IEnumerator ChangeColor()
    {
        Color c = r.material.GetColor("_Color");
        c = Color.Lerp(c, Color.red, Time.deltaTime);
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime;
            c = Color.Lerp(c, Color.red, t);
            r.material.SetColor("_Color", c);
            yield return null;
        }
    }

    public IEnumerator AnimateWings()
    {
        while(true)
        {
            wingA.DOLocalRotate(new Vector3(0,45, 0), 0.25f);
            wingB.DOLocalRotate(new Vector3(0,-45, 0), 0.25f);
            yield return new WaitForSeconds(0.25f);
            wingA.DOLocalRotate(new Vector3(0,0, 0), 0.25f);
            wingB.DOLocalRotate(new Vector3(0,0, 0), 0.25f);
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void FacePlayer()
    {
        var target = PlayerController.instance.transform.GetChild(0).position;
        //find the vector pointing from our position to the target
         _direction = (target - transform.position).normalized;
 
         //create the rotation we need to be in to look at the target
         _lookRotation = Quaternion.LookRotation(_direction);
 
         //rotate us over time according to speed until we are in the required rotation
         transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.fixedDeltaTime * RotationSpeed);

         Vector3 idealPosition = (transform.position - target).normalized * 2;
         idealPosition += target;

         GetComponent<Rigidbody>().AddForce(idealPosition - transform.position);

        RaycastHit hit;
        if(Physics.Raycast(target, (transform.position-target), out hit))
        {
            GetComponent<Collider>().isTrigger = (hit.transform != transform);
        }
        
    }
}
