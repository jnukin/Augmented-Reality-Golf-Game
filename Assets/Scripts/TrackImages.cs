using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class TrackImages : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;
    public GameObject[] ArPrefabs;
    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> createdPrefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, bool> prefabPlacement = new Dictionary<string, bool>();
    public Text placementDisplay;
    private float globalMin = 10000f;
    private float minRequiredPlacementDistance = 0.3f;

    private void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        foreach (GameObject prefab in ArPrefabs) {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);
            newPrefab.SetActive(false);
        }
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            //spawnedPrefabs[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage) {

        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;
        Quaternion rotation = trackedImage.transform.rotation;

        GameObject prefab = spawnedPrefabs[name];
        prefab.transform.position = position;
        prefab.transform.rotation = rotation;
        createdPrefabs[name] = prefab;
        prefab.SetActive(true);

        bool curr = true;
        float minDist = 10000f;
        foreach (GameObject go in createdPrefabs.Values)
        {
            if (name != go.name) {
                float d = Vector3.Distance(prefab.transform.position, go.transform.position);
                minDist = Mathf.Min(d, minDist);
                curr &= prefabPlacement[go.name];
            }
        }

        if (minDist >= minRequiredPlacementDistance)
        {
            prefabPlacement[name] = true;
            curr &= true;
        }
        else {
            prefabPlacement[name] = false;
            curr &= false;
        }

        if (curr)
        {
            placementDisplay.text = "Placement of Tracked Images : Good";
        }
        else {
            placementDisplay.text = "Placement of Tracked Images : Bad";
        }
    }
}
