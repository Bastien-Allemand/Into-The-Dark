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

    void Start()
    {
        // 1. On va chercher le composant ItemSpawner présent dans la scène
        ItemSpawner spawnerConfig = FindAnyObjectByType<ItemSpawner>();

        if (spawnerConfig == null)
        {
            Debug.LogError("GadgetSpawn : Aucun ItemSpawner trouvé dans la scène !");
            return;
        }

        // 2. On récupère la liste des prefabs autorisés pour cette nuit via l'ItemSpawner
        // (La logique interne de ton ItemSpawner filtre déjà par nuit grâce à ton code précédent)
        List<GameObject> prefabsAutorises = spawnerConfig.GetAllowedPrefabsForCurrentNight();

        // 3. On filtre notre liste de gadgets : on retire ceux qui ne font pas partie de la nuit actuelle
        for (int i = gadgets.Count - 1; i >= 0; i--)
        {
            if (!prefabsAutorises.Contains(gadgets[i].gadgetPrefab))
            {
                gadgets.RemoveAt(i); // Ce gadget n'a pas le droit de spawn cette nuit
            }
        }
    }

    void Update()
    {
        // 1. Si la map est pleine, on ne fait rien (on fige les chronos)
        if (currentTotalGadgets >= maxGadgetsOnMap) return;

        // 2. On fait tourner le chrono pour chaque gadget restant/autorisé
        foreach (var gadget in gadgets)
        {
            // On ne lance le chrono que si le gadget n'a pas atteint sa limite
            if (gadget.currentInstances < gadget.maxInstances)
            {
                gadget.timer += Time.deltaTime;

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

    private bool SpawnSpecificGadget(GadgetData gadgetToSpawn)
    {
        List<GadgetSocket> availableSockets = new List<GadgetSocket>();
        foreach (var socket in sockets)
        {
            if (socket.gadgetSocket != null && !socket.isOccupied)
            {
                availableSockets.Add(socket);
            }
        }

        if (availableSockets.Count == 0) return false;

        int randSocketIndex = UnityEngine.Random.Range(0, availableSockets.Count);
        GadgetSocket chosenSocket = availableSockets[randSocketIndex];

        GameObject spawnedObj = Instantiate(
            gadgetToSpawn.gadgetPrefab,
            chosenSocket.gadgetSocket.transform.position,
            chosenSocket.gadgetSocket.transform.rotation
        );

        chosenSocket.isOccupied = true;
        gadgetToSpawn.currentInstances++;
        currentTotalGadgets++;

        return true;
    }
}