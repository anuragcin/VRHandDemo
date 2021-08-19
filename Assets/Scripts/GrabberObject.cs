using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberObject : MonoBehaviour
{
    [SerializeField] private Color hoveredColor;

    [SerializeField] private float m_throwforce = 250f;

    private Color originalColor;
    private Material material;

    private List<Vector3> trackedPositions = new List<Vector3>();
    private bool isHeld;

    private void Awake()
    {
        material = this.GetComponent<Renderer>().material;
        originalColor = material.color;
    }

    /// <summary>
    /// Fixed Update - Matched the Physics update per frame
    /// </summary>
    private void FixedUpdate()
    {
        if (isHeld)
        {
            if (trackedPositions.Count > 20)
            {
                trackedPositions.RemoveAt(0);
                trackedPositions.Add(transform.position);
            }
            else
            {
                trackedPositions.Add(transform.position);
            }
        }
    }

    public void OnHoverStart()
    {
        material.color = hoveredColor;
    }

    public void OnHoverEnd()
    {
        material.color = originalColor;
    }

    public void OnGrabStart(Grabber hand)
    {
        isHeld = true;
        this.transform.parent = hand.transform;
        this.GetComponent<Rigidbody>().isKinematic = true;

        //FixedJoint fx = this.gameObject.AddComponent<FixedJoint>();
        //fx.connectedBody = hand.GetComponent<Rigidbody>();
        //fx.breakForce = 5000;
        //fx.breakTorque = 5000;
        //this.transform.parent = hand.transform;
    }

    public void OnGrabEnd()
    {
        isHeld = false;
        this.transform.parent = null;
        this.GetComponent<Rigidbody>().isKinematic = false;

        //FixedJoint fx = GetComponent<FixedJoint>();
        //if (fx)
        //{
        //    Destroy(fx);
        //    this.transform.parent = null ;
        //}

        Vector3 dir = trackedPositions[trackedPositions.Count-1] - trackedPositions[0];
        GetComponent<Rigidbody>().AddForce(dir * m_throwforce);

        trackedPositions.Clear();
    }

    private void OnJointBreak(float breakForce)
    {
        isHeld = false;
        this.transform.parent = null;
    }
}
