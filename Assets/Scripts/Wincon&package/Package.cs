using UnityEngine;

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