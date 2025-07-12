using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    public Button buttonPause;
    public Button buttonPlay;
    public Button buttonFast;
    public Button buttonUltra;

    void Start()
    {
        buttonPause.onClick.AddListener(() => TimeManager.Instance.SetTimeMultiplier(0f));
        buttonPlay.onClick.AddListener(() => TimeManager.Instance.SetTimeMultiplier(1f));
        buttonFast.onClick.AddListener(() => TimeManager.Instance.SetTimeMultiplier(2f));
        buttonUltra.onClick.AddListener(() => TimeManager.Instance.SetTimeMultiplier(4f));
    }

    void Update()
    {
        if (TimeManager.Instance == null) return;

        timeText.text = TimeManager.Instance.GetTimeFormatted();
    }
}