using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public class InventoryManager : MonoBehaviour
{
    private GameObject inventoryMenu;
    private bool menuActivated;

    public InputActionReference invToggle;

    public InputActionAsset playerActionMap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuActivated = false;
        inventoryMenu = GameObject.Find("InventoryMenu");
        inventoryMenu.SetActive(false);
    }
    private void OnEnable()
    {
        invToggle.action.started += toggleInventory;
    }

    private void OnDisable()
    {
        invToggle.action.started -= toggleInventory;
    }

    private void toggleInventory(InputAction.CallbackContext ctx)
    {
        if (!menuActivated)
        {
            
            menuActivated = true;
            inventoryMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            playerActionMap.Disable();
        }
        else
        {
            menuActivated = false;
            inventoryMenu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerActionMap.Enable();            
        }
    }
}
