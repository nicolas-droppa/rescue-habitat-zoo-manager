using UnityEngine;

public class JobSpawner : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // pravý klik
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = GridManager.Instance.WorldToCell(worldPos);

            Job newJob = new Job(cellPos, () =>
            {
                Debug.Log($"✅ Úloha dokončená na {cellPos}");
            });

            JobSystem.Instance.EnqueueJob(newJob);
            Debug.Log($"📥 Úloha pridaná na {cellPos}");
        }
    }
}
