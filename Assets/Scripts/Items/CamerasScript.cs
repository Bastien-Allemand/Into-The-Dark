using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CamerasScript : MonoBehaviour
{
    public static CamerasScript instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI m_cameraUiText;
    [SerializeField] private RenderTexture m_screenRenderTexture;

    [Header("Controller Reference")]
    [SerializeField] private PhoneController phoneStateScript;

    private List<Camera> m_cameras = new List<Camera>();
    private bool m_onCamera = false;
    [SerializeField] private int mCamAmount = 0;
    [SerializeField] private int m_currentCamera = 0;

    [HideInInspector] public Camera camActive;

    private void Awake()
    {
        instance = this;

        ClearScreenToBlack();
    }

    private void OnEnable()
    {
        ItemScript.OnCameraDeployed += RegisterCamera;
        ItemScript.OnCameraRemoved += UnregisterCamera;

        if (phoneStateScript != null)
        {
            phoneStateScript.OnPhoneStateChanged += HandlePhoneStateChanged;
            HandlePhoneStateChanged(phoneStateScript.GetCurrentPhoneState());
        }
    }

    private void OnDisable()
    {
        ItemScript.OnCameraDeployed -= RegisterCamera;
        ItemScript.OnCameraRemoved -= UnregisterCamera;

        if (phoneStateScript != null)
        {
            phoneStateScript.OnPhoneStateChanged -= HandlePhoneStateChanged;
        }
    }

    private void HandlePhoneStateChanged(PhoneState newState)
    {
        bool isLookingAtTabs = (newState == PhoneState.Camera);

        if (isLookingAtTabs)
        {
            if (m_cameras == null || m_cameras.Count == 0)
            {
                DisableAllCam();
            }
            else
            {
                SetCameraViewActive(true);
            }
        }
        else
        {
            DisableAllCam();
        }
    }
    private void Update()
    {
    }

    private void RegisterCamera(Camera camera)
    {
        if (camera == null) return;

        if (!m_cameras.Contains(camera))
        {
            m_cameras.Add(camera);
            camera.enabled = false;
            camera.targetTexture = null;
            if (m_onCamera) UpdateCameraDisplay();
            mCamAmount++;
        }
    }

    private void UnregisterCamera(Camera camera)
    {
        if (camera == null) return;

        if (m_cameras.Contains(camera))
        {
            camera.enabled = false;
            camera.targetTexture = null;
            ItemScript item = camera.GetComponentInParent<ItemScript>();
            if (item != null)
            {
                item.usingEnergy = false;
            }

            m_cameras.Remove(camera);
           

            if (m_currentCamera >= m_cameras.Count)
            {
                m_currentCamera = Mathf.Max(0, m_cameras.Count - 1);
            }
            mCamAmount--;
            Debug.Log("removed");
            UpdateCameraDisplay();
        }
    }

    public void SetCameraViewActive(bool active)
    {
        m_onCamera = active;
        UpdateCameraDisplay();
    }

    public void NextCamera()
    {
        if (!m_onCamera || m_cameras.Count == 0) return;

        m_currentCamera = (m_currentCamera + 1) % m_cameras.Count;

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
            if (m_cameraUiText != null) m_cameraUiText.gameObject.SetActive(false);
            ClearScreenToBlack();
            return;
        }

        for (int i = 0; i < m_cameras.Count; i++)
        {
            if (m_cameras[i] == null) continue;

            bool isTarget = (m_onCamera && i == m_currentCamera);
            ItemScript itemscript = m_cameras[i].GetComponentInParent<ItemScript>();
            if (isTarget)
            {
                m_cameras[i].targetTexture = m_screenRenderTexture;
                m_cameras[i].enabled = true;
            }
            else
            {
                m_cameras[i].enabled = false;
                m_cameras[i].targetTexture = null;
            }


            if (itemscript != null)
            {
                itemscript.usingEnergy = isTarget;
            }
        }

        if (m_cameraUiText != null)
        {
            m_cameraUiText.gameObject.SetActive(m_onCamera);
            if (m_onCamera && m_currentCamera < m_cameras.Count)
            {
                m_cameraUiText.text = "CAM " + (m_currentCamera + 1);
            }
        }

        if (m_currentCamera < m_cameras.Count)
        {
            camActive = m_cameras[m_currentCamera];
        }
    }

    private void LateUpdate()
    {
        if (m_onCamera && m_currentCamera < m_cameras.Count)
        {
            if (m_cameras[m_currentCamera] != null && m_cameras[m_currentCamera].enabled)
            {
                m_cameras[m_currentCamera].Render();
            }
        }
    }

    private void DisableAllCam()
    {
        m_onCamera = false;

        for (int i = 0; i < m_cameras.Count; i++)
        {
            if (m_cameras[i] != null)
            {
                m_cameras[i].enabled = false;
                m_cameras[i].targetTexture = null;

                ItemScript item = m_cameras[i].GetComponentInParent<ItemScript>();
                if (item != null) item.usingEnergy = false;
            }
        }
        camActive = null;
        if (m_cameraUiText != null) m_cameraUiText.gameObject.SetActive(false);
        ClearScreenToBlack();
    }

    private void ClearScreenToBlack()
    {
        if (m_screenRenderTexture != null)
        {
            RenderTexture activeBuffer = RenderTexture.active;
            RenderTexture.active = m_screenRenderTexture;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = activeBuffer;
        }
    }
}