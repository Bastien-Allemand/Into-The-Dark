using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GadgetData
{
    public string gadgetName;
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

    public List<GameObject> allowedPrefabs = new List<GameObject>();
    public int previewIndex = 0;
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
        if (currentTotalGadgets >= maxGadgetsOnMap) return;

        foreach (var gadget in gadgets)
        {
            if (gadget.currentInstances < gadget.maxInstances)
            {
                gadget.timer += Time.deltaTime;

                if (gadget.timer >= gadget.spawnTime)
                {
                    bool success = SpawnSpecificGadget(gadget);

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
                if (socket.allowedPrefabs.Count == 0 || socket.allowedPrefabs.Contains(gadgetToSpawn.gadgetPrefab))
                {
                    availableSockets.Add(socket);
                }
            }
        }

        if (availableSockets.Count == 0) return false;

        int randSocketIndex = UnityEngine.Random.Range(0, availableSockets.Count);
        GadgetSocket chosenSocket = availableSockets[randSocketIndex];

        // --- TRACAGE --- (Panneau 2 de l'image)
        Debug.Log("Tentative de spawn sur le socket : " + chosenSocket.gadgetSocket.name);

        GameObject spawnedObj = Instantiate(
            gadgetToSpawn.gadgetPrefab,
            chosenSocket.gadgetSocket.transform.position,
            chosenSocket.gadgetSocket.transform.rotation
        );

        // --- GESTION D'ERREUR SÉCURISÉE --- (Panneau 3B de l'image)
        if (spawnedObj == null)
        {
            // L'instantiation a échoué silencieusement (cas rare mais possible)
            Debug.LogError("ÉCHEC CRITIQUE : L'objet '" + gadgetToSpawn.gadgetPrefab.name + "' n'a pas pu être instancié !");
            // On retourne false : Le socket ne passera PAS en [OCCUPÉ]
            return false;
        }

        // Si on arrive ici, l'objet existe physiquement dans la Hierarchy.
        Debug.Log("Spawn réussi ! Clone créé : " + spawnedObj.name);

        chosenSocket.isOccupied = true;
        gadgetToSpawn.currentInstances++;
        currentTotalGadgets++;

        return true;
    }

    private void OnValidate()
    {
        if (gadgets == null || gadgets.Count == 0) return;

        foreach (var socket in sockets)
        {
            if (socket.allowedPrefabs == null)
            {
                socket.allowedPrefabs = new List<GameObject>();
            }

            if (socket.allowedPrefabs.Count == 0)
            {
                foreach (var g in gadgets)
                {
                    if (g.gadgetPrefab != null) socket.allowedPrefabs.Add(g.gadgetPrefab);
                }
            }
        }
    }

    public void RemplirTousLesSockets()
    {
        if (gadgets == null || gadgets.Count == 0)
        {
            Debug.LogWarning("Ta liste principale 'Gadgets' est vide !");
            return;
        }

        foreach (var socket in sockets)
        {
            if (socket.allowedPrefabs == null)
                socket.allowedPrefabs = new List<GameObject>();

            socket.allowedPrefabs.Clear();

            foreach (var g in gadgets)
            {
                if (g.gadgetPrefab != null)
                {
                    socket.allowedPrefabs.Add(g.gadgetPrefab);
                }
            }
        }
        Debug.Log("Tous les sockets ont été pré-remplis !");
    }

    [ContextMenu("Debug: Force Random Spawn")]
    public void ForceSpawnRandom()
    {
        if (gadgets.Count == 0) return;
        int rand = UnityEngine.Random.Range(0, gadgets.Count);
        SpawnSpecificGadget(gadgets[rand]);
    }

    // ==========================================
    // DEBBUGAGE VISUEL AVEC PRÉVISUALISATION 3D
    // ==========================================
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (sockets == null) return;

        foreach (var socket in sockets)
        {
            if (socket.gadgetSocket == null) continue;

            Vector3 pos = socket.gadgetSocket.transform.position;

            Gizmos.color = Color.gray;
            Gizmos.DrawLine(transform.position, pos);

            Gizmos.color = socket.isOccupied ? Color.red : Color.green;
            Gizmos.DrawWireSphere(pos, 0.3f);

            if (!socket.isOccupied && socket.allowedPrefabs != null && socket.allowedPrefabs.Count > 0)
            {
                int indexToDisplay = Mathf.Clamp(socket.previewIndex, 0, socket.allowedPrefabs.Count - 1);

                GameObject previewPrefab = socket.allowedPrefabs[indexToDisplay];

                if (previewPrefab != null)
                {
                    Gizmos.color = new Color(0f, 0.8f, 1f, 0.35f);

                    // Matrice du socket (là où on veut dessiner)
                    Matrix4x4 socketMatrix = socket.gadgetSocket.transform.localToWorldMatrix;
    
                    // Matrice inverse du prefab racine (pour neutraliser ses coordonnées d'asset)
                    Matrix4x4 prefabRootMatrix = previewPrefab.transform.worldToLocalMatrix;

                    // --- 1. Dessiner les Mesh statiques ---
                    MeshFilter[] meshFilters = previewPrefab.GetComponentsInChildren<MeshFilter>();
                    foreach (var mf in meshFilters)
                    {
                        if (mf.sharedMesh == null) continue;

                        Matrix4x4 meshWorldMatrix = mf.transform.localToWorldMatrix;
        
                        // Ordre crucial : On prend la position du mesh, on la rend relative au prefab, puis on l'applique au socket
                        Gizmos.matrix = socketMatrix * prefabRootMatrix * meshWorldMatrix;

                        Gizmos.DrawMesh(mf.sharedMesh);
                        Gizmos.DrawWireMesh(mf.sharedMesh);
                    }

                    // --- 2. Dessiner les Mesh animés (SkinnedMeshRenderers) ---
                    SkinnedMeshRenderer[] skinnedMeshes = previewPrefab.GetComponentsInChildren<SkinnedMeshRenderer>();
                    foreach (var smr in skinnedMeshes)
                    {
                        if (smr.sharedMesh == null) continue;

                        Matrix4x4 meshWorldMatrix = smr.transform.localToWorldMatrix;
        
                        Gizmos.matrix = socketMatrix * prefabRootMatrix * meshWorldMatrix;

                        Gizmos.DrawMesh(smr.sharedMesh);
                        Gizmos.DrawWireMesh(smr.sharedMesh);
                    }

                    // On réinitialise la matrice globale pour ne pas affecter les autres Gizmos
                    Gizmos.matrix = Matrix4x4.identity;
                }
            }

            int totalGadgetsDispo = gadgets.Count;
            int filtresActuels = socket.allowedPrefabs?.Count ?? 0;

            int activeIndex = (socket.allowedPrefabs != null && socket.allowedPrefabs.Count > 0)
                ? Mathf.Clamp(socket.previewIndex, 0, socket.allowedPrefabs.Count - 1) : -1;

            string labelText = socket.isOccupied ? "<color=red>[OCCUPÉ]</color>" : "<color=cyan>[PRÉVISUALISATION]</color>";
            labelText += $"\nFiltre: {(socket.allowedPrefabs.Count == 0 ? "TOUS" : socket.allowedPrefabs.Count + " spécifié(s)")}";

            if (socket.allowedPrefabs != null)
            {
                for (int i = 0; i < socket.allowedPrefabs.Count; i++)
                {
                    var p = socket.allowedPrefabs[i];
                    if (p != null)
                    {
                        if (i == activeIndex && !socket.isOccupied)
                        {
                            labelText += $"\n<color=#00FFFF><b>► {p.name} (Affiché)</b></color>";
                        }
                        else
                        {
                            labelText += $"\n • {p.name}";
                        }
                    }
                }
            }

            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.richText = true;
            style.alignment = TextAnchor.MiddleCenter;

            UnityEditor.Handles.Label(pos + Vector3.up * 1.2f, labelText, style);
        }
    }
#endif
}