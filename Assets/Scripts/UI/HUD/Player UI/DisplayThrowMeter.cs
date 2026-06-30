using UnityEngine;

public class DisplayThrowMeter : MonoBehaviour
{
    [SerializeField] private GameObject throwMeterUI;
    [SerializeField] private DropScript dropScript;
    private float maxlength = 125f;
    private Vector3 initialScale;

    private void Start()
    {
        initialScale = throwMeterUI.transform.localScale;
    }

    private void Update()
    {
        if (dropScript.chargingLeft || dropScript.chargingRight)
        {
            throwMeterUI.SetActive(true);

            float charge = dropScript.chargingLeft ? dropScript.leftCharge : dropScript.rightCharge;
            float fillAmount = Mathf.Clamp01(charge / dropScript.maxForce);

            throwMeterUI.transform.localScale = new Vector3(
                initialScale.x * fillAmount,
                initialScale.y,
                initialScale.z
            );
        }
        else
        {
            throwMeterUI.SetActive(false);
        }
    }
}
