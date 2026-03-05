using UnityEngine;

public class Jitter : MonoBehaviour
{
    [Header("Jitter Settings")]
    [Tooltip("How far the object can move during a jitter")]
    public float jitterStrength = 0.1f;

    [Tooltip("How long each jitter lasts (seconds)")]
    public float jitterDuration = 0.3f;

    [Tooltip("Time between jitters (seconds)")]
    public float jitterInterval = 3f;

    [Tooltip("Add some randomness to the interval")]
    public float intervalVariance = 0.5f;

    [Header("Axis Control")]
    public bool jitterX = true;
    public bool jitterY = true;
    public bool jitterZ = false;

    private Vector3 originalPosition;
    private bool isJittering = false;
    private float timer = 0f;
    private float nextJitterTime;

    void Start()
    {
        originalPosition = transform.localPosition;
        ScheduleNextJitter();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!isJittering && timer >= nextJitterTime)
        {
            StartCoroutine(DoJitter());
            ScheduleNextJitter();
        }
    }

    private System.Collections.IEnumerator DoJitter()
    {
        isJittering = true;
        float elapsed = 0f;

        while (elapsed < jitterDuration)
        {
            float x = jitterX ? Random.Range(-jitterStrength, jitterStrength) : 0f;
            float y = jitterY ? Random.Range(-jitterStrength, jitterStrength) : 0f;
            float z = jitterZ ? Random.Range(-jitterStrength, jitterStrength) : 0f;

            transform.localPosition = originalPosition + new Vector3(x, y, z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Snap back to original position
        transform.localPosition = originalPosition;
        isJittering = false;
    }

    private void ScheduleNextJitter()
    {
        timer = 0f;
        nextJitterTime = jitterInterval + Random.Range(-intervalVariance, intervalVariance);
        nextJitterTime = Mathf.Max(0.1f, nextJitterTime); // Ensure it's never negative
    }

    // Call this if you move the object and want to update the "resting" position
    public void UpdateOriginalPosition()
    {
        originalPosition = transform.localPosition;
    }
}
