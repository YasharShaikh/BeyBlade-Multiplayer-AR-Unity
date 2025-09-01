using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARPlacementPlaceDetectionController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject btn_place;
    [SerializeField] private GameObject btn_adjust;
    [SerializeField] private GameObject btn_searchGame;
    [SerializeField] private GameObject scaleSlider;
    [SerializeField] private TextMeshProUGUI text_informPanel;
    [SerializeField] private TextMeshProUGUI statusText;

    private ARPlaneManager arPlaneManager;
    private ARPlacementManager arPlacementManager;

    [Header("Testing Mode")]
    [SerializeField] private bool disableARPlacement = true; // ✅ Toggle for testing

    private void Awake()
    {
        arPlaneManager = FindAnyObjectByType<ARPlaneManager>();
        arPlacementManager = FindFirstObjectByType<ARPlacementManager>();

        if (arPlaneManager == null)
            Debug.LogWarning("[ARPlacementPlaceDetectionController] ARPlaneManager component missing!");

        if (arPlacementManager == null)
            Debug.LogWarning("[ARPlacementPlaceDetectionController] ARPlacementManager component missing!");
    }

    private void Start()
    {
        btn_place.SetActive(true);
        btn_adjust.SetActive(false);
        btn_searchGame.SetActive(true);
        scaleSlider.SetActive(true);

        if (disableARPlacement)
        {
            text_informPanel.text = "Testing Mode: Arena spawned normally.";
            statusText.text = "AR disabled.";
        }
        else
        {
            text_informPanel.text = "Move phone to detect plane surface.";
        }
    }

    void Update()
    {
        if (disableARPlacement) return; // ✅ skip plane detection

        bool planeDetected = false;

        foreach (var plane in arPlaneManager.trackables)
        {
            if (plane.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                planeDetected = true;
                break;
            }
        }

        statusText.text = planeDetected ? "Plane Detected!" : "Searching for a plane...";
    }

    public void DisableARPlacementPlaneDetection()
    {
        if (disableARPlacement) return; // ✅ already disabled

        if (arPlaneManager != null)
            arPlaneManager.enabled = false;

        if (arPlacementManager != null)
            arPlacementManager.enabled = false;

        SetAllPlanesActive(false);

        scaleSlider.SetActive(false);
        btn_place.SetActive(false);
        btn_adjust.SetActive(true);
        btn_searchGame.SetActive(true);

        text_informPanel.text = "Search game for battle";
    }

    public void EnableARPlacementPlaneDetection()
    {
        if (disableARPlacement) return; // ✅ testing mode ignores

        if (arPlaneManager != null)
            arPlaneManager.enabled = true;

        if (arPlacementManager != null)
            arPlacementManager.enabled = true;

        SetAllPlanesActive(true);

        btn_place.SetActive(true);
        btn_adjust.SetActive(false);
        btn_searchGame.SetActive(false);
        SetAllPlanesActive(true);

        text_informPanel.text = "Move phone to detect plane surface.";
    }

    private void SetAllPlanesActive(bool active)
    {
        if (arPlaneManager != null)
        {
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(active);
            }
        }
    }
}
