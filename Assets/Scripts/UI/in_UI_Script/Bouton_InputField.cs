using TMPro;
using UnityEngine;

public class Bouton_InputField : MonoBehaviour
{
    public TMP_InputField inputField;

    public void Valider()
    {
        string texte = inputField.text;
        Debug.Log("Texte saisi : " + texte);
    }
}
