using UnityEngine;
using System.Collections;

public class BasicAnimationScript : MonoBehaviour
{
    [Header("Position")]
    public Vector3 openOffset = new Vector3(0, 0, 0);

    [Header("Rotation")]
    public Vector3 openRotation = new Vector3(0, 0, 0);

    [Header("Animation")]
    public float animationDuration = 0.5f;

    private Vector3 closedPos;
    private Vector3 openedPos;

    private Quaternion closedRot;
    private Quaternion openedRot;

    private bool isOpen;
    private Coroutine currentAnimation;

    private void Start()
    {
        closedPos = transform.localPosition;
        openedPos = closedPos + openOffset;

        closedRot = transform.localRotation;
        openedRot = closedRot * Quaternion.Euler(openRotation);
    }

    public void Interact()
    {

        Debug.Log($"Animation {gameObject.name} Launch");

        isOpen = !isOpen;

        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(
            AnimateDrawer(
                isOpen ? openedPos : closedPos,
                isOpen ? openedRot : closedRot
            )
        );
    }

    private IEnumerator AnimateDrawer(Vector3 targetPos, Quaternion targetRot)
    {
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / animationDuration;

            transform.localPosition = Vector3.Lerp(
                startPos,
                targetPos,
                t
            );

            transform.localRotation = Quaternion.Slerp(
                startRot,
                targetRot,
                t
            );

            yield return null;
        }

        transform.localPosition = targetPos;
        transform.localRotation = targetRot;
    }
}