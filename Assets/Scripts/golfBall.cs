using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class golfBall : MonoBehaviour
{
    public GameObject plusPlayButton;
    public GameObject minusPlayButton;
    public GameObject displayPlayText;
    public GameObject winText;
    public GameObject golfHoleArrow;
    public GameObject golfBallArrow;
    public AudioSource hitSound;
    public AudioSource winSound;
    public AudioSource loseSound;

    private LineRenderer lr;
    private Vector3 startLr;
    private Vector3 startProjectileDirection;
    private float initialForce;
    float projectionAngle = 0f;

    public GameObject plusRotatePlayButton;
    public GameObject minusRotatePlayButton;

    private Button plusRotatePlayButtonC;
    private Button minusRotatePlayButtonC;

    private float rotateProjectileAngle = 0f;
    private bool allowTeleportation = true;
    private int winCounter = 0;
    private GameObject startTile = null;
    private bool playMode = false;
    private Rigidbody golfBallRb = null;
    private float ballMinHeight = -10f;
    private GameObject[] TeleportationTiles = null;


    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        golfBallRb = gameObject.GetComponent<Rigidbody>();
        Debug.Log("GOLFBALLRB " + golfBallRb.mass);
        golfBallRb.Sleep();

        plusRotatePlayButtonC = plusRotatePlayButton.GetComponent<Button>();
        minusRotatePlayButtonC = minusRotatePlayButton.GetComponent<Button>();

        plusRotatePlayButtonC.onClick.AddListener(plusRotatePlayButtonCall);
        minusRotatePlayButtonC.onClick.AddListener(minusRotatePlayButtonCall);
    }

    void plusRotatePlayButtonCall() {
        rotateProjectileAngle += 30f;
        //rotateProjectileAngle %= 360f; 
    }

    void minusRotatePlayButtonCall() {
        rotateProjectileAngle -= 30f;
        //rotateProjectileAngle %= 360f;
    }

    // Update is called once per frame
    void Update()
    {
        if (TeleportationTiles == null || TeleportationTiles.Length < 2) {
            var v = GameObject.FindGameObjectsWithTag("TeleportHole");
            if (v.Length > 0)
            {
                TeleportationTiles = v;
            }
        }
        if (startTile == null) {
            var v = GameObject.FindGameObjectsWithTag("StartGameGolfTile");
            if (v.Length > 0) {
                startTile = v[0];
            }
        }
        if (startTile != null && playMode)
        {
            if (gameObject.GetComponent<Renderer>().isVisible)
            {
                drawLine();
            }
            if (ballMinHeight > transform.position.y) {
                loseSound.Play();
                golfBallRb.Sleep();
                StartGame(ballMinHeight);
                Debug.Log("Golfball out of Bounds");
            }
        }
    }

    private void drawLine() {
        lr.enabled = true;
        Vector3 pos = transform.position;
        Vector3 dir = transform.right + transform.forward + transform.up;
        dir.Normalize();
        //startVelocity *= 100f;
        startProjectileDirection = Quaternion.Euler(0, rotateProjectileAngle, projectionAngle) * (10f * transform.right);
        Debug.Log("ProjectileNikunj: " + startProjectileDirection);
        int numberOfPoints = 30;
        lr.positionCount = numberOfPoints;
        //List<Vector3> newline;

        float timer = 0.1f;
        int i = 0;
        for (float j = 0; i < numberOfPoints; j += timer) {
            Vector3 linePos = pos + j * startProjectileDirection;
            linePos.y = pos.y + startProjectileDirection.y * j - 0.5f * 1f * j * j;
            
            lr.SetPosition(i, linePos);
            i++;
            if (linePos.y < transform.position.y) {
                break;
            }
        }
        lr.positionCount = i;
    }

    public void StartGame(float minHeight) {
        if (startTile != null)
        {
            Debug.Log("In Start Game Golf ball");
            transform.position = startTile.transform.position + new Vector3(0f, 0.01f, 0f);
            golfBallRb.WakeUp();
            playMode = true;
            ballMinHeight = minHeight;
            setButtonState(true);
            golfHoleArrow.SendMessage("EnterPlayMode");
            golfBallArrow.SendMessage("EnterPlayMode");
        }
        else {
            Debug.LogError("Start tile not Initialised>");
        }
    }

    public void EnterBuildGame()
    {
        //transform.position = startTile.transform.position + new Vector3(0f, 0.1f, 0f);
        golfBallRb.Sleep();
        playMode = false;
        setButtonState(false);
        golfHoleArrow.SendMessage("EnterBuildMode");
        golfBallArrow.SendMessage("EnterBuildMode");
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;
        Debug.Log("GolfBall Collision Tag " + collisionObject.tag);
        if (collisionObject.tag == "TeleportHole" && allowTeleportation)
        {
            golfBallRb.Sleep();
            allowTeleportation = false;
            TeleportBall(collisionObject);
        }
        else if (collisionObject.tag.Contains("UFO"))
        {
            golfBallRb.Sleep();
            transform.position = startTile.transform.position + new Vector3(0f, 0.01f, 0f);
        }
        else if (collisionObject.tag == "GolfHole") {
            golfBallRb.Sleep();
            winSound.Play();
            transform.position = startTile.transform.position + new Vector3(0f, 0.01f, 0f);
            Text t = winText.GetComponent<Text>();
            winCounter++;
            t.text = "Total Wins: " + winCounter;
        }
    }

    void TeleportBall(GameObject collisionObject) {
        Debug.Log("Teleport Ball Called!!!");
        if (TeleportationTiles != null && TeleportationTiles.Length > 1) {
            foreach (GameObject v in TeleportationTiles) {
                if (v.name != collisionObject.name) {
                    Debug.Log("Nikunj Teleportaing from " + collisionObject.name + "to " + v.name);
                    transform.position = v.transform.position + new Vector3(0f, 0.01f, 0f);
                    return;
                }
            }
            Debug.Log("Teleport Ball No Destination!!!");
        }
    }

    void FixedUpdate()
    {
        if (playMode && !golfBallRb.IsSleeping())
        {
            //if (collisionGameObject && collisionGameObject.tag.Contains("Sand"))
            //{
            //    golfBallRb.velocity *= (1 - 0.3f);
            //    //ballRb.angularVelocity *= (1 - 0.3f);
            //}
            //else
            //{
            golfBallRb.velocity *= (1 - 0.1f);
            //ballRb.angularVelocity *= (1 - 0.1f);
            //}
            if (golfBallRb.velocity.magnitude < 0.0001f)
            {
                golfBallRb.velocity = new Vector3();
                golfBallRb.angularVelocity = new Vector3();
                golfBallRb.Sleep();
            }
        }
    }

    //public void hitBall(Vector2 swipedir, float swipeSpeed) {
    public void hitBall(float swipeSpeed)
    {
        if (playMode)
        {
            golfBallRb.WakeUp();
            Debug.Log("BallRB Swipe Speed! " + swipeSpeed);
            hitSound.Play();
            //golfBallRb.velocity = hitVector * 0.0001f;
            //golfBallRb.velocity = hitVector * 0.001f;
            golfBallRb.velocity = startProjectileDirection*(swipeSpeed/3000f);
            allowTeleportation = true;
        }
    }

    public void hitBall(Vector3 hitVector)
    {
        if (playMode)
        {
            golfBallRb.WakeUp();
            Debug.Log("BallRB Swipe Speed! " + hitVector);
            hitSound.Play();
            //golfBallRb.velocity = hitVector * 0.0001f;
            golfBallRb.velocity = hitVector * 0.001f;
            //golfBallRb.velocity = startProjectileDirection * (swipeSpeed / 3000f);
            allowTeleportation = true;
        }
    }

    void setButtonState(bool state) {
        plusPlayButton.SetActive(state);
        minusPlayButton.SetActive(state);
        minusRotatePlayButton.SetActive(state);
        plusRotatePlayButton.SetActive(state);
        displayPlayText.SetActive(state);
        winText.SetActive(state);
    }

    public void MoveToStartTile() {
        golfBallRb.Sleep();
        loseSound.Play();
        transform.position = startTile.transform.position + new Vector3(0f, 0.01f, 0f);
        Debug.Log("Golf Ball Restarts due to UFO");
    }

    public void updateProjectionAngle(float angle) {
        projectionAngle = angle;
    }

}
