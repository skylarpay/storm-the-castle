using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1_Mock");
    }
    
    public void OpenControls()
    {
        SceneManager.LoadScene("ControlScreen");
    }
}