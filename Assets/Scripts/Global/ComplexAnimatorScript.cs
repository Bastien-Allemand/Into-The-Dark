using UnityEngine;

public class ComplexAnimatorScript : MonoBehaviour
{
    private Animator animator;
    private bool isOpen;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        isOpen = !isOpen;

        animator.SetBool("IsOpen", isOpen);

        Debug.Log($"IsOpen = {isOpen}");
    }
}