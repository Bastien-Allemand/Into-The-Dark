using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CamerasScript : MonoBehaviour
{
    public static CamerasScript instance { get; private set; }

    [SerializeField] private TextMeshProUGUI m_cameraUiText;
    [SerializeField] private RenderTexture m_screenRenderTexture;

    private List<Camera> m_cameras = new List<Camera>();
    private bool m_onCamera = false;
    private int m_currentCamera = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameObject[] allItems = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in allItems)
        {
            Camera childCam = item.GetComponentInChildren<Camera>();
            if (childCam != null)
            {
                m_cameras.Add(childCam);
                childCam.enabled = false;
                childCam.targetTexture = null;
            }
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
        m_currentCamera = (m_currentCamera - 1 + m_cameras.Count) % m_cameras.Count;
        UpdateCameraDisplay();
    }

    private void UpdateCameraDisplay()
    {
        for (int i = 0; i < m_cameras.Count; i++)
        {
            bool isTarget = (m_onCamera && i == m_currentCamera);

            m_cameras[i].enabled = isTarget;
            m_cameras[i].targetTexture = isTarget ? m_screenRenderTexture : null;
        }

        if (m_cameraUiText != null)
        {
            m_cameraUiText.gameObject.SetActive(m_onCamera);
            if (m_onCamera && m_cameras.Count > 0)
                m_cameraUiText.text = "CAM " + (m_currentCamera + 1);
        }
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
}
