using UnityEngine;

public class SetUIiToPlayer : MonoBehaviour
{
    [SerializeField] public GameObject UIhud;

    public void ActivateUI(GameManager.NightData game)
    {
        GetComponent<DisplayUI>().ActivateUI(game.characterOfTheNight);
        UIhud.GetComponentInChildren<InsaneBarScript>().ActivateUI(game);
        UIhud.GetComponentInChildren<DisplayThrowMeter>().ActivateThrowUI(game);
        UIhud.GetComponentInChildren<ConsumeScript>().ActivateConsumeScriptUI(game);
        UIhud.GetComponentInChildren<DisplayID>().ActivateUI(game.characterOfTheNight);
    }
}
