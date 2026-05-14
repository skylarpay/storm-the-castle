using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public bool isFull = false;

    public GameObject currentItem;
    public ItemData currentItemData;
    
    public GameObject draggableItemPrefab;

    public void AddItem(ItemData itemData)
    {
        if (currentItem == null)
        {
            GameObject item = Instantiate(draggableItemPrefab, transform);
            item.transform.SetParent(transform);

            currentItem = item;
            currentItemData = itemData;
            isFull = true;

            item.GetComponent<DraggableItem>().CreateDragItem(itemData);
            item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}