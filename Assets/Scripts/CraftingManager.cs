using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager instance;
    [SerializeField] private ItemSlot[] inputArray;
    [SerializeField] private Transform outputPanel;
    [SerializeField] private ItemRecipeSo[] recipes;

    private void Awake()
    {
        instance = this;
    }
    public bool CanCraftRecipe(ItemRecipeSo recipe)
    {
        int foundCount = 0;
        ItemData[] foundItemData = new ItemData[2];
        for (int i = 0; i < inputArray.Length; i++)
        {
            if (inputArray[i] == null) continue;
            
            ItemSlot slot = inputArray[i].GetComponent<ItemSlot>();
    
            if (slot == null || slot.currentItem == null) continue;

            foundItemData[i] = slot.currentItemData;
        }

        foreach (ItemTypeAndCount neededItem in recipe.input)
        {
            int matchCount = 0;

            for (int i = 0; i < foundItemData.Length; i++)
            {
                if (foundItemData[i] == null) continue;

                if (foundItemData[i] == neededItem.item)
                {
                    matchCount++;
                }
            }

            if (matchCount < neededItem.count)
            {
                return false;
            }
        }

        return true;
    }
    

    public void Craft(ItemRecipeSo recipe)
    {
        Debug.Log("Crafting");
        if (!CanCraftRecipe(recipe)) return;

        GameObject craftedItem = Instantiate(recipe.outputPrefab, outputPanel);
        
        craftedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
    
    
}
