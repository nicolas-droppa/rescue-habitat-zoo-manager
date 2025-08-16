using UnityEngine;

public class BuildMenuController : MonoBehaviour
{
    public GameObject menuRoot;

    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
        menuRoot.SetActive(isOpen);
    }
}
