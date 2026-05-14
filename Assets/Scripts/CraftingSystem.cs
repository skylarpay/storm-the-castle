using UnityEngine;

public class CraftingSystem
{
    private DraggableItem[,] itemArray;
    private const int GRID_SIZE = 3;
    public CraftingSystem()
    {
        itemArray = new DraggableItem[GRID_SIZE, GRID_SIZE];
    }

    private bool IsEmpty(int x, int y)
    {
        return itemArray[x, y] == null;
    }

    public DraggableItem GetItem(int x, int y)
    {
        return itemArray[x, y];
    }

    public void SetItem(DraggableItem item, int x, int y)
    {
        itemArray[x, y] =  item;
    }

    private void RemoveItem(int x, int y)
    {
        SetItem(null, x, y);
    }

    private bool TryAddItem(DraggableItem item, int x, int y)
    {
        if (IsEmpty(x, y))
        {
            SetItem(item, x, y);
            return true;
        }
        else
        {
            return false;
        }
    }
}
