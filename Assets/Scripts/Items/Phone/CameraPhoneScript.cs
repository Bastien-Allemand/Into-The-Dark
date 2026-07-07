using UnityEngine;

public class CameraPhoneScript : MonoBehaviour
{
    [SerializeField] private PhoneController phoneStateScript;
    [SerializeField] private RectTransform phoneCanvaRectTransform;

    private void Awake()
    {
        phoneStateScript = GetComponent<PhoneController>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        DisplayCanvaCamera();
    }

    private void DisplayCanvaCamera()
    {
        if (phoneStateScript != null && phoneStateScript.GetCurrentPhoneState() == PhoneState.Camera)
        {
            phoneCanvaRectTransform.localPosition = new Vector3(-0.05f, -0.31f, -0.42f);
            phoneCanvaRectTransform.localRotation = Quaternion.Euler(0f, phoneCanvaRectTransform.localEulerAngles.y, -90f);
        }
        else
        {
            phoneCanvaRectTransform.localPosition = new Vector3(-0.05f, 0.47f, -0.365f);
            phoneCanvaRectTransform.localRotation = Quaternion.Euler(0f, phoneCanvaRectTransform.localEulerAngles.y, 0f);
        }
    }
}
