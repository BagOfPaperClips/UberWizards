using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransition : MonoBehaviour
{
    [SerializeField] GameObject settings;
    [SerializeField] GameObject credits;

    private void Start()
    {
        settings.SetActive(false);
        credits.SetActive(false);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    // -- SETTINGS -- //
    public void Settings()
    {
        settings.SetActive(true);
        
    }
    public void Back()
    {
        settings.SetActive(false);
    }

    // -- CREDITS -- //
    public void Credits()
    {
        credits.SetActive(true);

    }
    public void Back1()
    {
        credits.SetActive(false);
    }
}
