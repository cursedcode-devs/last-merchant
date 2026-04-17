using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sonar : MonoBehaviour
{

    [Header("Radar Scan")]
    public float distance = 20f;
    public float angle = 90f;
    public int rays = 15;

    [Header("Beep Timing")]
    public float slowBeep = 1.2f;
    public float fastBeep = 0.12f;

    [Header("Audio")]
    public AudioClip beepClip;
    public float volume = 1f;
    public float farPitch = 0.9f;
    public float closePitch = 1.6f;

    private AudioSource audioSource;
    private float timer = 0f;
    public FMODUnity.EventReference sonarSound;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.pitch = 1f;
        audioSource.panStereo = 0f;

        Debug.Log($"[Sonar] START — beepClip: {(beepClip == null ? "NULL ❌" : beepClip.name + " ✅")}");
        Debug.Log($"[Sonar] START — AudioSource: {(audioSource == null ? "NULL ❌" : "found ✅")}");
        int mask = LayerMask.GetMask("Planet");
        Debug.Log($"[Sonar] START — Planet layer mask: {mask} {(mask == 0 ? "❌ (layer not found!)" : "✅")}");

        Physics2D.queriesStartInColliders = false;
    }

    void Update()
    {
        ScanCone();
    }

    void ScanCone()
    {
        int planetLayer = LayerMask.GetMask("Planet");

        float startAngle = -angle / 2f;
        float step = angle / (rays - 1);

        bool foundHit = false;
        float closestDistance = distance;
        float bestSide = 0f;

        for (int i = 0; i < rays; i++)
        {
            float currentAngle = startAngle + step * i;
            Vector2 dir = RotateVector(transform.up, currentAngle);

            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                dir,
                distance,
                planetLayer
            );

            Debug.DrawRay(transform.position, dir * distance, Color.green);

            if (hit.collider != null)
            {
                foundHit = true;

                if (hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    bestSide = Mathf.Clamp(currentAngle / (angle / 2f), -1f, 1f);
                }
            }
        }

        if (foundHit)
        {
            timer -= Time.deltaTime;

            float t = 1f - (closestDistance / distance);
            float beepRate = Mathf.Lerp(slowBeep, fastBeep, t);

            //Debug.Log($"[Sonar] foundHit | timer: {timer:F3} | beepRate: {beepRate:F3}");

            if (timer <= 0f)
            {
                Debug.Log("[Sonar] >>> Calling PlayBeep NOW <<<");
                FMODUnity.RuntimeManager.PlayOneShot(sonarSound, this.transform.position);
                timer = beepRate;
            }
        }
        else
        {
            timer = 0f;
        }
    }

    //void PlayBeep(float closeness, float side)
    //{
    //    if (beepClip == null)
    //    {
    //        Debug.LogError("[Sonar] PlayBeep called but beepClip is NULL ❌");
    //        return;
    //    }

    //    audioSource.pitch = Mathf.Lerp(farPitch, closePitch, closeness);
    //    audioSource.panStereo = side;
    //    audioSource.PlayOneShot(beepClip, volume);

    //    Debug.Log($"[Sonar] PlayOneShot fired ✅ | clip: {beepClip.name} | pitch: {audioSource.pitch:F2} | pan: {audioSource.panStereo:F2} | volume: {volume}");
    //}

    Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);

        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }
}