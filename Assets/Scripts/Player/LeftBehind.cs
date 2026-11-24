using UnityEngine;
using System.Collections;

public class LeftBehind : MonoBehaviour
{
    public Package package;
    public PlayerMovement player1;
    public PlayerMovement player2;
    public Camera cam;                    // leave empty to use the Main Camera

    public float offscreenMargin = 0.5f;  // how far past the edge counts as off-screen
    public float deathDelay = 0.5f;       // must be off-screen this long before dying
    public float respawnDelay = 1f;
    public float respawnEdgeInset = 1.5f; // how far from the screen edge they reappear
    public CameraFollow cameraFollow;

    float offscreenTimer;
    bool respawning;


    void Start()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        if (respawning) return;

        PlayerMovement holder = package != null ? package.holder : null;

        if (holder == null) { offscreenTimer = 0f; return; }   // nobody has it: no rule

        PlayerMovement other = holder == player1 ? player2 : player1;


        float half = cam.orthographicSize * cam.aspect;
        float dx = other.transform.position.x - cam.transform.position.x;

        if (Mathf.Abs(dx) > half + offscreenMargin)
        {
            offscreenTimer += Time.deltaTime;
            if (offscreenTimer >= deathDelay)
            {
                // Left behind on the left means the holder is heading right, so respawn on the right
                int side = holder.objectiveDirection;
                StartCoroutine(Die(other, holder, side));
            }
        }
        else
        {
            offscreenTimer = 0f;
        }
    }

    IEnumerator Die(PlayerMovement victim, PlayerMovement holder, int side)
    {
        respawning = true;
        offscreenTimer = 0f;

        victim.gameObject.SetActive(false);   // "dead"
       // On the last screen they stay dead. `respawning` stays true so this rule stops running.
        if (cameraFollow != null && cameraFollow.OnLastScreen(holder))
            yield break;
        yield return new WaitForSeconds(respawnDelay);
        if (cameraFollow != null && cameraFollow.OnLastScreen(holder))
            yield break;   // the holder reached the last screen while we were waiting

        float half = cam.orthographicSize * cam.aspect;
        float x = cam.transform.position.x + side * (half - respawnEdgeInset);
        float y = holder.transform.position.y + 0.5f;

        victim.gameObject.SetActive(true);
        victim.Teleport(new Vector2(x, y));
        victim.ResetState();
        RespawnEffect.Spawn(victim.gameObject);

        respawning = false;
    }
}