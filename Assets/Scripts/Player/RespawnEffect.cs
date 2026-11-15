using UnityEngine;

public static class RespawnEffect
{
    public static void Spawn(GameObject who)
    {
        SpriteRenderer sr = who.GetComponent<SpriteRenderer>();
        Color tint = sr != null ? sr.color : Color.white;

        GameObject go = new GameObject("RespawnEffect");
        go.transform.position = who.transform.position;

        ParticleSystem ps = go.AddComponent<ParticleSystem>();
        ps.Stop();   // it auto-plays on creation, so stop it before changing settings

        var main = ps.main;
        main.duration = 0.6f;
        main.loop = false;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.6f, 1.1f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(1.5f, 5f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.08f, 0.25f);
        main.startColor = new ParticleSystem.MinMaxGradient(Color.white, tint);   // random mix of white and the player's color
        main.gravityModifier = -0.3f;   // sparkles drift upward
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = ps.emission;
        emission.rateOverTime = 25f;   // a trickle of sparkles on top of the burst
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 40) });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.5f;

        // Twinkle: spin, shrink to nothing, and fade out
        var rot = ps.rotationOverLifetime;
        rot.enabled = true;
        rot.z = new ParticleSystem.MinMaxCurve(-180f * Mathf.Deg2Rad, 180f * Mathf.Deg2Rad);

        var size = ps.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0f));

        var col = ps.colorOverLifetime;
        col.enabled = true;
        Gradient g = new Gradient();
        g.SetKeys(
            new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 0.6f), new GradientAlphaKey(0f, 1f) });
        col.color = new ParticleSystem.MinMaxGradient(g);

        var rend = go.GetComponent<ParticleSystemRenderer>();
        rend.material = new Material(Shader.Find("Sprites/Default"));
        rend.sortingOrder = 10;

        ps.Play();
        Object.Destroy(go, 2f);
    }
}