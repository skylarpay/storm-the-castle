using UnityEngine;
using TMPro;
using System.Collections;

public class UIMessageManager : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public CanvasGroup messagePanelCanvasGroup;
    
    [Header("Messages in order")]
    public string[] messages = new string[]
    {
        "To successfully storm the castle, you must first create a weapon.",
        "Find the right components and craft your sword to advance to Level 2.",
        "Click on the scroll for assistance."
    };
    
    public float[] displayTimes = new float[] { 4f, 4f, 3f };
    public float fadeDuration = 1f;
    
    void Start()
    {
        StartCoroutine(ShowMessagesSequentially());
    }
    
    IEnumerator ShowMessagesSequentially()
    {
        for (int i = 0; i < messages.Length; i++)
        {
            // Set the message text
            messageText.text = messages[i];
            
            // Start invisible
            messageText.alpha = 0;
            
            // Fade IN the text
            yield return StartCoroutine(FadeText(0, 1, fadeDuration));
            
            // Wait for display time
            yield return new WaitForSeconds(displayTimes[i]);
            
            // Fade OUT the text
            yield return StartCoroutine(FadeText(1, 0, fadeDuration));
            
            // Small delay between messages
            yield return new WaitForSeconds(0.5f);
        }
        
        // After ALL messages, fade out the panel
        if (messagePanelCanvasGroup != null)
        {
            yield return StartCoroutine(FadePanel(1, 0, fadeDuration));
            messagePanelCanvasGroup.gameObject.SetActive(false);
        }
    }
    
    IEnumerator FadeText(float start, float end, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, elapsed / duration);
            messageText.alpha = alpha;
            yield return null;
        }
        messageText.alpha = end;
    }
    
    IEnumerator FadePanel(float start, float end, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, elapsed / duration);
            messagePanelCanvasGroup.alpha = alpha;
            yield return null;
        }
        messagePanelCanvasGroup.alpha = end;
    }
}