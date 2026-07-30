using UnityEngine;

public class DisplayThrowMeter : MonoBehaviour
{
    GameManager.NightData? night = null;
    [SerializeField] private GameObject throwMeterUI;
     private DropScript dropScript;
    private float maxlength = 125f;
    private Vector3 initialScale;
    bool InGame = false;

    private void Awake()
    {
        initialScale = throwMeterUI.transform.localScale;
        if (!night.HasValue)
            return;
        InGame = true;
        dropScript = night.Value.nightGO.GetComponent<DropScript>();
    }

    private void Update()
    {
        if (!InGame)
            return;

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
    public void ActivateThrowUI(GameManager.NightData nightSelection)
    {
        night = nightSelection;
        Awake();
    }
}
