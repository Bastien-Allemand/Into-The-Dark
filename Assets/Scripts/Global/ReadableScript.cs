using UnityEngine;
using System.Collections;

public class Readable : MonoBehaviour
{
    [Header("Reading Position")]
    [SerializeField] private Vector3 readLocalPosition = new Vector3(0f, -0.1f, 0.5f);
    [SerializeField] private Vector3 readLocalRotation = Vector3.zero;

    [Header("Animation")]
    [SerializeField] private float animationDuration = 0.5f;


    private Transform originalParent;
    private Vector3 originalWorldPosition;
    private Quaternion originalWorldRotation;

    private bool isInAnimation = false;

    private bool isReading = false;

    public bool IsReading => isReading;
    public bool IsInAnimation => isInAnimation;

    private Coroutine currentAnimation;

    private Camera playerCamera;

    private void Start()
    {
        playerCamera = GetActivePlayerCamera();

        originalParent = transform.parent;
        originalWorldPosition = transform.position;
        originalWorldRotation = transform.rotation;
    }

    private Camera GetActivePlayerCamera()
    {
        GameObject nightManager = GameObject.Find("Night Manager");

        if (nightManager == null)
        {
            Debug.LogError("Night Manager Doesn't Exist.");
            return null;
        }

        foreach (Transform child in nightManager.transform)
        {
            if (child.gameObject.activeSelf)
            {
                Camera cam = child.GetComponentInChildren<Camera>();

                if (cam != null)
                    return cam;
            }
        }
        return null;
    }

    public void PlaceObjectInFOV()
    {
        if (isInAnimation)
            return;

        if (isReading)
        {
            StopReading();
        }
        else
        {
            StartReading();
        }
    }

    private void StartReading()
    {
        if (isInAnimation) return;

        isReading = true;

        Debug.Log("START READING");

        GetComponent<Collider>().enabled = false;

        originalParent = transform.parent;
        originalWorldPosition = transform.position;
        originalWorldRotation = transform.rotation;

        transform.SetParent(playerCamera.transform, true);

        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(
            AnimateLocalTransform(
                readLocalPosition,
                Quaternion.Euler(readLocalRotation)
            )
        );
    }

    private void StopReading()
    {
        if (isInAnimation) return;

        isReading = false;

        Debug.Log("STOP READING");

        GetComponent<Collider>().enabled = true;

        currentAnimation = StartCoroutine(ReturnToOrigin());
    }

    private IEnumerator AnimateLocalTransform(Vector3 targetPos, Quaternion targetRot)
    {
        isInAnimation = true;

        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / animationDuration;

            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        transform.localPosition = targetPos;
        transform.localRotation = targetRot;

        isInAnimation = false;
    }

    private IEnumerator ReturnToOrigin()
    {
        isInAnimation = true;

        transform.SetParent(originalParent, true);

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / animationDuration;

            transform.position = Vector3.Lerp(startPos, originalWorldPosition, t);
            transform.rotation = Quaternion.Slerp(startRot, originalWorldRotation, t);

            yield return null;
        }

        transform.position = originalWorldPosition;
        transform.rotation = originalWorldRotation;

        isInAnimation = false;
    }
}