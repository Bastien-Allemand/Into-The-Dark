using System.Threading;
using UnityEngine;

public class RadarScript : MonoBehaviour
{
    public GameObject radar;
    public GameObject playerIcon;
    public GameObject ghostIcon;

    public Vector2 worldMin = new Vector2 (0, 0);
    public Vector2 worldMax = new Vector2 (0, 0);

    public RectTransform mapRadarRect;

    public Transform player;
    public Transform ghost;

    void Start()
    {
        
    }

    void Update()
    {
        if (radar.transform.parent != null)
        {
            radar.transform.localRotation = Quaternion.Euler(90f, 90f, 90f);
        }

        //playerIcon.anchoredPosition = WorldToRadar(player.position);
        //ghostIcon.anchoredPosition = WorldToRadar(monster.position);

        if (Input.GetKeyDown(KeyCode.R))
        {
            //radarUI.SetActive(!radarUI.activeSelf);
        }
    }

    private Vector2 WorldToRadar(Vector3 worldPos)
    {
        float x = Mathf.InverseLerp(worldMin.x, worldMax.x, worldPos.x);
        float y = Mathf.InverseLerp(worldMin.y, worldMax.y, worldPos.z);

        float radarX = (x - 0.5f) * mapRadarRect.rect.width;
        float radarY = (y - 0.5f) * mapRadarRect.rect.height;

        return new Vector2(radarX, radarY);
    }
}
