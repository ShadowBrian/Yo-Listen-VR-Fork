using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class DialogueReader : MonoBehaviour
{
    public float speed = 0.1f;
    private List<string> sounds = new List<string>(){
        "a",
        "b",
        "c",
        "d",
        "e",
        "f",
        "g",
        "h",
        "i",
        "j",
        "k",
        "l",
        "m",
        "n",
        "p",
        "q",
        "r",
        "s",
        "t",
        "v",
        "w",
        "x",
        "y",
        "z",
        "ch",
        "th",
        "sh",
        "ph",
        "wh",
        "o",
        "u",
    };

    public ScriptData currentScript;
    public bool autoNext = false;

    public virtual void ReadLetter(string letter)
    {
        int index = sounds.IndexOf(letter);
        float location = index * 0.25f;
        StopCoroutine("PlaySound");
        Debug.Log(location);
        StartCoroutine(PlaySound(location, 0.2f));
    }

    public TextMeshProUGUI textDisplay;

    public void StartScript(ScriptData script)
    {
        this.currentScript = script;
        StopAllCoroutines();
        textDisplay.maxVisibleCharacters = 0;
        StartCoroutine(ReadScript(script));
    }

    public IEnumerator ReadScript(ScriptData script)
    {
        foreach(var text in script.textToRead)
        {
            textDisplay.maxVisibleCharacters = 0;
            string currentRead = text;
            textDisplay.text = currentRead;
            currentRead = currentRead.ToLower();

            yield return null;
            var tInfo = textDisplay.textInfo;
            for(int i = 0; i < tInfo.characterCount; i++)
            {
                Debug.Log(currentRead.Substring(i,1));
                yield return new WaitForSeconds(speed);
                textDisplay.maxVisibleCharacters = i+1;
                string checkText = "" + tInfo.characterInfo[i].character;
                checkText = checkText.ToLower();
                Debug.Log(checkText);
                if(sounds.Contains(checkText))
                {
                    if(i+1<tInfo.characterCount)
                    {
                        var nextCheck = "" + tInfo.characterInfo[i+1].character;
                        nextCheck = nextCheck.ToLower();
                        if(nextCheck.Equals("h") && sounds.Contains(checkText + nextCheck))
                        {
                            ReadLetter(checkText+nextCheck);
                            i+=1;
                            continue;
                        }
                    }
                    ReadLetter(checkText);
                }
            }
            yield return new WaitForSeconds(0.5f);
            textDisplay.maxVisibleCharacters = 0;
        }


        if(autoNext && script.next)
        {
            StartScript(script.next);
        }
    }

    public IEnumerator PlaySound(float location, float time)
    {
        GetComponent<AudioSource>().time = location;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(time);
        GetComponent<AudioSource>().Stop();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(currentScript)
        {
            StartCoroutine(ReadScript(currentScript));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
