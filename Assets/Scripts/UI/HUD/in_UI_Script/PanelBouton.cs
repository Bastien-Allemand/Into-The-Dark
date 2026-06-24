using UnityEngine;

public class PanelBouton : MonoBehaviour
{
    [Header("target doit etre enfant de panel")]
    [SerializeField] Transform panel;
    [SerializeField] Transform target;

    public void TargetShowOnly()
    {
        if (!panel.Find(target.name))
        {
            Debug.Log("Target is not direct child of Panel");
            return;
        }
        foreach (Transform child in panel)
        {
            child.gameObject.SetActive(false);
        }
        target.gameObject.SetActive(true);
    }
}
