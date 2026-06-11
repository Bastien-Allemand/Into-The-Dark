using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadarScript : MonoBehaviour
{
    [Header("Radar")]
    public GameObject radar;

    [Header("Icons")]
    public RectTransform playerIcon;
    public RectTransform ghostIcon;

    [Header("Scan")]
    public RectTransform circle;
    public Image circleImage; // 🔥 AJOUT IMPORTANT

    [Header("World References")]
    public Transform player;
    public Transform ghost;

    [Header("Map Bounds")]
    public Vector2 worldMin = new Vector2(-27f, -18.3f);
    public Vector2 worldMax = new Vector2(26.7f, 21.6f);

    [Header("Radar Size")]
    [SerializeField] private float radarWidth = 740f;
    [SerializeField] private float radarHeight = 710f;

    [Header("Scan Settings")]
    [SerializeField] private float scanSpeed = 500f;
    [SerializeField] private float scanDurationAfterReveal = 1.0f;

    [Header("Scan Max Size")]
    [SerializeField] private float maxScanSize = 2250f;

    private bool isScanning = false;

    void Start()
    {
        ghostIcon.gameObject.SetActive(false);
        circle.gameObject.SetActive(false);
    }

    void Update()
    {
        if (radar.transform.parent != null)
        {
            radar.transform.localRotation =
                Quaternion.Euler(90f, 90f, 90f);
        }

        playerIcon.anchoredPosition = WorldToRadar(player.position);
        ghostIcon.anchoredPosition = WorldToRadar(ghost.position);

        if (Input.GetKeyDown(KeyCode.F) && !isScanning)
        {
            StartCoroutine(ShowRadar());
        }
    }

    IEnumerator ShowRadar()
    {
        isScanning = true;

        ghostIcon.gameObject.SetActive(false);
        circle.gameObject.SetActive(true);

        Vector2 playerPos = playerIcon.anchoredPosition;
        Vector2 ghostPos = ghostIcon.anchoredPosition;

        circle.anchoredPosition = playerPos;
        circle.sizeDelta = Vector2.zero;

        // reset alpha
        SetCircleAlpha(1f);

        float distanceToGhost = Vector2.Distance(playerPos, ghostPos);
        bool ghostRevealed = false;

        while (circle.sizeDelta.x < maxScanSize)
        {
            float newSize = circle.sizeDelta.x + scanSpeed * Time.deltaTime;
            circle.sizeDelta = new Vector2(newSize, newSize);

            float currentRadius = newSize * 0.5f;

            // 🔥 FADE PROGRESSIF
            float t = newSize / maxScanSize;
            float alpha = Mathf.Lerp(1f, 0f, t);
            SetCircleAlpha(alpha);

            if (!ghostRevealed && currentRadius >= distanceToGhost)
            {
                ghostRevealed = true;
                ghostIcon.gameObject.SetActive(true);
            }

            yield return null;
        }

        yield return new WaitForSeconds(scanDurationAfterReveal);

        ghostIcon.gameObject.SetActive(false);

        circle.sizeDelta = Vector2.zero;
        SetCircleAlpha(1f); // reset
        circle.gameObject.SetActive(false);

        isScanning = false;
    }

    private void SetCircleAlpha(float alpha)
    {
        if (circleImage == null) return;

        Color c = circleImage.color;
        c.a = alpha;
        circleImage.color = c;
    }

    private Vector2 WorldToRadar(Vector3 worldPos)
    {
        float x = Mathf.InverseLerp(worldMin.x, worldMax.x, worldPos.x);

        float y = 1f - Mathf.InverseLerp(worldMin.y, worldMax.y, worldPos.z);

        float radarX = (x - 0.5f) * radarWidth;
        float radarY = (y - 0.5f) * radarHeight;

        return new Vector2(radarX, radarY);
    }
}