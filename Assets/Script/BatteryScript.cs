using UnityEngine;
using UnityEngine.UI;

public class BatteryScript : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public void SetMaxBattery(int battery)
    {
        slider.maxValue = battery;
        slider.value = battery;

        fill.color = gradient.Evaluate(1f);
    }
    public void SetBattery(int battery)
    {
        slider.value = battery;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    void Update()
    {
        
    }
}
