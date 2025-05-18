using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacementManager : MonoBehaviour
{
    [SerializeField] private GameObject arena;

    private ARRaycastManager raycastManager;
    private static List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private Camera mainCamera;
    private Vector2 screenCenter;
    private bool isArenaPlaced = false;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        if (arena != null)
            arena.SetActive(false); // Hide until placed
    }

    private void Update()
    {
        if (isArenaPlaced)
            return;

        Ray ray = mainCamera.ScreenPointToRay(screenCenter);

        if (raycastManager.Raycast(ray, raycastHits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = raycastHits[0].pose;

            if (arena != null)
            {
                arena.transform.position = hitPose.position;
                arena.SetActive(true);
                isArenaPlaced = true;
            }
            else
            {
                Debug.LogWarning("Arena GameObject is not assigned in ARPlacementManager.");
            }
        }
    }
}
