using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

// Reference : [ChatGPT] [Prompt] "unity script to check for swipe with swipe speed on mobile touch input";

public class SwipeDetector : MonoBehaviour
{
    public GameObject golfBall;

    public Button plusPlayButton;
    public Button minusPlayButton;
    public Text playDegreeDisplay;

    private float hitAngle = 0f;

    public float minSwipeDistance = 50f;
    //50f

    public float minSwipeSpeed = 500f;
    //100f

    private Vector2 touchStartPos;

    private float touchStartTime;

    private float touchEndTime;

    private void Start()
    {
        plusPlayButton.onClick.AddListener(plusPlayButtonCall);
        minusPlayButton.onClick.AddListener(minusPlayButtonCall);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // record the touch start position and time
                    touchStartPos = touch.position;
                    touchStartTime = Time.time;
                    break;
                case TouchPhase.Ended:
                    float touchDistance = Vector2.Distance(touchStartPos, touch.position);
                    float touchDuration = Time.time - touchStartTime;

                    float swipeSpeed = touchDistance / touchDuration;

                    if (touchDistance > minSwipeDistance && swipeSpeed > minSwipeSpeed)
                    {
                        Vector2 swipeDirection = touch.position - touchStartPos;
                        Vector2 v = swipeDirection.normalized;

                        // y will map to depth, which is forward vector
                        // x will map to right, which is right vector

                        Vector3 zVector = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
                        zVector.Normalize();

                        Vector3 xVector = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z);
                        xVector.Normalize();

                        Vector3 hitdir = v.x * xVector + v.y * zVector;
                        hitdir.Normalize();

                        //Vector3 centerVector = Camera.main.transform.forward;
                        hitdir = Quaternion.Euler(hitAngle, 0f, 0f) * hitdir;
                        hitdir.y = hitdir.y > 0 ? hitdir.y : -1f * hitdir.y;
                        hitdir.Normalize();
                        golfBall?.SendMessage("hitBall", swipeSpeed * hitdir);
                        //golfBall?.SendMessage("hitBall", swipeSpeed);
                    }
                    break;
            }
        }
    }

    private void plusPlayButtonCall()
    {
        if (hitAngle + 5f > 90f)
        {

        }
        else
        {
            hitAngle += 5f;
            changeProjectionAngle();
            playDegreeDisplay.text = hitAngle + " degrees";
        }
    }

    private void minusPlayButtonCall()
    {
        if (hitAngle - 5f < 0f)
        {

        }
        else
        {
            hitAngle -= 5f;
            changeProjectionAngle();
            playDegreeDisplay.text = hitAngle + " degrees";
        }
    }

    private void changeProjectionAngle() {
        golfBall.SendMessage("updateProjectionAngle", hitAngle);
    }
}

