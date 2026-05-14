using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    [SerializeField] public TMP_Text quantityText;
    [SerializeField] public Image itemImage;

    private Transform originalParent;
    [SerializeField] private CanvasGroup canvasGroup;

    public void CreateDragItem(string name, int amount, Sprite icon)
    {
        itemName = name;
        quantity = amount;
        itemSprite = icon;
        
        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
        itemImage.sprite = itemSprite;
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
        
        ItemSlot dropSlot = eventData.pointerEnter?.GetComponent<ItemSlot>();
        ItemSlot originalSlot = originalParent.GetComponent<ItemSlot>();
        

        if (dropSlot != null)
        {
            if (dropSlot.currentItem != null)
            {
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
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
