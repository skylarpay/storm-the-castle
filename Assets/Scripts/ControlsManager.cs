using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClickSound;
    
    void PlaySound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
    
    public void BackToPreviousScene()
    {
        PlaySound();
        
        // Just unload this scene and unpause
        SceneManager.UnloadSceneAsync("ControlScreen");
        Time.timeScale = 1f;
    }
}