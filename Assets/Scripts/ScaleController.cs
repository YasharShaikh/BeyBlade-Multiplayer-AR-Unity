using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ScaleController : MonoBehaviour
{
    [SerializeField] private Slider sliderScale;

    private XROrigin xrOrigin;

    private void Awake()
    {
        xrOrigin = GetComponent<XROrigin>();

        if (xrOrigin == null)
            Debug.LogError("XROrigin not found on this GameObject.");
    }

    private void Start()
    {
        if (sliderScale != null)
        {
            sliderScale.onValueChanged.AddListener(OnSliderValueChange);
            OnSliderValueChange(sliderScale.value); 
        }
        else
        {
            Debug.LogError("Slider reference is missing.");
        }
    }

    public void OnSliderValueChange(float value)
    {
        if (xrOrigin != null && value > 0f)
        {
            xrOrigin.transform.localScale = Vector3.one * value;
        }
    }
}
