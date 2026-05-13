using UnityEngine;
using UnityEngine.InputSystem;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int amount;
    [SerializeField] private Sprite thisSprite;
    private bool playerInRange;
    private InventoryManager inventoryManager;
    [SerializeField] InputActionReference interact;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    void OnEnable()
    {
        interact.action.Enable();
    }

    void OnDisable()
    {
        interact.action.Disable();
    }
    
    private void OnTriggerExit(Collider collision) 
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            playerInRange = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && interact.action.WasPressedThisFrame())
        {
            inventoryManager.AddItem(itemName, amount, thisSprite);
            Destroy(gameObject);
        }
    }
}
