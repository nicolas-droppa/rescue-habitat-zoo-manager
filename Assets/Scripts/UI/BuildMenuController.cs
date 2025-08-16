using UnityEngine;

public class BuildMenuController : MonoBehaviour
{
    public GameObject menuRoot;
    private bool isOpen;

    void Start()
    {
        isOpen = menuRoot.activeSelf;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isOpen = !isOpen;
        menuRoot.SetActive(isOpen);
    }
}
