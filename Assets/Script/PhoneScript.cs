using UnityEngine;

public class PhoneScript : MonoBehaviour
{
    public GameObject phone;
    public Light phoneLight;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            phone.SetActive(!phone.activeSelf);

            if (!phone.activeSelf)
            {
                phoneLight.enabled = false;
            }
        }

        if (Input.GetMouseButtonDown(0) && phone.activeSelf)
        {
            phoneLight.enabled = !phoneLight.enabled;
        }
    }
}