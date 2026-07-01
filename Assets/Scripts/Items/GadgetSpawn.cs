using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GadgetData
{
    public GameObject gadgetPrefab;
    public float spawnTime = 5f;
    public int maxInstances = 1;

    [HideInInspector] public int currentInstances = 0;
    [HideInInspector] public float timer = 0f;
}

[Serializable]
public class GadgetSocket
{
    public GameObject gadgetSocket;
    public bool isOccupied;
}

public class GadgetSpawn : MonoBehaviour
{
    [SerializeField] private int maxGadgetsOnMap = 5;
    [SerializeField] private List<GadgetData> gadgets = new List<GadgetData>();
    [SerializeField] private List<GadgetSocket> sockets = new List<GadgetSocket>();

    private int currentTotalGadgets = 0;

    void Update()
    {
        // 1. Si la map est pleine, on ne fait rien (on fige les chronos)
        if (currentTotalGadgets >= maxGadgetsOnMap) return;

        // 2. On fait tourner le chrono pour chaque gadget
        foreach (var gadget in gadgets)
        {
            // On ne lance le chrono que si le gadget n'a pas atteint sa limite
            if (gadget.currentInstances < gadget.maxInstances)
            {
                gadget.timer += Time.deltaTime; // Time.deltaTime = le temps écoulé depuis la dernière frame

                // 3. Si le temps est écoulé, on le fait apparaître !
                if (gadget.timer >= gadget.spawnTime)
                {
                    bool success = SpawnSpecificGadget(gadget);

                    // Si le spawn a réussi, on remet le chrono à zéro
                    if (success)
                    {
                        gadget.timer = 0f;
                    }
                }
            }
        }
    }

    // Nouvelle fonction qui s'occupe de faire spawner LE gadget dont le temps est écoulé
    private bool SpawnSpecificGadget(GadgetData gadgetToSpawn)
    {

    if (gadgetToSpawn == null)
        return false;

    if (gadgetToSpawn.gadgetPrefab == null)
    {
        return false;

    }

    // 1. Chercher un socket libre
    List<GadgetSocket> availableSockets = new List<GadgetSocket>();
        foreach (var socket in sockets)
        {
            if (socket.gadgetSocket != null && !socket.isOccupied)
            {
                availableSockets.Add(socket);
            }
        }

        // Sécurité : Aucun socket libre
        if (availableSockets.Count == 0) return false;

        // 2. Tirer un socket au hasard parmi ceux qui sont libres
        int randSocketIndex = UnityEngine.Random.Range(0, availableSockets.Count);
        GadgetSocket chosenSocket = availableSockets[randSocketIndex];

        // 3. Instanciation
        GameObject spawnedObj = Instantiate(
            gadgetToSpawn.gadgetPrefab,
            chosenSocket.gadgetSocket.transform.position,
            chosenSocket.gadgetSocket.transform.rotation
        );

        // (Si tu as utilisé le script GadgetItem de ma réponse précédente, décommente ces lignes)
        // GadgetItem itemScript = spawnedObj.GetComponent<GadgetItem>();
        // if (itemScript != null) itemScript.Setup(gadgetToSpawn, chosenSocket);

        // 4. Mise à jour des compteurs
        chosenSocket.isOccupied = true;
        gadgetToSpawn.currentInstances++;
        currentTotalGadgets++;

        return true;
    }
}