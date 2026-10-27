using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransition : MonoBehaviour
{
    [SerializeField] GameObject settings;

    private void Start()
    {
        settings.SetActive(false);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Settings()
    {
        settings.SetActive(true);
        
    }
    public void Back()
    {
        settings.SetActive(false);
    }
}
