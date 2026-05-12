using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PersistentUIManager : MonoBehaviour
{
    public static PersistentUIManager Instance;
    
    public GameObject controlsButton;
    public GameObject restartButton;
    public AudioSource audioSource;
    public AudioClip buttonClickSound;
    
    void Awake()
    {
        // Singleton pattern - only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        // Find buttons by name (or assign in Inspector)
        if (controlsButton == null)
            controlsButton = GameObject.Find("IG_ControlsButton");
        if (restartButton == null)
            restartButton = GameObject.Find("RestartButton");
            
        // Add button listeners
        if (controlsButton != null)
        {
            Button btn = controlsButton.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OpenControls);
        }
        
        if (restartButton != null)
        {
            Button btn = restartButton.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(RestartToTitle);
        }
    }
    
    public void OpenControls()
    {
        PlayButtonSound();
    
        // Save which scene we're coming from
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("ReturnToScene", currentScene);
        PlayerPrefs.Save();
    
        SceneManager.LoadScene("ControlScreen");
    }
    
    public void RestartToTitle()
    {
        PlayButtonSound();
        SceneManager.LoadScene("TitleScreen");
    }
    
    void PlayButtonSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}