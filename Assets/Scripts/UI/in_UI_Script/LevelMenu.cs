using Unity.VectorGraphics;
using UnityEditor;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] GameObject GO_content;
    [SerializeField] GameObject GO_prefab_bouton;
    [SerializeField] SceneAsset[] Scenes_Level;

    GameSave gameSave => GameSaveManager.Instance.gameSave;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        foreach (SceneAsset scene in Scenes_Level)
        {
            GameObject bouton = Instantiate(GO_prefab_bouton, GO_content.transform);
            bouton.GetComponent<Bouton>().sc_target = scene;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
