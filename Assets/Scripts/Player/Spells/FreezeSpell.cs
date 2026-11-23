using UnityEngine;
using static UnityEngine.Rendering.ReloadAttribute;

public class FreezeSpell : MonoBehaviour, ISpell
{
    public float speed = 10f;
    public float slowMultiplier = 0.4f;   // 0.4 = 40% of normal speed
    public float slowDuration = 3f;
    public float lifetime = 3f;           // disappears after this long if it hits nothing
    public AudioClip castSound;    // plays when the icicle is shot
    public AudioClip hitSound;     // plays when it freezes a player
    public float volume = 1f;

    GameObject caster;

    public void Init(Vector2 direction, GameObject caster)
    {
        this.caster = caster;
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
        PlaySound(castSound);
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    void PlaySound(AudioClip clip)
    {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == caster) return;

        // Fly through the package and through invisible triggers (goal zones, bounds boxes)
        if (other.GetComponentInParent<Package>() != null) return;
        PlayerMovement target = other.GetComponent<PlayerMovement>();
        if (target == null && other.isTrigger) return;

        if (target != null)
        {
            target.ApplySlow(slowMultiplier, slowDuration);
            PlaySound(hitSound);

            //for package drop on freeze
/*            if (Package.Instance != null && Package.Instance.holder == target)
                Package.Instance.Drop();*/
        }

        Destroy(gameObject);   // vanish on a player, wall or the ground
    }
}