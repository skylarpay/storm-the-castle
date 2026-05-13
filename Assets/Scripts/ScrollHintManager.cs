using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class ScrollHintManager : MonoBehaviour, IPointerClickHandler
{
    public Image scrollImage;
    public GameObject hintPanel;  // The brown text box
    public TextMeshProUGUI hintText;
    public AudioSource audioSource;
    public AudioClip clickSound;
    
    private bool isOpen = false;
    private int clickCount = 0;
    private Color originalColor;
    
    void Start()
    {
        originalColor = scrollImage.color;
        
        // Start the glow coroutine
        StartCoroutine(InitialGlow());
        
        // Hide hint panel at start
        hintPanel.SetActive(false);
    }
    
    IEnumerator InitialGlow()
    {
        float duration = 3f;
        float elapsed = 0;
        Color targetColor = Color.yellow;
        
        // Pulse glow effect for 3 seconds
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.PingPong(Time.time * 2, 1);
            scrollImage.color = Color.Lerp(originalColor, targetColor, t);
            yield return null;
        }
        
        // Return to original color
        scrollImage.color = originalColor;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        // Play sparkly sound
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        
        clickCount++;
        
        if (!isOpen)
        {
            // Open hint panel
            isOpen = true;
            hintPanel.SetActive(true);
            
            // Set hint text based on click count
            if (clickCount == 1)
            {
                hintText.text = "The finest swords are composed of equal parts wood and iron.";
            }
            else if (clickCount >= 2)
            {
                hintText.text = "The finest swords are composed of equal parts wood and iron.\n\nStuck? Once you collect the items, bring them to the crafting stump.";
                
                // Optional: Add code to highlight items here
                // HighlightCollectibleItems();
            }
        }
        else
        {
            // Close hint panel
            isOpen = false;
            hintPanel.SetActive(false);
        }
    }
    
    // Optional: Call this to highlight items on the map
    void HighlightCollectibleItems()
    {
        // Find items and add glow/outline
        // GameObject[] items = GameObject.FindGameObjectsWithTag("Collectible");
        // foreach (GameObject item in items)
        // {
        //     // Add highlight effect
        // }
    }
}