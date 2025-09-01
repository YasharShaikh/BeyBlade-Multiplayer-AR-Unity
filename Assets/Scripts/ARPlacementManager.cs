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

    [Header("Testing Mode")]
    [SerializeField] private bool disableARPlacement = true; // ✅ Toggle this in Inspector

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
        {
            if (disableARPlacement)
            {
                // ✅ For testing, spawn arena at default position
                arena.transform.position = new Vector3(0, 0, 2f); // 2m in front of world origin
                arena.SetActive(true);
                isArenaPlaced = true;
                Debug.Log("[ARPlacementManager] Arena spawned in testing mode.");
            }
            else
            {
                arena.SetActive(false); // Hide until AR places it
            }
        }
    }

    private void Update()
    {
        if (disableARPlacement) return; // ✅ Skip AR entirely in testing

        if (isArenaPlaced || raycastManager == null || arCamera == null)
            return;

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
