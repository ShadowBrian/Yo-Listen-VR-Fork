using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotGrab : GrabInteractable
{

    public ScriptData spawn, startTutorial, desperation, end, grabbedPot;
    public DialogueReader dialogue;
    bool potGrabbed = false, potCollided = false;

    public GameObject fairy;

    public override bool CheckIfInteractable()
    {
        return true;
    }
    

    public override void Start()
    {
        base.Start();
        StartCoroutine(TextLoop());
    }

    public override void OnInteract()
    {
        base.OnInteract();
        potGrabbed = true;
    }

    private void OnCollisionEnter(Collision other) {
        if(!potGrabbed) return;
        potCollided = true;
        GameManager.instance.CreateFairy(transform.position);
        Destroy(gameObject);
    }

    public override void Release()
    {
        base.Release();
    }

    private void OnDestroy() {
        var go = Instantiate(Resources.Load<GameObject>("ShatterSound"));
        go.transform.position = transform.position;
        Destroy(go, 1);
    }
    public IEnumerator TextLoop()
    {
        yield return new WaitForSeconds(1);
        yield return dialogue.ReadScript(spawn);
        yield return new WaitForSeconds(2);
        dialogue.StartScript(startTutorial);
        Timer t = new Timer(120);
        yield return new WaitUntil(()=>t.Finished()|| potGrabbed);
        if (potGrabbed)
        {
            dialogue.StartScript(grabbedPot);
            yield return new WaitUntil(()=> potCollided);

        }
        else{
            dialogue.StartScript(desperation);
            t = new Timer(40);
            yield return new WaitUntil(()=>t.Finished()|| potGrabbed);
            if (potGrabbed)
            {
                dialogue.StartScript(grabbedPot);
                yield return new WaitUntil(()=> potCollided);

                yield break;
            }
            potGrabbed = false;
            dialogue.StartScript(end);
            yield return new WaitForSeconds(12);
            GameManager.instance.Win("...by making a fairy die from abandonment");
        }
    }
}
