using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadarScript : MonoBehaviour
{
    [Header("Radar")]
    public GameObject radar;

    [Header("Map")]
    public RectTransform rectCanva;
    public Image imageFloor1;
    public Image imageFloor2;

    [Header("Icons")]
    public RectTransform playerIcon;
    public RectTransform ghostIcon;

    [Header("Scan")]
    public RectTransform circle;
    public Image circleImage;

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

    [Header("Ghost Attraction")]
    public Pathfinding ghostPathfinding;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip radarSound;

    private bool isScanning = false;
    private float firstFloorHeight = 17f;

    private

    void Start()
    {
        ghostIcon.gameObject.SetActive(false);
        circle.gameObject.SetActive(false);
        imageFloor2.enabled = false;
    }

    void Update()
    {
        if (radar.transform.parent != null)
        {
            radar.transform.localRotation = Quaternion.Euler(90f, 90f, 90f);

            rectCanva.transform.localRotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        }

        Debug.Log(player.position.y);

        if (player.position.y >= firstFloorHeight)
        {
            imageFloor1.enabled = false;
            imageFloor2.enabled = true;
        }
        else if (player.position.y < firstFloorHeight)
        {
            imageFloor1.enabled = true;
            imageFloor2.enabled = false;
        }

        playerIcon.anchoredPosition = WorldToRadar(player.position);
        ghostIcon.anchoredPosition = WorldToRadar(ghost.position);

        if (Input.GetKeyDown(KeyCode.F) && !isScanning)
        {
            if (audioSource != null && radarSound != null)
            {
                audioSource.PlayOneShot(radarSound);
            }

            StartCoroutine(ShowRadar());
        }
    }

    private IEnumerator StopRadarSoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
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

        SetCircleAlpha(1f);

        float distanceToGhost = Vector2.Distance(playerPos, ghostPos);
        bool ghostRevealed = false;

        while (circle.sizeDelta.x < maxScanSize)
        {
            float newSize = circle.sizeDelta.x + scanSpeed * Time.deltaTime;
            circle.sizeDelta = new Vector2(newSize, newSize);

            float currentRadius = newSize * 0.5f;

            float t = newSize / maxScanSize;
            float alpha = Mathf.Lerp(1f, 0f, t);
            SetCircleAlpha(alpha);

            if (!ghostRevealed && currentRadius >= distanceToGhost)
            {
                ghostRevealed = true;
                ghostIcon.gameObject.SetActive(true);

                StartCoroutine(StopRadarSoundAfterDelay(1f));

                if (ghostPathfinding != null)
                {
                    ghostPathfinding.AttractToPlayer(player, 2f);
                }
            }

            yield return null;
        }

        yield return new WaitForSeconds(scanDurationAfterReveal);

        ghostIcon.gameObject.SetActive(false);

        circle.sizeDelta = Vector2.zero;
        SetCircleAlpha(1f);
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