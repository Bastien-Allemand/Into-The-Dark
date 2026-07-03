using UnityEngine;

public class ComplexAnimatorScript : MonoBehaviour
{
    private Animator animator;
    private bool isOpen;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError($"[{name}] Aucun Animator trouvé !");
        }
        else
        {
            Debug.Log($"[{name}] Animator trouvé : {animator.runtimeAnimatorController.name}");
        }
    }

    public void Interact()
    {
        Debug.Log($"[{name}] Interact appelé");

        if (animator == null)
        {
            Debug.LogError($"[{name}] Animator NULL");
            return;
        }

        isOpen = !isOpen;

        Debug.Log($"[{name}] IsOpen = {isOpen}");

        animator.SetBool("IsOpen", isOpen);

        if (isOpen)
        {
            Debug.Log($"[{name}] Ouverture");

            animator.speed = 1f;
        }
        else
        {
            Debug.Log($"[{name}] Fermeture");

            animator.speed = -1f;

            animator.Play("OpenAnim", 0, 1f);
        }

        Debug.Log($"[{name}] Etat actuel : {animator.GetCurrentAnimatorStateInfo(0).shortNameHash}");
    }
}