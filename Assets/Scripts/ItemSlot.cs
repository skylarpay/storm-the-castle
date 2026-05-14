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
    public ItemData currentItemData;
    
    public GameObject draggableItemPrefab;

    public void AddItem(ItemData itemData)
    {
        Debug.Log("entered itemSlot Script");
        if (currentItem == null)
        {
            GameObject item  = Instantiate(draggableItemPrefab, transform);
            item.transform.SetParent(transform);
            currentItem = item;
            currentItemData = itemData;
            item.GetComponent<DraggableItem>().CreateDragItem(itemData);
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
