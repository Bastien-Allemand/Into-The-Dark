using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GadgetData
{
    public GameObject gadgetPrefab;

    public float spawnTime = 5f;
    public int maxInstances = 1;
}

[Serializable]
public class GadgetSocket
{
    public GameObject gadgetSocket;

    public bool Item;
}
public class GadgetSpawn : MonoBehaviour
{
    [SerializeField] private int maxGadget = 0;
    [SerializeField] private List<GadgetData> gadget = new List<GadgetData>();
    [SerializeField] private List<GadgetSocket> spawn = new List<GadgetSocket>();

    private bool Spawn()
    {
        int lenSocket = spawn.Count;
        int randSocket = UnityEngine.Random.Range(0, lenSocket);

        int lenGadget = gadget.Count;
        int randGadget = UnityEngine.Random.Range(0, lenGadget);

        if (spawn[randSocket] != null && !spawn[randSocket].Item)
        {
            Instantiate(
                gadget[randGadget].gadgetPrefab,
                spawn[randSocket].gadgetSocket.transform.position,
                spawn[randSocket].gadgetSocket.transform.rotation
            );

            spawn[randSocket].Item = true;
            return true;
        }

        return false;
    }
}
