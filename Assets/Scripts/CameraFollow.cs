using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour
{
    public Package package;
    public PlayerMovement player1;
    public PlayerMovement player2;
    public Transform boundsParent;        // drag the "Bounds" object here

    public float followSmoothTime = 0.15f;
    public float entryInset = 1.5f;       // how far inside the new box the holder appears
    public float backEdgeMargin = 0.5f;   // how close to the back edge the holder can get
    public float respawnDelay = 1f;
    public float respawnEdgeInset = 1.5f;

    public float fadeTime = 0.2f;    // fade out and fade in, each
    public float blackTime = 0.3f;   // how long it stays fully black

    bool transitioning;
    float fade;                     

    Camera cam;
    List<Collider2D> sections = new List<Collider2D>();
    int index;                            // which bounds box we're in
    float startY;
    float velocityX;
    Coroutine respawnRoutine;

    float HalfView => cam.orthographicSize * cam.aspect;

    void Start()
    {
        cam = GetComponent<Camera>();
        startY = transform.position.y;

        sections.AddRange(boundsParent.GetComponentsInChildren<Collider2D>());
        sections.Sort((a, b) => a.bounds.center.x.CompareTo(b.bounds.center.x));   // left to right

        // Start on the box nearest the camera
        float best = float.MaxValue;
        for (int i = 0; i < sections.Count; i++)
        {
            float d = Mathf.Abs(sections[i].bounds.center.x - transform.position.x);
            if (d < best) { best = d; index = i; }
        }
    }

    void LateUpdate()
    {
        if (transitioning) return;
        PlayerMovement holder = package != null ? package.holder : null;
        if (holder == null) return;   // nobody has the package: camera stays put

        Bounds b = sections[index].bounds;
        Vector2 p = holder.transform.position;

        // The holder can't walk back out of the edge they entered from
        if (holder.objectiveDirection == 1 && p.x < b.min.x + backEdgeMargin)
        {
            p.x = b.min.x + backEdgeMargin;
            holder.Teleport(p);
        }
        else if (holder.objectiveDirection == -1 && p.x > b.max.x - backEdgeMargin)
        {
            p.x = b.max.x - backEdgeMargin;
            holder.Teleport(p);
        }

        // Holder reached the edge of the box: switch screens
        int dir = p.x > b.max.x ? 1 : (p.x < b.min.x ? -1 : 0);
        if (dir != 0 && index + dir >= 0 && index + dir < sections.Count)
        {
            StartCoroutine(ChangeSection(holder, dir, b));
            return;
        }

        // Otherwise follow the holder, staying inside the current box
        float x = Mathf.SmoothDamp(transform.position.x, FollowTarget(holder), ref velocityX, followSmoothTime);
        transform.position = new Vector3(x, startY, transform.position.z);
    }

    float FollowTarget(PlayerMovement holder)
    {
        Bounds cur = sections[index].bounds;
        float minX = cur.min.x + HalfView;
        float maxX = cur.max.x - HalfView;
        if (minX > maxX) return cur.center.x;   // box narrower than the camera
        return Mathf.Clamp(holder.transform.position.x, minX, maxX);
    }

    IEnumerator ChangeSection(PlayerMovement holder, int dir, Bounds oldBounds)
    {
        transitioning = true;
        Time.timeScale = 0f;   // freeze the game during the blackout

        // Fade to black
        for (float t = 0f; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            fade = t / fadeTime;
            yield return null;
        }
        fade = 1f;

        // Switch screens while it's black
        index += dir;
        Bounds nb = sections[index].bounds;

        float entryX = dir == 1 ? nb.min.x + entryInset : nb.max.x - entryInset;
        holder.Teleport(new Vector2(entryX, holder.transform.position.y));
        holder.ResetState();

        velocityX = 0f;
        transform.position = new Vector3(FollowTarget(holder), startY, transform.position.z);

        // Was the other player shoved across the same edge?
        PlayerMovement other = holder == player1 ? player2 : player1;
        float oldEdge = dir == 1 ? oldBounds.max.x : oldBounds.min.x;
        bool pushed = (other.transform.position.x - oldEdge) * dir > 0f;

        yield return new WaitForSecondsRealtime(blackTime);

        // Fade back in
        for (float t = 0f; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            fade = 1f - t / fadeTime;
            yield return null;
        }
        fade = 0f;

        Time.timeScale = 1f;
        transitioning = false;

        if (pushed)
        {
            if (respawnRoutine != null) StopCoroutine(respawnRoutine);
            respawnRoutine = StartCoroutine(RespawnPushed(other, holder, dir));
        }
    }
    void OnGUI()
    {
        if (fade <= 0f) return;
        GUI.color = new Color(0f, 0f, 0f, fade);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
    }

    IEnumerator RespawnPushed(PlayerMovement other, PlayerMovement holder, int dir)
    {
        other.gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnDelay);

        float x = transform.position.x + dir * (HalfView - respawnEdgeInset);
        other.gameObject.SetActive(true);
        other.Teleport(new Vector2(x, holder.transform.position.y));
        other.ResetState();
    }
}