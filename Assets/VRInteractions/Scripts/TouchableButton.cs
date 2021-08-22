using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchableButton : InteractiveObject
{

    public Transform button;
    public Transform pressedPosiiton;
    public UnityEvent onPressed;
    public UnityEvent onReleased;

    private Vector3 initialPosition;



    private void Start()
    {
        //store the initial position of the button
        initialPosition = button.position;
    }

    public override void OnHoverStart()
    {
        base.OnHoverStart();

        //Move the button to the pressed position
        button.position = pressedPosiiton.position;

        //Trigger Pressed
        onPressed.Invoke();

    }

    public override void OnHoverEnd()
    {
        base.OnHoverEnd();

        //Move the button back to the initial position
        button.position = initialPosition;

        //Trigger Released 
        onReleased.Invoke();

    }
}
