using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Package : MonoBehaviour
{
    public static Package Instance;

    public Vector2 carryOffset = new Vector2(0f, 0.9f);
    public float pickupDelay = 1.8f;   // the player who lost it can't regrab it for this long
    public float dropToss = 1.2f;      // small upward pop when dropped
    public PlayerMovement holder;    // null = nobody has it

    Rigidbody2D rb;
    PlayerMovement lastHolder;
    float lastHolderBlockedUntil;

    public float stealCooldown = 1.5f;    // after any grab, nobody can steal it for this long
    public float stealDistance = 0.05f;   // how close the players' colliders must be to count as touching

    PlayerMovement[] players;
    float stealBlockedUntil;

    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        // Only the SOLID collider ignores the players. The trigger must stay active to detect them.
        foreach (PlayerMovement p in FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None))
            foreach (Collider2D pc in p.GetComponents<Collider2D>())
                foreach (Collider2D c in GetComponents<Collider2D>())
                    if (!c.isTrigger)
                        Physics2D.IgnoreCollision(c, pc);

        players = FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
    }

    void Update()
    {
        if (holder == null || Time.time < stealBlockedUntil) return;

        Collider2D holderCol = holder.GetComponent<Collider2D>();

        foreach (PlayerMovement p in players)
        {
            if (p == holder || !p.gameObject.activeInHierarchy) continue;

            Collider2D otherCol = p.GetComponent<Collider2D>();
            if (holderCol.Distance(otherCol).distance <= stealDistance)
            {
                PickUp(p);   // takes it straight off the other player
                break;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (holder != null) return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player == null) return;
        if (player == lastHolder && Time.time < lastHolderBlockedUntil) return;

        PickUp(player);
    }

    void PickUp(PlayerMovement player)
    {
        holder = player;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;   // no physics while carried
        transform.SetParent(player.transform);
        transform.localPosition = carryOffset;
        stealBlockedUntil = Time.time + stealCooldown;
    }

    public void BlastToward(Vector2 target, float flightTime)
    {
        if (holder != null) Drop();   // knock it loose first

        Vector2 start = transform.position;
        float g = Physics2D.gravity.y * rb.gravityScale;   // gravity acting on the package (negative)

        // Pick a velocity that lands the package on the target after flightTime seconds
        float vx = (target.x - start.x) / flightTime;
        float vy = (target.y - start.y - 0.5f * g * flightTime * flightTime) / flightTime;
        rb.linearVelocity = new Vector2(vx, vy);
    }


    public void Drop()
    {
        if (holder == null) return;

        lastHolder = holder;
        lastHolderBlockedUntil = Time.time + pickupDelay;

        holder = null;
        transform.SetParent(null);
        rb.simulated = true;
        rb.linearVelocity = new Vector2(0.1f, 3f);
    }
}