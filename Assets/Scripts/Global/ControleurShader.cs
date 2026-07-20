using UnityEngine;

public class ControleurShader : MonoBehaviour
{
    private const string KEYWORD_SHADER = "_VISION_NOCTURNE_ON";

    private bool estActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            ToggleShader();
        }
    }

    public void ToggleShader()
    {
        estActive = !estActive;

        if (estActive)
        {
            Shader.EnableKeyword(KEYWORD_SHADER);
            Debug.Log("Shader Activé");
        }
        else
        {
            // Désactive l'effet globalement
            Shader.DisableKeyword(KEYWORD_SHADER);
            Debug.Log("Shader Désactivé");
        }
    }

    void OnDisable()
    {
        Shader.DisableKeyword(KEYWORD_SHADER);
    }
}