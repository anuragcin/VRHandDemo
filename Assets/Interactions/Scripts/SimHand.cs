using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimHand : MonoBehaviour
{
    public float speed;
    private GrabberObject hoveredObject;
    private GrabberObject grabbedObject;
    [SerializeField] private Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        GrabberObject touchingObject = other.GetComponent<GrabberObject>();
        if (touchingObject != null)
        {
            hoveredObject = touchingObject;
            hoveredObject.OnHoverStart();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GrabberObject stoptouchingObject = other.GetComponent<GrabberObject>();
        if (stoptouchingObject == hoveredObject && stoptouchingObject != null)
        {
            hoveredObject.OnHoverEnd();
            hoveredObject = null;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Handle rotation
        transform.Rotate(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);

        // Handle movement
        transform.position += transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.position += transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetBool("Gripped", true);
            if (hoveredObject != null)
            {
                grabbedObject = hoveredObject;
                grabbedObject.OnGrabStart(this.GetComponent<Grabber>());
            }
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            anim.SetBool("Gripped", false);
            if (grabbedObject != null)
            {
                grabbedObject.OnGrabEnd();
                grabbedObject = null;
            }
        }
    }
}
