using UnityEngine;

public class PhoneLightScript : MonoBehaviour
{
    [SerializeField] private Light phoneLight;
    [SerializeField] private PhoneController phoneController;
    [SerializeField] private UIBattery uiBattery;

    [SerializeField] private Material fogMaterial;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        phoneController.OnPhoneStateChanged += HandlePhoneStateChanged;
    }

    private void OnDisable()
    {
        phoneController.OnPhoneStateChanged -= HandlePhoneStateChanged;
    }

    void Update()
    {
        FlashLightShaderUpdate();
    }

    public void ToggleFlashlight()
    {
        if (phoneLight == null || uiBattery == null) return;

        if (phoneController.GetCurrentPhoneState() == PhoneState.Idle && uiBattery.HaveBattery)
        {
            phoneLight.enabled = !phoneLight.enabled;
        }
    }   
    
    private void HandlePhoneStateChanged(PhoneState newState)
    {
        if (newState != PhoneState.Idle && phoneLight != null)
        {
            phoneLight.enabled = false;
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
