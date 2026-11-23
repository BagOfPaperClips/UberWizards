using UnityEngine;

public class PackageBlasterSpell : MonoBehaviour, ISpell
{
    public float speed = 14f;
    public float lifetime = 2f;
    public float flightTime = 0.6f;   // how long the package takes to fly to the middle
    public AudioClip castSound;
    public AudioClip hitSound;
    public float volume = 1f;
    [Range(0f, 1f)]
    public float landNearCaster = 0.5f;   // 0 = at the other player, 0.5 = middle, 1 = on the caster

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == caster) return;

        Package pkg = Package.Instance;
        PlayerMovement target = other.GetComponent<PlayerMovement>();
        bool hitHolder = target != null && pkg != null && pkg.holder == target;
        bool hitPackage = pkg != null && other.GetComponentInParent<Package>() == pkg;

        // Ignore other triggers (goal zones, bounds boxes) so the spell flies through them
        if (other.isTrigger && !hitPackage) return;

        if (hitHolder || hitPackage)
        {
            BlastToMiddle(pkg);
            PlaySound(hitSound);
        }

        Destroy(gameObject);
    }

    void BlastToMiddle(Package pkg)
    {
        PlayerMovement casterPlayer = caster != null ? caster.GetComponent<PlayerMovement>() : null;
        PlayerMovement otherPlayer = null;
        foreach (PlayerMovement p in FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None))
            if (p != casterPlayer) otherPlayer = p;

        if (casterPlayer == null || otherPlayer == null) return;

        Vector2 casterPos = casterPlayer.transform.position;
        Vector2 otherPos = otherPlayer.transform.position;
        Vector2 landing = Vector2.Lerp(otherPos, casterPos, landNearCaster);
        pkg.BlastToward(landing, flightTime);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }
}