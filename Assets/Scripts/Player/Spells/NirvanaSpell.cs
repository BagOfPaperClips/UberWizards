using UnityEngine;

public class NirvanaSpell : MonoBehaviour, ISpell
{
    public float speedMultiplier = 1.5f;   // 1.5 = 50% faster
    public float duration = 3f;
    public AudioClip castSound;
    public float volume = 1f;

    Transform follow;
    Vector3 baseScale;

    void Awake()
    {
        baseScale = transform.localScale;
    }

    public void Init(Vector2 direction, GameObject caster)
    {
        follow = caster.transform;

        PlayerMovement player = caster.GetComponent<PlayerMovement>();
        if (player != null)
            player.ApplyBoost(speedMultiplier, duration);

        if (castSound != null)
            AudioSource.PlayClipAtPoint(castSound, Camera.main.transform.position, volume);

        Destroy(gameObject, duration);
    }

    void Update()
    {
        if (follow == null) return;

        transform.position = follow.position;   // stick to the caster

        // Gentle pulse so the aura looks alive
        float pulse = 1f + 0.1f * Mathf.Sin(Time.time * 12f);
        transform.localScale = baseScale * pulse;
    }
}