using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CamerasScript : MonoBehaviour
{

    public static CamerasScript instance;

    [SerializeField] public int m_camAmount = 0;
    [SerializeField] private TextMeshProUGUI m_cameraUiText;

    [SerializeField] private RenderTexture m_screenRenderTexture;

    private List<Camera> m_cameras = new List<Camera>();
    private bool m_onCamera = false;
    private int m_currentCamera = 0;

   
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        if (m_cameraUiText != null)
            m_cameraUiText.gameObject.SetActive(false);

        GameObject[] allItems = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in allItems)
        {
            Camera childCam = item.GetComponentInChildren<Camera>();

            if (childCam != null)
            {
                AddCamera(childCam.gameObject);
            }
        }
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
        // Switch between cameras
        if (m_onCamera)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                m_currentCamera--;
                if (m_currentCamera < 0) m_currentCamera = m_cameras.Count - 1;
                UpdateCameraDisplay();
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                m_currentCamera++;
                if (m_currentCamera >= m_cameras.Count) m_currentCamera = 0;
                UpdateCameraDisplay();
            }
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
}
