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

    void Start()
    {
        if (m_cameraUiText != null)
            m_cameraUiText.gameObject.SetActive(false);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    if (m_cameras.Count < m_maxCameras)
        //    {
        //        Debug.Log("grgdf");

        //        GameObject newCameraObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //        //name of the camera
        //        newCameraObj.name = "CreatedCamera_" + (m_cameras.Count + 1);

        //        Camera newCam = newCameraObj.AddComponent<Camera>();

        //        //component of the camera
        //        Rigidbody rb = newCameraObj.AddComponent<Rigidbody>();
        //        rb.useGravity = true;
        //        rb.isKinematic = false;

        //        newCameraObj.transform.position = new Vector3(0f, 4f, -10f + m_testValue);
        //        m_testValue += 1;

        //        newCam.enabled = false;

        //        m_cameras.Add(newCam);
        //    }
        //}

        // Toggle camera display
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (m_cameras.Count > 0)
            {
                m_onCamera = !m_onCamera;
                UpdateCameraDisplay();
            }
        }
        // Switch between cameras
        if (m_onCamera)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                m_currentCamera--;
                if (m_currentCamera < 0) m_currentCamera = m_cameras.Count - 1;
                UpdateCameraDisplay();
            }

            if (Input.GetKeyDown(KeyCode.D))
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
