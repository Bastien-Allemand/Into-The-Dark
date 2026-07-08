using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CamerasScript : MonoBehaviour
{
    public static CamerasScript instance { get; private set; }

    [SerializeField] private TextMeshProUGUI m_cameraUiText;
    [SerializeField] private RenderTexture m_screenRenderTexture;
    [SerializeField] private int m_CamAmount;
    [SerializeField] private PhoneController phoneStateScript;

    private List<Camera> m_cameras = new List<Camera>();
    private bool m_onCamera = false;
    private int m_currentCamera = 0;
    public Camera camActive;
    

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    private void Update()
    {
        if (phoneStateScript != null && phoneStateScript.GetCurrentPhoneState() != PhoneState.Camera)
        {
            DisableAllCam();
        }

        GetItemCamera();
        RemoveItemCamera();
    }

    public void SetCameraViewActive(bool active)
    {
        m_onCamera = active;
        UpdateCameraDisplay();
    }

    public void NextCamera()
    {
        if (!m_onCamera || m_cameras.Count == 0) return;

        m_currentCamera++;
        if (m_currentCamera >= m_cameras.Count)
        {
            m_currentCamera = 0;
        }
        UpdateCameraDisplay();
    }

    public void PreviousCamera()
    {
        if (!m_onCamera || m_cameras.Count == 0) return;

        m_currentCamera--;
        if (m_currentCamera < 0)
        {
            m_currentCamera = m_cameras.Count - 1;
        }

        UpdateCameraDisplay();
    }

    private void UpdateCameraDisplay()
    {

        if (m_cameras == null || m_cameras.Count == 0)
        {
            camActive = null;
            return;
        }

        for (int i = 0; i < m_cameras.Count; i++)
        {
            bool isTarget = (m_onCamera && i == m_currentCamera);

            m_cameras[i].enabled = isTarget;
            m_cameras[i].GetComponentInParent<ItemScript>().usingEnergy = isTarget;
            m_cameras[i].targetTexture = isTarget ? m_screenRenderTexture : null;
        }

        if (m_cameraUiText != null)
        {
            m_cameraUiText.gameObject.SetActive(m_onCamera);
            if (m_onCamera && m_cameras.Count > 0)
                m_cameraUiText.text = "CAM " + (m_currentCamera + 1);
        }

        camActive = m_cameras[m_currentCamera];
    }

    void LateUpdate()
    {
        if (m_onCamera && m_cameras.Count > m_currentCamera)
        {
            
            if (m_cameras[m_currentCamera].enabled)
            {
                m_cameras[m_currentCamera].Render();
            }
        }
    }

    void DisableAllCam()
    {
        for(int i = 0; i < m_cameras.Count; i++)
        {
            m_cameras [i].enabled = false;
        }
        camActive = null;
        m_onCamera = false;

    }

    void GetItemCamera()
    {
        GameObject[] allItems = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in allItems)
        {
            Camera childCam = item.GetComponentInChildren<Camera>();
            ItemScript itemscript = item.GetComponent<ItemScript>();

            if (childCam != null && itemscript.deployed == true && item.layer == 17)
            {
                int nbCam = m_cameras.Count;

                if(nbCam <= 0)
                {
                    m_cameras.Add(childCam);
                    childCam.enabled = false;
                    childCam.targetTexture = null;
                    m_CamAmount++;
                }
                else
                {
                    for (int i = 0; i < nbCam; i++)
                    {
                        if (m_cameras[i] != childCam)
                        {
                            m_cameras.Add(childCam);
                            childCam.enabled = false;
                            childCam.targetTexture = null;
                            m_CamAmount++;
                        }
                    }
                }
                    
            }
        }
    }

    void RemoveItemCamera()
    {
        GameObject[] allItems = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in allItems)
        {
            Camera childCam = item.GetComponentInChildren<Camera>();
            ItemScript itemscript = item.GetComponent<ItemScript>();

            if (childCam != null && itemscript.deployed == false && item.layer == 17)
            {
                
                int nbCam = m_cameras.Count;
                for (int i = 0; i < nbCam; i++)
                {
                    if (m_cameras[i] == childCam)
                    {
                        Debug.Log("removed");
                        m_cameras.Remove(childCam);
                        childCam.enabled = false;
                        childCam.targetTexture = null;
                        m_CamAmount--;
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        Debug.Log("PhoneStateScrip DISABLED");
    }

    private void OnEnable()
    {
        Debug.Log("PhoneStateScrip ENABLED");
    }
}
