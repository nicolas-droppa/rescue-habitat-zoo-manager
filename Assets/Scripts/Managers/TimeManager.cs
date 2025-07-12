using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public float gameTime;
    public float timeMultiplier = 1f;
    public bool isPaused => timeMultiplier == 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SetTimeMultiplier(isPaused ? 1f : 0f);

        if (isPaused) return;

        gameTime += Time.deltaTime * timeMultiplier;
    }

    public void SetTimeMultiplier(float multiplier)
    {
        timeMultiplier = multiplier;
    }

    public string GetTimeFormatted()
    {
        int totalSeconds = Mathf.FloorToInt(gameTime);
        int hours = (totalSeconds / 60) % 24;
        int minutes = totalSeconds % 60;
        return $"{hours:D2}:{minutes:D2}";
    }
}