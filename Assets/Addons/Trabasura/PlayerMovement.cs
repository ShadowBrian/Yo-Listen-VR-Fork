using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    Vector3 mouseInput; 

    public bool canJump = true;
    public float speed = 12f;
    public float airResistance = 0.05f;
    public bool groundedLastFrame= false;

    public Transform groundPoint;

    public LayerMask groundMask;
    public float groundDistance = 0.4f;

    public float jumpHeight = 2.5f, jumpBoost = 1.25f;

    public bool moveable = true;

    public List<AudioClip> footsteps;
    public AudioClip jump;
    public float footStepDistance = 0;
    float footStepCheck =0;
    Transform ground;
    public bool isGrounded{
        get{
            var cols = Physics.OverlapSphere(groundPoint.position, groundDistance, groundMask);
            foreach(var col in cols)
            {
                if(col.transform != transform && !col.isTrigger) 
                {
                    ground = col.transform;
                    return true;
                }

            }
            return false;
        }
    }
    
    public float gravity = -9.81f;
    public Vector3 movementInput
    {
        get{
            // if(GameManager.uiOpened) return Vector3.zero;
            return new Vector3(Input.GetAxis("Horizontal"), 0 ,Input.GetAxis("Vertical"));
        }
    }

    public Vector3 velocity, jumpDirection, expectedChange, startLocalPosition;
    // Update is called once per frame
    //TODO Parent in spite of moving character controller
    void Update()
    {   
        startLocalPosition = transform.localPosition;
        var eu = transform.eulerAngles;
        eu.x = 0;
        eu.z = 0;
        transform.eulerAngles = eu;
        if(moveable)
            mouseInput = Vector3.ClampMagnitude(movementInput, 1);

        var inputVelocity = transform.TransformDirection(mouseInput) * Time.deltaTime * speed;
        if(!isGrounded && groundedLastFrame)
        {
            jumpDirection = inputVelocity/Time.deltaTime;
        }
        groundedLastFrame = isGrounded;
        
        if(isGrounded)
        {       
            footStepCheck += inputVelocity.magnitude;
            if(footStepCheck > footStepDistance && footsteps.Count > 0)
            {
                footStepCheck = 0;
                GetComponent<AudioSource>().clip = footsteps[Random.Range(0, footsteps.Count)];
                GetComponent<AudioSource>().Play();
            }
            if(moveable && controller.enabled)controller.Move(inputVelocity);
            if(!Input.GetButton("Jump"))
            {
                jumpDirection = Vector3.Lerp(jumpDirection, Vector3.zero, 1);
            }
        }
        else{
            jumpDirection += inputVelocity * 2;
            jumpDirection = Vector3.ClampMagnitude(jumpDirection , speed * jumpBoost);
            jumpDirection *= (1-airResistance*Time.deltaTime);
        }



        // velocity.y = Mathf.Clamp(velocity.y, gravity/2, 100);
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
            transform.parent = ground;
        }
        if(!isGrounded){
            velocity.y += gravity * Time.deltaTime;
            transform.parent = null;
        }
        if(moveable && controller.enabled)controller.Move((velocity + jumpDirection) * Time.deltaTime);

        if(Input.GetButton("Jump") && isGrounded && moveable && canJump)// && !GameManager.uiOpened)
        {
            if(velocity.y <= 0)
            {
                GetComponent<AudioSource>().clip = jump;

                GetComponent<AudioSource>().Play();
            }
            Debug.Log(isGrounded);
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            
        }
        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundPoint.position, groundDistance);
    }
}
