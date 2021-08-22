using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Hand
{
    Left,
    Right
}
public class Grabber : MonoBehaviour
{
    [SerializeField] private Hand hand;
    [SerializeField] private Animator anim;
    private string gripButton;
    private string triggerButton;
    private InteractiveObject hoveredObject;
    private InteractiveObject grabbedObject;

    private void OnTriggerEnter(Collider other)
    {
        InteractiveObject touchingObject = other.GetComponent<InteractiveObject>();
        if (touchingObject != null)
        {
            hoveredObject = touchingObject;
            hoveredObject.OnHoverStart();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractiveObject stoptouchingObject = other.GetComponent<InteractiveObject>();
        if (stoptouchingObject == hoveredObject && stoptouchingObject != null)
        {
            hoveredObject.OnHoverEnd();
            hoveredObject = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gripButton = "XRI_" + hand +"_GripButton";
        triggerButton = "XRI_" + hand + "_TriggerButton";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown(gripButton))
        {
            Debug.Log(hand + " Grip Pressed");

            anim.SetBool("Gripped", true);
            if(hoveredObject != null)
            {
                grabbedObject = hoveredObject;
                grabbedObject.OnGrabStart(this);
            }
        }

        if (Input.GetButtonUp(gripButton))
        {
            Debug.Log(hand + " Grip Rleased");

            anim.SetBool("Gripped", false);
            if (grabbedObject != null)
            {
                grabbedObject.OnGrabEnd();
                grabbedObject = null;
            }
        }
        
        if (Input.GetButtonDown(triggerButton) && grabbedObject !=null)
        {
            grabbedObject.OnTriggerStart();
        }

        if (Input.GetButtonUp(triggerButton) && grabbedObject != null)
        {
            grabbedObject.OnTriggerEnd();
        }
    }
}
