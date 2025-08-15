using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BuildMenuUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform middleGrid;
    public GameObject buttonPrefab;

    [Header("Item Lists")]
    public List<BuildItem> foundations;
    public List<BuildItem> walls;
    public List<BuildItem> grounds;
    public List<BuildItem> doors;
    public List<BuildItem> objects;
    public List<BuildItem> water;
    public List<BuildItem> electricity;
    public List<BuildItem> animals;

    private BuildCategory currentCategory;

    public void ShowCategory(int categoryIndex)
    {
        currentCategory = (BuildCategory)categoryIndex;
        RefreshGrid();
    }

    void RefreshGrid()
    {
        // Vymažeme staré tlačidlá
        foreach (Transform child in middleGrid)
            Destroy(child.gameObject);

        // Vyberieme zoznam podľa kategórie
        List<BuildItem> selectedList = GetItemListForCategory(currentCategory);

        // Vytvoríme nové tlačidlá
        if (selectedList != null)
        {
            foreach (var item in selectedList)
            {
                var btnObj = Instantiate(buttonPrefab, middleGrid);
                btnObj.GetComponentInChildren<Text>().text = item.itemName;
                btnObj.GetComponent<Image>().sprite = item.icon;

                // Pri kliknutí zavoláme BuildManager
                btnObj.GetComponent<Button>().onClick.AddListener(() => {
                    BuildManager.Instance.SelectBuildItem(item);
                });
            }
        }
    }

    private List<BuildItem> GetItemListForCategory(BuildCategory category)
    {
        switch (category)
        {
            case BuildCategory.Foundations: return foundations;
            case BuildCategory.Walls: return walls;
            case BuildCategory.Grounds: return grounds;
            case BuildCategory.Doors: return doors;
            case BuildCategory.Objects: return objects;
            case BuildCategory.Water: return water;
            case BuildCategory.Electricity: return electricity;
            case BuildCategory.Animals: return animals;
            default: return null;
        }
    }
}
