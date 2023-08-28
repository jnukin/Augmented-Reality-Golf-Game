using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.VisualElement;
using UnityEngine.Networking.Types;
using UnityEngine.Profiling;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;

public class DrawArrow : MonoBehaviour
{
    //[SerializeField] private GameObject sessionOrigin;
    private float rayLength = 1f; // the length of the ray
    private float rayWidth = 0.05f; // the width of the ray
    public GameObject golfHoleArrowHeadPrefab; // the prefab for the arrowhead
    private LineRenderer golfHoleLineRenderer;
    private GameObject golfHoleArrowHead;

    public GameObject golfBallArrowHeadPrefab; // the prefab for the arrowhead
    private LineRenderer golfBallLineRenderer;
    private GameObject golfBallArrowHead;

    private GameObject golfHole = null;
    private bool playMode = false;
    private GameObject golfBall = null;

    private float groundHeight = 0f;

    

    void Start()
    {
        // Get a reference to the LineRenderer component
        golfHoleLineRenderer = GetComponent<LineRenderer>();

        // Set the initial position and width of the line
        //golfHoleLineRenderer.SetPosition(0, transform.position);
        //golfHoleLineRenderer.SetPosition(1, transform.position + transform.forward * rayLength);
        golfHoleLineRenderer.startWidth = rayWidth;
        golfHoleLineRenderer.endWidth = rayWidth;

        // Instantiate the arrowhead at the end of the line
        //golfHoleArrowHead = Instantiate(golfHoleArrowHeadPrefab, golfHoleLineRenderer.GetPosition(1), Quaternion.identity);
        //golfHoleArrowHead.transform.parent = transform;
        //golfHoleLineRenderer.enabled = true;
        //golfBallLineRenderer.
    }

    void Update()
    {
        if (golfHole == null)
        {
            var v = GameObject.FindGameObjectsWithTag("GolfHole");
            if (v.Length > 0)
            {
                golfHole = v[0];
                Debug.Log("Found Golf Hole");
            }
        }
        //if (golfBall == null)
        //{
        //    var v = GameObject.FindGameObjectsWithTag("StartGameGolfTile");
        //    if (v.Length > 0)
        //    {
        //        golfBall = v[0];
        //    }
        //}
        if (golfHole != null && playMode)
        {
            //Vector3 startPos = golfHole.transform.position;
            //Vector3 cameraPos = Camera.main.transform.position + new Vector3(0.1f, 0.1f, 0.1f);
            //cameraPos.y = groundHeight;
            //Vector3 direction = cameraPos - startPos;
            //direction.Normalize();
            //Debug.Log("NikunjLine Direction " + direction);

            //golfHoleLineRenderer.SetPosition(0, startPos);
            //golfHoleLineRenderer.SetPosition(1, cameraPos);


            Vector3 startPos = Camera.main.transform.position;
            startPos.y = groundHeight;

            // Calculate the direction vector from this object to the target object
            Vector3 direction = golfHole.transform.position - startPos;
            direction.y = 0;
            direction.Normalize();
            Debug.Log("NikunjLine Direction " + direction);
            // Update the position and width of the line to match the direction
            golfHoleLineRenderer.SetPosition(0, startPos);
            golfHoleLineRenderer.SetPosition(1, startPos + direction.normalized * rayLength);

            //gameObject.transform.position = startPos + (direction.normalized * 0.5f);


            //// Update the position and rotation of the arrowhead to match the end of the line
            //golfHoleArrowHead.transform.position = golfHoleLineRenderer.GetPosition(1);
            //golfHoleArrowHead.transform.rotation = Quaternion.LookRotation(direction);

        }
        //if (golfBall != null && playMode)
        //{
        //    // Calculate the direction vector from this object to the target object
        //    Vector3 direction = golfBall.transform.position - transform.position;

        //    // Update the position and width of the line to match the direction
        //    golfBallLineRenderer.SetPosition(0, transform.position);
        //    golfBallLineRenderer.SetPosition(1, transform.position + direction.normalized * rayLength);

        //    // Update the position and rotation of the arrowhead to match the end of the line
        //golfBallArrowHead.transform.position = golfBallLineRenderer.GetPosition(1);
        //golfBallArrowHead.transform.rotation = Quaternion.LookRotation(direction);
        //}
    }

    public void EnterPlayMode(float minHeight) {
        //if (golfHole != null) {
            playMode = true;
            golfHoleLineRenderer.enabled = true;
            groundHeight = minHeight;
        Debug.Log("RAYCASTER PLAYMODE");
        //}
    }

    public void EnterBuildMode()
    {
        playMode = false;
        golfHoleLineRenderer.enabled = false;
    }
    //Create a new 3D object in your scene, such as a cube or a sphere.
    //Scale the object down so that it is smaller and more arrow-like in shape.
    //Rotate the object so that it points in the direction of the positive z-axis.
    //Add a new material to the object, and set the material's color to the color you want the arrowhead to be.
    //Apply any additional modifications you want to the object to customize its appearance.
    //Once you are happy with the appearance of the object, drag it from the Hierarchy panel into the Project panel to create a new prefab.
    //You can now use this prefab in your code to instantiate the arrowhead object at the end of your LineRenderer.
}
