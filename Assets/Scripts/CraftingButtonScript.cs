using UnityEngine;

public class CraftingButtonScript : MonoBehaviour
{ 
    [SerializeField] private ItemRecipeSo recipe;

    public void OnCraftButtonClicked()
    {
        if (CraftingManager.instance.CanCraftRecipe(recipe))
        {
            CraftingManager.instance.Craft(recipe);
        }
        else
        {
            Debug.Log("Cannot Craft");
        }
    }
}
