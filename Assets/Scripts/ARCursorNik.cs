using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARCursorNik : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public GameObject cursorChildObject;
    public GameObject golfPlanePrefab;
    public GameObject elevatedPlanePrefab;
    public GameObject golfBall;

    public Material golfPlaneMaterial;
    public Material elevatedPlaneMaterial;

    public GameObject objectToPlace;
    public GameObject ufoPrefab;
    public GameObject wmPrefab;
    public GameObject LPrefab;
    public GameObject TPrefab;

    public ARRaycastManager raycastManager;
    public GameObject lockedGolfPlane = null;
    public GameObject lockedElevatedPlane = null;
    public GameObject displayText = null;
    public Button ChangeMode;
    private bool buildPlaneMode = true;
    public bool cursorActive = true;

    public Button upButton;
    public Button downButton;
    public Button leftButton;
    public Button rightButton;

    public Button rotateButton;
    public Button plusButton;
    public Button minusButton;
    public Button doneButton;

    public Button UFOButton;
    public Button WMButton;
    public Button LButton;
    public Button TButton;

    private GameObject currentObject = null;
    private Text displayTextComponent = null;
    private float groundPlaneHeight = 0f;
    private bool playMode = false;
    private bool editMode = false;
    private List<TrackableId> selectedPlanes = new List<TrackableId>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        cursorChildObject.SetActive(true);
        displayTextComponent = displayText.GetComponent<Text>();

        upButton.onClick.AddListener(upButtonCall);
        downButton.onClick.AddListener(downButtonCall);
        leftButton.onClick.AddListener(leftButtonCall);
        rightButton.onClick.AddListener(rightButtonCall);

        rotateButton.onClick.AddListener(rotateButtonCall);
        plusButton.onClick.AddListener(plusButtonCall);
        minusButton.onClick.AddListener(minusButtonCall);
        doneButton.onClick.AddListener(doneButtonCall);

        WMButton.onClick.AddListener(WMButtonCall);
        UFOButton.onClick.AddListener(UFOButtonCall);

        LButton.onClick.AddListener(LButtonCall);
        TButton.onClick.AddListener(TButtonCall);
    }

    void LButtonCall() {
        if (currentObject != null)
        {
            currentObject.gameObject.SetActive(false);
            GameObject ufoObject = Instantiate(LPrefab, currentObject.gameObject.transform.position, Quaternion.identity);
            currentObject = ufoObject;
            //currentObject.gameObject.tag = "UFOBumper";
        }
    }

    void TButtonCall()
    {
        if (currentObject != null)
        {
            currentObject.gameObject.SetActive(false);
            GameObject ufoObject = Instantiate(TPrefab, currentObject.gameObject.transform.position, Quaternion.identity);
            currentObject = ufoObject;
            //currentObject.gameObject.tag = "UFOBumper";
        }
    }

    void WMButtonCall()
    {
        if (currentObject != null)
        {
            currentObject.gameObject.SetActive(false);
            GameObject wmObject = Instantiate(wmPrefab, currentObject.gameObject.transform.position, Quaternion.identity);
            currentObject = wmObject;
        }
    }

    void UFOButtonCall()
    {
        if (currentObject != null)
        {
            currentObject.gameObject.SetActive(false);
            GameObject ufoObject = Instantiate(ufoPrefab, currentObject.gameObject.transform.position, Quaternion.identity);
            currentObject = ufoObject;
            currentObject.gameObject.tag = "UFOBumper";
        }
    }

    void upButtonCall() {
        if (currentObject != null) {
            Vector3 v = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
            v.Normalize();
            currentObject.gameObject.transform.Translate(v * 0.1f, Space.World);
            //currentObject.gameObject.transform.Translate(Camera.main.transform.forward * 0.1f, Space.World);
            //currentObject.gameObject.transform.Translate(new Vector3(0f, 0f , 0.1f), Space.World); 
        }
    }

    void downButtonCall()
    {
        if (currentObject != null)
        {
            Vector3 v = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
            v.Normalize();
            currentObject.gameObject.transform.Translate(-v * 0.1f, Space.World);
            //currentObject.gameObject.transform.Translate(-0.1f * Camera.main.transform.forward, Space.World);
            //currentObject.gameObject.transform.Translate(new Vector3(0f, 0f, -0.1f), Space.World);
        }
    }
    void leftButtonCall()
    {
        if (currentObject != null)
        {
            Vector3 v = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z);
            v.Normalize();
            currentObject.gameObject.transform.Translate(-v * 0.1f, Space.World);
            //currentObject.gameObject.transform.Translate(-0.1f * Camera.main.transform.right, Space.World);
            //currentObject.gameObject.transform.Translate(new Vector3(-0.1f, 0f, 0f), Space.World);
        }
    }

    void rightButtonCall()
    {
        if (currentObject != null)
        {
            Vector3 v = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z);
            v.Normalize();
            currentObject.gameObject.transform.Translate(v * 0.1f, Space.World);
            //currentObject.gameObject.transform.Translate(Camera.main.transform.right * 0.1f, Space.World);
            //currentObject.gameObject.transform.Translate(new Vector3(0.1f, 0f, 0f), Space.World);
        }
    }

    void rotateButtonCall()
    {
        if (currentObject != null)
        {
            currentObject.gameObject.transform.Rotate(new Vector3(0f, 90f, 0f));
        }
    }

    void plusButtonCall()
    {
        if (currentObject != null)
        {
            currentObject.gameObject.transform.localScale += currentObject.gameObject.transform.localScale/2;
        }
    }

    void minusButtonCall()
    {
        if (currentObject != null)
        {
            currentObject.gameObject.transform.localScale -= currentObject.gameObject.transform.localScale / 2;
        }
    }

    void doneButtonCall()
    {
        if (currentObject != null)
        {
            if (currentObject.tag.Contains("UFO")) {
                Debug.Log("UFO enabling motion");
                currentObject.SendMessage("enableUFOMotion");
            }
            Rigidbody currRb = gameObject.GetComponent<Rigidbody>();
            if (currRb != null)
            {
                currRb.useGravity = false;
                currRb.constraints |= RigidbodyConstraints.FreezePositionY;
                currRb.Sleep();
            }
            displayTextComponent.text = "Tap to Mark Real World Objects";
            editMode = false;
        }
    }

    private void changeButtonStatus(bool newState) {
        upButton.gameObject.SetActive(newState);
        downButton.gameObject.SetActive(newState);
        leftButton.gameObject.SetActive(newState);
        rightButton.gameObject.SetActive(newState);

        rotateButton.gameObject.SetActive(newState);
        plusButton.gameObject.SetActive(newState);
        minusButton.gameObject.SetActive(newState);
        doneButton.gameObject.SetActive(newState);

        UFOButton.gameObject.SetActive(newState);
        WMButton.gameObject.SetActive(newState);

        LButton.gameObject.SetActive(newState);
        TButton.gameObject.SetActive(newState);
    }

    // Update is called once per frame
    void Update()
    {
        if (cursorActive)
        {
            updateCursor();
        }

        if (!playMode)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !editMode)
            {
                if (buildPlaneMode)
                {
                    Vector2 screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
                    List<ARRaycastHit> hits = new List<ARRaycastHit>();
                    raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

                    if (lockedGolfPlane == null)
                    {
                        Vector3 t = transform.position;
                        groundPlaneHeight = t.y;
                        //t.y -= 10;
                        var p = hits[0].trackableId;
                        selectedPlanes.Add(p);
                        hits[0].trackable.gameObject.GetComponent<MeshRenderer>().material = golfPlaneMaterial;
                        lockedGolfPlane = hits[0].trackable.gameObject;
                        //Debug.Log("NikunjRotationPlane " + lockedGolfPlane.transform.rotation);
                        lockedGolfPlane.transform.rotation = Quaternion.identity;
                        //Debug.Log(" " + lockedGolfPlane.transform.rotation);
                        //hits[0].trackable.gameObject.SetActive(false);
                        //golfPlanePrefab.transform.localScale = p.transform.lossyScale;
                        //lockedGolfPlane = GameObject.Instantiate(golfPlanePrefab, t, transform.rotation);
                        displayTextComponent.text = "Select Elevated Plane";
                        Debug.Log("NIKUNJPROJECT : " + lockedGolfPlane.name);
                    }
                    else
                    {
                        Debug.Log("NikunjPlane " + hits[0].trackable.gameObject.transform.position.y + " " + groundPlaneHeight + " " + (hits[0].trackable.gameObject.transform.position.y > groundPlaneHeight) );
                        if (hits[0].trackable.gameObject.transform.position.y > groundPlaneHeight)
                        {
                            //hits[0].trackable.gameObject.SetActive(false);
                            //lockedElevatedPlane = GameObject.Instantiate(elevatedPlanePrefab, transform.position, transform.rotation);
                            var p = hits[0].trackableId;
                            selectedPlanes.Add(p);
                            hits[0].trackable.gameObject.GetComponent<MeshRenderer>().material = elevatedPlaneMaterial;
                            lockedElevatedPlane = hits[0].trackable.gameObject;
                            lockedElevatedPlane.transform.rotation = Quaternion.identity;
                            planeManager.planesChanged += DisableNewPlanes;
                            SetAllPlanesActive(false);
                            buildPlaneMode = false;
                            displayTextComponent.text = "Tap to Mark Real World Objects";
                            ChangeMode.onClick.AddListener(StartPlayMode);
                        }
                        else
                        {
                            displayTextComponent.text = "ReSelect Elevated Plane";
                        }
                    }
                }
                else
                {
                    List<ARRaycastHit> hits = new List<ARRaycastHit>();
                    raycastManager.Raycast(Input.GetTouch(0).position, hits);
                    if (hits.Count > 0)
                    {
                        //Vector3 v = objectToPlace.transform.localScale * Vector3.Distance(hits[0].pose.position, Vector3.zero);
                        //currentObject = GameObject.Instantiate(objectToPlace, hits[0].pose.position, Quaternion.identity);
                        currentObject = GameObject.Instantiate(objectToPlace, transform.position, Quaternion.identity);
                        //currentObject.transform.localScale = v;
                        editMode = true;
                        displayTextComponent.text = "Click Check to Finish Editing.";
                    }
                }
            }
        }
        else {

        }
    }

    void StartPlayMode() {
        playMode = true;
        cursorActive = false;
        cursorChildObject.SetActive(false);
        displayTextComponent.text = "In Play Mode";
        editMode = false;
        var t = ChangeMode.gameObject.transform.GetChild(0);
        if (t)
            t.GetComponent<Text>().text = "Start Build Mode";
        else {
            Debug.Log("NikunjChangeMode Error");
        }
        ChangeMode.onClick.AddListener(StartBuildMode);
        changeButtonStatus(false);
        currentObject = null;

        golfBall.SendMessage("StartGame", groundPlaneHeight);
    }

    void StartBuildMode()
    {
        playMode = false;
        cursorActive = true;
        cursorChildObject.SetActive(true);
        displayTextComponent.text = "Tap to Mark Real World Objects";
        editMode = false;
        var t = ChangeMode.gameObject.transform.GetChild(0);
        if (t)
            t.GetComponent<Text>().text = "Start Play Mode";
        else
        {
            Debug.Log("NikunjChangeMode Error");
        }
        ChangeMode.onClick.AddListener(StartPlayMode);
        changeButtonStatus(true);
        golfBall.SendMessage("EnterBuildGame");
    }

    void updateCursor() {
        Vector2 screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        if (buildPlaneMode)
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

            if (hits.Count > 0)
            {
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;
            }
        }
        else {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            raycastManager.Raycast(screenPosition, hits);
            if (hits.Count > 0)
            {
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;
            }
        }
    }

    private void DisableNewPlanes(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
            if(!selectedPlanes.Contains(plane.trackableId))
                plane.gameObject.SetActive(false);
        }
    }

    private void SetAllPlanesActive(bool value) {
        foreach (var plane in planeManager.trackables) {
            if (!selectedPlanes.Contains(plane.trackableId))
                plane.gameObject.SetActive(value);
        }
    }
}
