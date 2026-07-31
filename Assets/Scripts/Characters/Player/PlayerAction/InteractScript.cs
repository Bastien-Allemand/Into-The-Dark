using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    private PlayerAction controls;

    [Header("References")]
    [SerializeField] private Camera playerCamera;

    [Header("Interaction Settings")]
    [SerializeField] private float interactDistance = 5f;
    [SerializeField] private LayerMask interactableLayers = ~0;

    [Header("UI Hand GameObjects")]
    [SerializeField] private GameObject leftHandUI;
    [SerializeField] private GameObject rightHandUI;

    [Header("UI Hand Components")]
    [SerializeField] private Image leftHandImageComponent;
    [SerializeField] private Image rightHandImageComponent;

    [Header("UI Sprites (Optionnel si déjà mis sur l'UI)")]
    [SerializeField] private Sprite leftHandOpened;
    [SerializeField] private Sprite rightHandOpened;
    [SerializeField] private Sprite leftHandClosed;
    [SerializeField] private Sprite rightHandClosed;

    [Header("Hand Content References")]
    [SerializeField] private HandContent leftHandContent;
    [SerializeField] private HandContent rightHandContent;

    private Readable currentReadObject;
    private bool isHoveringInteractive = false;

    private bool isHoldingLeft = false;
    private bool isHoldingRight = false;

    private void Awake()
    {
        controls = InputManager.controls;
    }

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // Récupération automatique de l'Image même si elle se trouve sur un objet enfant
        if (leftHandImageComponent == null && leftHandUI != null)
            leftHandImageComponent = leftHandUI.GetComponentInChildren<Image>(true);

        if (rightHandImageComponent == null && rightHandUI != null)
            rightHandImageComponent = rightHandUI.GetComponentInChildren<Image>(true);

        HideUIHands();
    }

    private void OnEnable()
    {
        if (controls != null)
        {
            controls.PlayerMoves.Interact.performed += OnInteractPressed;
            controls.PlayerMoves.Interact.canceled += OnInteractReleased;
        }
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.PlayerMoves.Interact.performed -= OnInteractPressed;
            controls.PlayerMoves.Interact.canceled -= OnInteractReleased;
        }

        isHoldingLeft = false;
        isHoldingRight = false;
        HideUIHands();
    }

    private void Update()
    {
        CheckHoverObject();
    }

    private void OnInteractPressed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        bool isLeft = ctx.control.name == "leftButton";
        bool isRight = ctx.control.name == "rightButton";

        if (isLeft) isHoldingLeft = true;
        else if (isRight) isHoldingRight = true;
        else { isHoldingLeft = true; isHoldingRight = true; }

        if (isHoveringInteractive)
        {
            SetHandsUIState(isHoldingLeft, isHoldingRight);
        }

        InteractWithObject(isLeft, isRight);
    }

    private void OnInteractReleased(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        if (ctx.control.name == "leftButton") isHoldingLeft = false;
        else if (ctx.control.name == "rightButton") isHoldingRight = false;
        else { isHoldingLeft = false; isHoldingRight = false; }

        if (isHoveringInteractive)
        {
            SetHandsUIState(isHoldingLeft, isHoldingRight);
        }
        else
        {
            HideUIHands();
        }
    }

    private bool LeftHandHasObject()
    {
        return leftHandContent != null && leftHandContent.filled;
    }

    private bool RightHandHasObject()
    {
        return rightHandContent != null && rightHandContent.filled;
    }

    private void CheckHoverObject()
    {
        if (playerCamera == null) return;

        if (currentReadObject != null)
        {
            if (isHoveringInteractive)
            {
                isHoveringInteractive = false;
                HideUIHands();
            }
            return;
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        bool lookingAtTarget = false;

        // Le QueryTriggerInteraction.Ignore évite de bloquer le Raycast sur votre propre joueur/triggers
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayers, QueryTriggerInteraction.Ignore))
        {
            bool isComplex = hit.collider.GetComponentInParent<ComplexAnimatorScript>() != null;
            bool isReadable = hit.collider.GetComponentInParent<Readable>() != null || hit.collider.CompareTag("Readable");
            bool isLootable = hit.collider.GetComponentInParent<BasicAnimationScript>() != null || hit.collider.CompareTag("Lootable");
            bool isGadget = hit.collider.CompareTag("Item") || (hit.collider.transform.parent != null && hit.collider.transform.parent.CompareTag("Item"));

            if (isComplex || isReadable || isLootable || isGadget)
            {
                lookingAtTarget = true;
            }
        }

        if (lookingAtTarget != isHoveringInteractive)
        {
            isHoveringInteractive = lookingAtTarget;

            if (isHoveringInteractive)
            {
                SetHandsUIState(isHoldingLeft, isHoldingRight);
            }
            else
            {
                HideUIHands();
            }
        }
    }

    private void SetHandsUIState(bool leftClosed, bool rightClosed)
    {
        // MAIN GAUCHE
        if (!LeftHandHasObject())
        {
            if (leftHandUI != null) leftHandUI.SetActive(true);
            if (leftHandImageComponent != null)
            {
                leftHandImageComponent.enabled = true;
                if (leftHandClosed != null && leftHandOpened != null)
                {
                    leftHandImageComponent.sprite = leftClosed ? leftHandClosed : leftHandOpened;
                }
            }
        }
        else
        {
            DisableLeftHandUI();
        }

        // MAIN DROITE
        if (!RightHandHasObject())
        {
            if (rightHandUI != null) rightHandUI.SetActive(true);
            if (rightHandImageComponent != null)
            {
                rightHandImageComponent.enabled = true;
                if (rightHandClosed != null && rightHandOpened != null)
                {
                    rightHandImageComponent.sprite = rightClosed ? rightHandClosed : rightHandOpened;
                }
            }
        }
        else
        {
            DisableRightHandUI();
        }
    }

    private void DisableLeftHandUI()
    {
        if (leftHandUI != null) leftHandUI.SetActive(false);
        if (leftHandImageComponent != null) leftHandImageComponent.enabled = false;
    }

    private void DisableRightHandUI()
    {
        if (rightHandUI != null) rightHandUI.SetActive(false);
        if (rightHandImageComponent != null) rightHandImageComponent.enabled = false;
    }

    private void HideUIHands()
    {
        DisableLeftHandUI();
        DisableRightHandUI();
    }

    private void InteractWithObject(bool isLeftClick, bool isRightClick)
    {
        if (isLeftClick && LeftHandHasObject()) return;
        if (isRightClick && RightHandHasObject()) return;

        if (currentReadObject != null)
        {
            if (currentReadObject.IsInAnimation) return;

            currentReadObject.PlaceObjectInFOV();

            if (!currentReadObject.IsReading)
                currentReadObject = null;

            return;
        }

        if (playerCamera == null) return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayers, QueryTriggerInteraction.Ignore))
            return;

        ComplexAnimatorScript animatorObject = hit.collider.GetComponentInParent<ComplexAnimatorScript>();
        if (animatorObject != null)
        {
            animatorObject.Interact();
            return;
        }

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