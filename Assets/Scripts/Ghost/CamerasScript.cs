using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CamerasScript : MonoBehaviour
{
    [SerializeField] public int m_camAmount = 0;
    [SerializeField] private TextMeshProUGUI m_cameraUiText;

    private List<Camera> m_cameras = new List<Camera>();
    private bool m_onCamera = false;
    private int m_currentCamera = 0;

    [SerializeField] private float maxBattery = 100f;
    [SerializeField] private float batteryRemaining;

    void Start()
    {
        if (m_cameraUiText != null)
            m_cameraUiText.gameObject.SetActive(false);

        batteryRemaining = maxBattery;
    }

    void AddCamera(GameObject camera)
    {
        Camera cam = camera.GetComponent<Camera>();
        if (cam != null && !m_cameras.Contains(cam))
        {
            m_cameras.Add(cam);
            m_camAmount++;
        }
    }

    void Update()
    {
        if (!m_onCamera)
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_currentCamera--;
            if (m_currentCamera < 0)
                m_currentCamera = m_cameras.Count - 1;

            UpdateCameraDisplay();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            m_currentCamera++;
            if (m_currentCamera >= m_cameras.Count)
                m_currentCamera = 0;

            UpdateCameraDisplay();
        }
    }

    // Update the camera display based on the current state
    private void UpdateCameraDisplay()
    {
        for (int i = 0; i < m_cameras.Count; i++)
        {
            m_cameras[i].enabled = (m_onCamera && i == m_currentCamera);
        }

        if (m_cameraUiText != null)
        {
            if (m_onCamera && m_cameras.Count > 0)
            {
                m_cameraUiText.gameObject.SetActive(true);
                m_cameraUiText.text = m_cameras[m_currentCamera].gameObject.name;
            }
            else
            {
                m_cameraUiText.gameObject.SetActive(false);
            }
        }
    }
    public void OpenCameraView()
    {
        if (m_cameras.Count <= 0)
            return;

        m_onCamera = true;
        UpdateCameraDisplay();
    }

    public void CloseCameraView()
    {
        m_onCamera = false;
        UpdateCameraDisplay();
    }
}