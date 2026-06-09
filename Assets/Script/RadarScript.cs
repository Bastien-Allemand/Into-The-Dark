using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class RadarScript : MonoBehaviour
{
    public GameObject radar;
    public GameObject playerIcon;
    public GameObject ghostIcon;

    public Vector2 worldMin = new Vector2 (-27f, -18.3f);
    public Vector2 worldMax = new Vector2 (26.7f, 21.6f);

    public Transform player;
    public Transform ghost;

    public Image radarImage;

    private float radarWidth = 0.74f;
    private float radarHeight = 0.71f;

    void Start()
    {
        RectTransform rt = radarImage.GetComponent<RectTransform>();

        //radarWidth = rt.rect.width * 0.4f * 0.001f;
        //radarHeight = rt.rect.height * 0.4f * 0.001f;
    }

    void Update()
    {
        if (radar.transform.parent != null)
        {
            radar.transform.localRotation = Quaternion.Euler(90f, 90f, 90f);
        }

        Vector2 pos = WorldToRadar(player.position);
        playerIcon.transform.localPosition =
            new Vector3(pos.x, -0.5f, pos.y);

        Vector2 pos2 = WorldToRadar(ghost.position);
        ghostIcon.transform.localPosition =
            new Vector3(pos2.x, -0.5f, pos2.y);

        //Debug.Log(pos);
        Debug.Log(radarWidth);
        Debug.Log(radarHeight);

        if (Input.GetKeyDown(KeyCode.F))
        {
            radarUI.SetActive(!radarUI.activeSelf);
        }
    }

    private Vector2 WorldToRadar(Vector3 worldPos)
    {
        float x = Mathf.InverseLerp(worldMin.x, worldMax.x, worldPos.x);
        float y = Mathf.InverseLerp(worldMin.y, worldMax.y, worldPos.z);

        float radarX = (x - 0.5f) * radarWidth;
        float radarY = (y - 0.5f) * radarHeight;

        return new Vector2(radarX, radarY);
    }
}
