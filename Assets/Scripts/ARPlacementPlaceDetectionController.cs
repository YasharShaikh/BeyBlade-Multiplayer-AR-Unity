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
    private ARPlaneManager arPlaneManager;
    private ARPlacementManager arPlacementManager;

    private void Awake()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
        arPlacementManager = GetComponent<ARPlacementManager>();

        if (arPlaneManager == null)
            Debug.LogError("ARPlaneManager component missing!");

        if (arPlacementManager == null)
            Debug.LogError("ARPlacementManager component missing!");
    }

    private void Start()
    {
        btn_place.SetActive(true);
        btn_adjust.SetActive(false);
        btn_searchGame.SetActive(true);
        scaleSlider.SetActive(true);
        text_informPanel.text = "Move phone to detect plane surface.";
    }

    public void DisableARPlacementPlaneDetection()
    {
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
