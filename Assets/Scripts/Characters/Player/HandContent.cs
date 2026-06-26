using UnityEngine;

public class HandContent : MonoBehaviour
{
    [SerializeField] public Transform holdPoint;


    [SerializeField] public bool filled = false;
    [SerializeField] public GameObject inHand;
    [SerializeField] public ItemScript itemScript;


    public void GiveObject(GameObject obj)
    {
        if (!filled)
        {
            obj.transform.SetParent(holdPoint, false);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;

            obj.transform.localScale = Vector3.one;

            foreach (Collider col in obj.GetComponentsInChildren<Collider>())
            {
                col.enabled = false;
            }

            Rigidbody rb = obj.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            filled = true;
            inHand = obj;
            itemScript = obj.GetComponent<ItemScript>();
        }
    }
    public GameObject TakeOutObject(bool _restoreRb)
    {
        if (!filled)
            return null;

        GameObject result = inHand;

        result.transform.SetParent(null);

        foreach (Collider col in result.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }

        Rigidbody rb = result.GetComponent<Rigidbody>();

        if (rb != null && _restoreRb)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        filled = false;
        inHand = null;
        itemScript = null;

        return result;
    }

}
