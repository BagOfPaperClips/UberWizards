using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Package package;
    public PlayerMovement player1;
    public PlayerMovement player2;

    public float slideTime = 0.35f;
    public float entryInset = 1f;          // how far the holder is nudged into the new screen
    public float respawnDelay = 1f;
    public float respawnEdgeInset = 1.5f;  // how far from the far edge the other player appears

    Camera cam;
    Vector3 velocity;
    float startX;
    int screenIndex;                       // 0 = start screen, 1 = one right, -1 = one left
    Coroutine respawnRoutine;

    float ScreenWidth => cam.orthographicSize * 2f * cam.aspect;
    float TargetX => startX + screenIndex * ScreenWidth;

    void Start()
    {
        cam = GetComponent<Camera>();
        startX = transform.position.x;
    }

    void LateUpdate()
    {
        PlayerMovement holder = package != null ? package.holder : null;

        if (holder != null)
        {
            // Which screen is the holder standing on right now?
            int holderScreen = Mathf.RoundToInt((holder.transform.position.x - startX) / ScreenWidth);

            if (holderScreen != screenIndex)
            {
                int dir = holderScreen > screenIndex ? 1 : -1;
                screenIndex += dir;   // one screen at a time

                // Nudge the holder inside the new screen so they pop out at the entry side
                Vector2 p = holder.transform.position;
                holder.Teleport(new Vector2(p.x + dir * entryInset, p.y));

                OnScreenChanged(holder, dir);
            }
        }

        Vector3 goal = new Vector3(TargetX, transform.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, goal, ref velocity, slideTime);
    }

    bool IsOnScreen(PlayerMovement p)
    {
        return Mathf.Abs(p.transform.position.x - TargetX) <= ScreenWidth / 2f;
    }

    void OnScreenChanged(PlayerMovement holder, int dir)
    {
        PlayerMovement other = holder == player1 ? player2 : player1;

        if (IsOnScreen(other)) return;   // they kept up, no respawn needed

        if (respawnRoutine != null) StopCoroutine(respawnRoutine);
        respawnRoutine = StartCoroutine(RespawnAhead(other, holder, dir));
    }

    IEnumerator RespawnAhead(PlayerMovement other, PlayerMovement holder, int dir)
    {
        yield return new WaitForSeconds(respawnDelay);

        if (IsOnScreen(other)) yield break;   // they caught up during the wait

        float x = TargetX + dir * (ScreenWidth / 2f - respawnEdgeInset);   // far edge, ahead of the holder
        other.Teleport(new Vector2(x, holder.transform.position.y));
        other.ResetState();
    }
}