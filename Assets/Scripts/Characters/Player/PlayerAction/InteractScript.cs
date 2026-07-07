using UnityEngine;

public class Interact : MonoBehaviour
{
    private PlayerAction controls;

    [Header("References")]
    [SerializeField] private Camera playerCamera;

    [Header("Interaction")]
    [SerializeField] private float interactDistance = 5f;

    private bool wasInteractingLastFrame;

    private Readable currentReadObject;

    private void Awake()
    {
        controls = InputManager.controls;
    }

    private void Update()
    {
        float interactValue = controls.PlayerInteraction.Interact.ReadValue<float>();
        bool isInteractingThisFrame = Mathf.Abs(interactValue) > 0.5f;

        if (isInteractingThisFrame && !wasInteractingLastFrame)
        {
            InteractWithObject();
        }

        wasInteractingLastFrame = isInteractingThisFrame;
    }

    private void InteractWithObject()
    {
        if (currentReadObject != null)
        {
            if (currentReadObject.IsInAnimation)
                return;

            currentReadObject.PlaceObjectInFOV();

            if (!currentReadObject.IsReading)
                currentReadObject = null;

            return;
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            return;

        BasicAnimationScript lootItem = hit.collider.GetComponentInParent<BasicAnimationScript>();

        if (lootItem != null)
        {
            lootItem.Interact();
            return;
        }

        Readable readable = hit.collider.GetComponentInParent<Readable>();

        if (readable != null)
        {
            readable.PlaceObjectInFOV();
            currentReadObject = readable;
            return;
        }


    }
}