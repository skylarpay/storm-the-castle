using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour
{
    //SLOT DATA//
    public bool isFull = false;

    public GameObject currentItem;
    
    public GameObject draggableItemPrefab;

    public void AddItem(string itemName, int amount, Sprite icon)
    {
        Debug.Log("entered itemSlot Script");
        if (currentItem == null)
        {
            GameObject item  = Instantiate(draggableItemPrefab, transform);
            item.transform.SetParent(transform);
            item.GetComponent<DraggableItem>().CreateDragItem(itemName, amount, icon);
            currentItem = item;
            item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
