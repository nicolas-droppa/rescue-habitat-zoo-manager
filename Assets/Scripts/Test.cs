using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Hello Unity");
    }

    void Update()
    {
        Debug.Log(TimeManager.Instance.GetTimeFormatted());
    }
}