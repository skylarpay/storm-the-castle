using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public bool isFull = false;

    public GameObject currentItem;
    
    public GameObject draggableItemPrefab;

    public void AddItem(string itemName, int amount, Sprite icon)
    {
        if (currentItem == null)
        {
            GameObject item = Instantiate(draggableItemPrefab, transform);
            item.transform.SetParent(transform);

            item.GetComponent<DraggableItem>().CreateDragItem(itemName, amount, icon);

            currentItem = item;
            isFull = true;

            item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}