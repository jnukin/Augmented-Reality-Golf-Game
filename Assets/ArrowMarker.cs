using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMarker : MonoBehaviour
{
    public string targetObjectTag = "";
    private GameObject targetObject = null;
    private bool playMode = false;
    private GameObject actualArrow;

    // Start is called before the first frame update
    void Start()
    {
        actualArrow = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject == null)
        {
            var v = GameObject.FindGameObjectsWithTag(targetObjectTag);
            if (v.Length > 0)
            {
                targetObject = v[0];
                Debug.Log("Found " + targetObjectTag);
            }
        }

        if (targetObject != null && playMode)
        {
            //actualArrow.SetActive(!targetObject.GetComponent<Renderer>().isVisible);
            Vector3 direction = targetObject.transform.position - transform.position;
            direction.y = 0f;

            // Rotate the arrow towards the target object
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void EnterPlayMode()
    {
        //if (golfHole != null) {
        playMode = true;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        //}
    }

    public void EnterBuildMode()
    {
        playMode = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
