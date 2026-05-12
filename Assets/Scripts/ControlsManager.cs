using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsManager : MonoBehaviour
{
    public void BackToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}