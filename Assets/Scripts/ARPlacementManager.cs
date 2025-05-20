using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacementManager : MonoBehaviour
{
    [SerializeField] private GameObject arena;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private Camera arCamera;

    private static List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private bool isArenaPlaced = false;

    private void Awake()
    {
        if (raycastManager == null)
            raycastManager = FindAnyObjectByType<ARRaycastManager>();

        if (arCamera == null)
            arCamera = Camera.main;
    }

    private void Start()
    {
        if (arena != null)
            arena.SetActive(false); // Hide until placed
    }

    private void Update()
    {
        if (isArenaPlaced || raycastManager == null || arCamera == null)
            return;
            
        if(raycastManager == null)
        {
            Debug.Log("[ARPlacementManager] raycastManager is null");
        }
        if(arCamera == null)
        {
            Debug.Log("[ARPlacementManager] arCamera is null ");
        }

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        if (raycastManager.Raycast(screenCenter, raycastHits, TrackableType.PlaneWithinPolygon))
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
