using UnityEngine;

[CreateAssetMenu(fileName = "ItemRecipeSo", menuName = "Scriptable Objects/ItemRecipeSo")]
public class ItemRecipeSo : ScriptableObject
{
    public string recipeName;
    public DraggableItem outputPrefab;
    public ItemTypeAndCount[] input;
    public ItemTypeAndCount[] output;
}

[System.Serializable]
public class ItemTypeAndCount
{
    public ItemData item;
    public int count;

    public ItemTypeAndCount(ItemData i, int c)
    {
        item = i;
        count = c;
    }
}