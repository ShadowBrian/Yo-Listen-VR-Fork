using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DevConsoleController : MonoBehaviour
{
    public Transform InputField;
    protected bool active = false;
    
    // Start is called before the first frame update
    void Start()
    {
        active = true;
        ToggleDevConsole();
    }

    public virtual bool KeyCombination()
    {
        return Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftShift); 
    }

    public virtual void ToggleDevConsole()
    {
        active = !active;
        InputField.gameObject.SetActive(active);
        if(active)
        {
            EventSystem.current.SetSelectedGameObject(InputField.gameObject);
        }
    }

    public void ProcessInput(string input)
    {
        ProcessInput(input.Split());
    }

    public abstract void ProcessInput(string[] args);
    // Update is called once per frame
    void Update()
    {
        if(KeyCombination())
        {
            ToggleDevConsole();
        }
    }
}
