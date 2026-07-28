using UnityEngine;
using UnityEngine.InputSystem;

public class DropScript : MonoBehaviour
{
    [SerializeField] public Camera playerCamera;

    [Header("Hands")]
    [SerializeField] public GameObject leftHand;
    [SerializeField] public GameObject rightHand;

    private HandContent leftHandContent;
    private HandContent rightHandContent;

    private PlayerAction controls;

    [Header("Throw settings")]
    [SerializeField] private float minForce = 5f;
    [SerializeField] public float maxForce = 20f;
    [SerializeField] private float chargeSpeed = 10f;

    public float leftCharge;
    public float rightCharge;

    public bool chargingLeft;
    public bool chargingRight;

    private void Awake()
    {
        controls = InputManager.controls;

        leftHandContent = leftHand.GetComponent<HandContent>();
        rightHandContent = rightHand.GetComponent<HandContent>();
    }
    private void OnEnable()
    {
        controls.PlayerInteraction.ThrowObject.started += _ => OnStart();
        controls.PlayerInteraction.ThrowObject.canceled += _ => OnRelease();
    }

    private void OnDisable()
    {
        controls.PlayerInteraction.ThrowObject.started -= _ => OnStart();
        controls.PlayerInteraction.ThrowObject.canceled -= _ => OnRelease();
    }

    private void OnStart()
    {
        int side = (int)controls.PlayerInteraction.ThrowObject.ReadValue<float>();
        if (side < 0)
        {
            if (!leftHandContent.filled) return;

            chargingLeft = true;
            leftCharge = 0f;
        }
        else if (side > 0)
        {
            if (!rightHandContent.filled) return;

            chargingRight = true;
            rightCharge = 0f;
        }

    }

    private void OnRelease()
    {
        if (chargingLeft)
        {
            chargingLeft = false;
            Throw(leftHandContent, leftCharge);
        }
        if (chargingRight)
        {
            chargingRight = false;
            Throw(rightHandContent, rightCharge);
        }
    }
    private void Update()
    {
        if (chargingLeft)
            leftCharge += Time.deltaTime * chargeSpeed;

        if (chargingRight)
            rightCharge += Time.deltaTime * chargeSpeed;
    }

    private void Throw(HandContent hand, float charge)
    {
        GameObject obj = hand.TakeOutObject(true);

        if (obj == null)
            return;

        DroneScript drone = obj.GetComponent<DroneScript>();

        if (drone != null)
        {
            drone.PlaceDrone();
            return;
        }

        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError($"{obj.name} has no Rigidbody!");
            return;
        }

        float force = Mathf.Clamp(charge, minForce, maxForce);

        rb.AddForce(
            playerCamera.transform.forward * force,
            ForceMode.Impulse
        );
    }

}
