using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : GrabberObject
{

    public Transform tip;
    public TrailRenderer brushStrokePrefab;
    public Color[] colors;


    private TrailRenderer currentStroke;
    private int currentColorIndex;
    private Color currentColor;
    private Stack<TrailRenderer> previousStrokes = new Stack<TrailRenderer>();

    private void Start()
    {
        UpdateColor();
    }
    public override void OnTriggerStart()
    {
        base.OnTriggerStart();

        //Create a new brush storke and parent to brush tip
        currentStroke = Instantiate(brushStrokePrefab, tip.position, tip.rotation, tip);

        ///Change the color of new brushstroke to the current color
        currentStroke.material.color = currentColor;

    }

    public override void OnTriggerEnd()
    {
        base.OnTriggerEnd();
        //unparent the current brush stroke from brush tip
        currentStroke.transform.SetParent(null);

        previousStrokes.Push(currentStroke);

    }

    public void CycleColor()
    {
        //Increase the current colorindex
        currentColorIndex++;

        if (currentColorIndex == colors.Length)
        {
            currentColorIndex = 0;
        }

        UpdateColor();
       

    }

    private void UpdateColor()
    {
        //Update the current Color
        currentColor = colors[currentColorIndex];

        //Change the color of the brush tip
        tip.GetComponent<Renderer>().material.color = currentColor;

    }

    public void undo()
    {
        //If there are any previous strokes 
        if (previousStrokes.Count > 0)
        {
            //pop the most recent stroke from the stack
            var lastStroke = previousStrokes.Pop();
            Destroy(lastStroke.gameObject, 2f);

        }
    }
}
