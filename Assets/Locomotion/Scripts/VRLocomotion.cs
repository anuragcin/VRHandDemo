using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRLocomotion : MonoBehaviour
{

    public Hand hand;
    public float snapRotationAngle;
    public Transform player;
    public float movementSpeed;
    public float teleportRange = 10f;
    public GameObject teleportReticle;
    public string teleportButton;
    public float curveHeight;
    public int numCurveSegments;
    public float fadeDuration;
    public RawImage fadeImage;

    private LineRenderer teleportBeam;


    public void Start()
    {
        teleportBeam = GetComponent<LineRenderer>();

        // Set the number of vertices on the teleport beam
        teleportBeam.positionCount = numCurveSegments + 1;
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        HandleMovement();
        HandleTeleport();

    }

    private void HandleTeleport()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, teleportRange))
        {
            teleportBeam.enabled = true;
            //teleportBeam.SetPosition(0, transform.position);
            //teleportBeam.SetPosition(1, hit.point);
            // Calculate the teleport beam curve
            CalculateBeamCurve(transform.position, hit.point);

            teleportReticle.SetActive(true);
            teleportReticle.transform.position = hit.point;

            if (Input.GetButtonDown(teleportButton))
            {
                //player.position = hit.point;
                // Teleport the player
                StartCoroutine(PerformTeleport(hit.point));
            }

        }
        else
        {
            teleportBeam.enabled = false;
            teleportReticle.SetActive(false);
        }
    }

    private IEnumerator PerformTeleport(Vector3 destination)
    {
        // Start fading to black over some time
        float fadeTime = fadeDuration;

        // While we're still fading
        while (fadeTime > 0)
        {
            // Reduce the time left in the fade
            fadeTime -= Time.deltaTime;
           
            // Calculate the alpha of the black fade image
            float percent = fadeTime / fadeDuration;
            
            fadeImage.color = Color.Lerp(Color.black, Color.clear, percent);
            
            // Take a break for a frame so other code can run
            yield return new WaitForEndOfFrame();
        }
        // Make sure the fade image is completely black
        fadeImage.color = Color.black;
        
        // Do the teleport!
        player.position = destination;
        
        // Start fading to black over some time
        fadeTime = fadeDuration;
        
        // While we're still fading
        while (fadeTime > 0)
        {
            // Reduce the time left in the fade
            fadeTime -= Time.deltaTime;
        
            // Calculate the alpha of the black fade image
            float percent = fadeTime / fadeDuration;
            fadeImage.color = Color.Lerp(Color.clear, Color.black, percent);
            
            // Take a break for a frame so other code can run
            yield return new WaitForEndOfFrame();
        }
        // Make sure the fade image is completely transparent
        fadeImage.color = Color.clear;
    }

    private void CalculateBeamCurve(Vector3 start, Vector3 end)
    {
        // Calculate the midpoint between the start and the end
        Vector3 midpoint = (start + end) / 2f;
      
        // Calculate the position of the curve's control point
        Vector3 controlPoint = midpoint + Vector3.up * curveHeight;
        
        // For each segment in the curve
        for (int i = 0; i < numCurveSegments; i++)
        {
            // Calculate how far along the curve we are as a percentage
            float percent = i / (float)numCurveSegments;
        
            // Calculate the Lerp between the start and the control point
            Vector3 a = Vector3.Lerp(start, controlPoint, percent);
            
            // Calculate the Lerp between the control point and the end
            Vector3 b = Vector3.Lerp(controlPoint, end, percent);
            
            // Finally, Lerp between these two results to get the point on the curve
            Vector3 vertex = Vector3.Lerp(a, b, percent);
            
            // Assign the vertex position to the curve
            teleportBeam.SetPosition(i, vertex);
        }
        
        // Set the last vertex on the curve to the end
        teleportBeam.SetPosition(numCurveSegments, end);
    }

    private void HandleMovement()
    {
        float vertical = -Input.GetAxis("XRI_" + hand + "_Primary2DAxis_Vertical");

        //Get the forward direction of the player
        //Vector3 forwardDir = player.forward;

        Vector3 forwardDir = Camera.main.transform.forward;
        forwardDir.y = 0; // prevent flying

        forwardDir.Normalize(); // maintain normal speed

        player.position += forwardDir * 
                               vertical * 
                                    movementSpeed * 
                                        Time.deltaTime;
    }

    private void HandleRotation()
    {
        //if the thumbstick is pressed
        //Get the horizontal thumbstick aaxis in the controller
        //Determine which angke to rotate
        //Rotate the Player
        if (Input.GetButtonDown("XRI_"+ hand +"_Primary2DAxisClick")) //pushed the thumbstick
        {
            Debug.Log(hand + "Thumb stick pushed");

            float horizontal = Input.GetAxis("XRI_" + hand + "_Primary2DAxis_Horizontal");

            float angle;
            if (horizontal < 0)
            {
                angle = snapRotationAngle;
            }
            else
            {
                angle = -snapRotationAngle;
            }

            player.Rotate(0, angle, 0);
        }
            

    }
}
