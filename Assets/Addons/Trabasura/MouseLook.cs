using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;

    float xRotation = 0f;

    
    public float mouseSensitivity = 100f;

    // Start is called before the first frame update

    public static MouseLook instance;
    void Awake()
    {
        instance = this;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start()
    {
    }

    public static void ChangeSensitivity(float sensitivity)
    {
        instance.mouseSensitivity = sensitivity;
    }

    float lastTimeScale = 1;

    private void FixedUpdate() {
        lastTimeScale = Mathf.Lerp(lastTimeScale, Time.timeScale, Time.fixedDeltaTime/Time.timeScale);    
    }
    // Update is called once per frame
    void Update()
    {
        //if(GameManager.uiOpened) return;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime/lastTimeScale;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime/lastTimeScale;


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90 , 90);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
