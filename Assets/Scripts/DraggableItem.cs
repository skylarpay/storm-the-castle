using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public ItemData itemData;
    [SerializeField] public TMP_Text quantityText;
    [SerializeField] public Image itemImage;

    private Transform originalParent;
    [SerializeField] private CanvasGroup canvasGroup;

    public void CreateDragItem(ItemData thisItemData)
    {
        itemData = thisItemData;
        quantityText.text = itemData.maxStack.ToString();
        quantityText.enabled = true;
        itemImage.sprite = itemData.icon;
        itemImage.enabled = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        //find where the item is being dropped and where it came from
        ItemSlot dropSlot = eventData.pointerEnter?.GetComponent<ItemSlot>();
        ItemSlot originalSlot = originalParent.GetComponent<ItemSlot>();
        

        if (dropSlot != null)
        {
            if (dropSlot.currentItem != null) //handles when a slot already has an item in it
            {
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                originalSlot.currentItemData = itemData;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
                originalSlot.currentItemData = null;
            }
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
            dropSlot.currentItemData = itemData;
        }
        else
        {
            transform.SetParent(originalParent);
        }
        
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
}
