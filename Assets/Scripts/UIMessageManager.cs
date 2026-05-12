using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIMessageManager : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public CanvasGroup messageCanvasGroup;
    
    [Header("Messages in order")]
    public string[] messages = new string[]
    {
        "To successfully storm the castle, you must first create a weapon.",
        "Find the right components and craft your sword to advance to Level 2.",
        "Click on the scroll for assistance."
    };
    
    public float[] displayTimes = new float[] { 7f, 7f, 5f };
    public float fadeDuration = 1f;
    
    void Start()
    {
        StartCoroutine(ShowMessagesSequentially());
    }
    
    IEnumerator ShowMessagesSequentially()
    {
        for (int i = 0; i < messages.Length; i++)
        {
            // Set message text
            messageText.text = messages[i];
            
            // Fade in
            yield return StartCoroutine(FadeCanvasGroup(messageCanvasGroup, 0, 1, fadeDuration));
            
            // Wait for display time
            yield return new WaitForSeconds(displayTimes[i]);
            
            // Fade out
            yield return StartCoroutine(FadeCanvasGroup(messageCanvasGroup, 1, 0, fadeDuration));
            
            // Small delay between messages
            yield return new WaitForSeconds(0.5f);
        }
        
        // Hide the text object completely after all messages
        messageText.gameObject.SetActive(false);
    }
    
    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        cg.alpha = end;
    }
}