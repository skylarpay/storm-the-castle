using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClickSound;
    
    private static string previousScene;
    
    // PlaySound method defined FIRST (so other methods can see it)
    void PlaySound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
    
    public void PlayGame()
    {
        PlaySound();
        
        // Reset messages for a fresh game
        PlayerPrefs.DeleteKey("MessagesPlayed");
        PlayerPrefs.Save();
        
        SceneManager.LoadScene("Game_World");
    }
    
    public void OpenControls()
    {
        PlaySound();
        
        // Save which scene we're coming from
        previousScene = SceneManager.GetActiveScene().name;
        
        // Load Controls additively (keeps current scene running)
        SceneManager.LoadScene("ControlScreen", LoadSceneMode.Additive);
        
        // Pause the game while in controls menu
        Time.timeScale = 0f;
    }
    
    public void CloseControls()
    {
        PlaySound();
        
        // Unload the Controls scene
        SceneManager.UnloadSceneAsync("ControlScreen");
        
        // Unpause the game
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        PlaySound();
        
        // Reset time scale first
        Time.timeScale = 1f;
        
        // Reset messages for fresh restart
        PlayerPrefs.DeleteKey("MessagesPlayed");
        PlayerPrefs.Save();
        
        // Reload the current scene to reset progress
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}