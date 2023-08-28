using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOMotionScript : MonoBehaviour
{
    private int motionCounter = 0;
    private int dir = 1;
    private int MaxMotion = 100;
    private bool enableMotionFlag = false;
    //private GameObject golfBall;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Staring UFO ");
    }

    // Update is called once per frame
    void Update()
    {
        if (motionCounter > MaxMotion) {
            motionCounter = 0;
            dir *= -1;
        }
        if (enableMotionFlag) {
            //transform.Translate(new Vector3(0.1f * dir, 0f, 0f), Space.World);
            transform.position += new Vector3(0.01f * dir, 0f, 0f);
            motionCounter += 1;
            //Debug.Log("Moving UFO " + gameObject.name);
        }
    }

    public void enableUFOMotion() {
        Debug.Log("Motion reach UFO " + gameObject.name);
        enableMotionFlag = true;
        Rigidbody ufoRb = gameObject.GetComponent<Rigidbody>();
        ufoRb.useGravity = false;
        ufoRb.constraints |= RigidbodyConstraints.FreezePositionY;
        ufoRb.Sleep();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Party UFO " + collision.gameObject.tag);
        if (collision.gameObject.tag == "GolfBall") {
            Debug.Log("UFO GolfBall Restart");
            collision.gameObject.SendMessage("MoveToStartTile");
        }
    }
}
