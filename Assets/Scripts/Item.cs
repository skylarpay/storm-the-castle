using UnityEngine;
using UnityEngine.InputSystem;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemData itemData;
    [SerializeField] private InputActionReference interact;

    private bool playerInRange;
    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        playerInRange = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void OnEnable()
    {
        if (interact != null && !interact.action.enabled)
        {
            interact.action.Enable();
        }
    }

    void Update()
    {
        if (playerInRange && interact != null && interact.action.WasPressedThisFrame())
        {
            inventoryManager.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}