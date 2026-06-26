using UnityEngine;

public class PhoneLightScript : MonoBehaviour
{
    [SerializeField] private Light phoneLight;
    [SerializeField] private PhoneStateScrip phoneStateScript;
    [SerializeField] private UIBattery uiBattery;

    [SerializeField] private Material fogMaterial;

    void Start()
    {
        
    }

    void Update()
    {
        HandleLight();
        FlashLightShaderUpdate();
    }

    private void HandleLight()
    {
        if (Input.GetMouseButtonDown(0) && phoneStateScript.GetCurrentPhoneState() != PhoneState.Hidden && uiBattery.HaveBattery)
        {
            phoneLight.enabled = !phoneLight.enabled;
        }
    }

    private void FlashLightShaderUpdate()
    {
        if (fogMaterial == null || phoneLight == null)
            return;
        fogMaterial.SetVector("_FlashlightPos", phoneLight.transform.position);
        fogMaterial.SetVector("_FlashlightDir", phoneLight.transform.forward);
    }
}
