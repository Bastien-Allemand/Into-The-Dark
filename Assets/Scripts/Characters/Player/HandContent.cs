using UnityEngine;

public class HandContent : MonoBehaviour
{
    [SerializeField] public bool filled = false;
    [SerializeField] public GameObject inhand;

    public void GiveObject(GameObject obj)
    {
        if (filled == false)
        {
            Debug.Log("Object Given Successfully");
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            filled = true;
            inhand = obj;
        }
    }
    public GameObject TakeOutObject()
    {
        if (filled == true)
        {
            Debug.Log("Object Dropped Successfully");
            inhand.transform.SetParent(null);
            filled = false;
            
            GameObject result = inhand;
            inhand = null;
            
            return result;
        }
        return null;
    }

}
