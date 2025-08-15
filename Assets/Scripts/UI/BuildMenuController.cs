using UnityEngine;

public class BuildMenuController : MonoBehaviour
{
    public GameObject buildMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            buildMenu.SetActive(!buildMenu.activeSelf);
        }
    }
}
