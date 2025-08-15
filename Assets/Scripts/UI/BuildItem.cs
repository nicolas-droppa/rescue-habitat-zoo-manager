using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class BuildItem
{
    public string itemName;
    public Sprite icon;
    public RuleTile tile;
    public BuildCategory category;
}

public enum BuildCategory
{
    Foundations,
    Walls,
    Grounds,
    Doors,
    Objects,
    Water,
    Electricity,
    Animals
}
