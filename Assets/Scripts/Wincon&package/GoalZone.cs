using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalZone : MonoBehaviour
{
    public PlayerMovement onlyFor;   // leave empty = either player can win here
    PlayerMovement winner;

    [SerializeField] GameObject winScreen;

    void OnTriggerStay2D(Collider2D other)
    {
        if (winner != null) return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null) return;
        if (onlyFor != null && player != onlyFor) return;
        if (Package.Instance.holder != player) return;   // must be carrying it

        winner = player;
        Time.timeScale = 0f;   // freeze the game
    }

    void Update()
    {
        if (winner != null && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OnGUI()
    {
        if (winner == null) return;
        winScreen.SetActive(true);
        Time.timeScale = 1f;   // freeze the game
    }
}