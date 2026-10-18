using UnityEngine;

public class FreezeSpell : MonoBehaviour, ISpell
{
    public float speed = 10f;
    public float slowMultiplier = 0.4f;   // 0.4 = 40% of normal speed
    public float slowDuration = 3f;
    public float lifetime = 3f;           // disappears after this long if it hits nothing

    GameObject caster;

    public void Init(Vector2 direction, GameObject caster)
    {
        this.caster = caster;
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == caster) return;   // don't hit yourself

        PlayerMovement target = other.GetComponent<PlayerMovement>();
        if (target != null)
            target.ApplySlow(slowMultiplier, slowDuration);

        Destroy(gameObject);   // vanish on hitting a player or a wall
    }
}