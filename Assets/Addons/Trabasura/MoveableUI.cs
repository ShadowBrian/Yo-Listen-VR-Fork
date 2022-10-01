using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MoveableUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{

    bool moving = false;
    public Vector3 startPosition;
    
    public bool bounding;
    public  Bounds maxPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = transform.localPosition - Input.mousePosition;
        moving = true;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        moving = false;
        // throw new System.NotImplementedException();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if(moving)
         {
            
            transform.localPosition = startPosition + Input.mousePosition;
            if(bounding)
            {
                if(!maxPosition.Contains(transform.localPosition))
                {
                    transform.localPosition = maxPosition.ClosestPoint(transform.localPosition);
                }
            }
         }
    }
}
