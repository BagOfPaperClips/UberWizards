using UnityEngine;

// Put this on your spell prefabs (e.g. a fireball script) to receive the direction
public interface ISpell
{
    void Init(Vector2 direction, GameObject caster);
}

[System.Serializable]
public class SpellSlot
{
    public string spellName = "Empty";
    public KeyCode key;
    public float cooldown = 4f;
    public GameObject prefab;
    [HideInInspector] public float nextCastTime;
}

public class SpellCaster : MonoBehaviour
{
    public Vector2 spawnOffset = new Vector2(0.8f, 0f);   // in front of the player; flips with facing
    public SpellSlot[] spells = new SpellSlot[]
    {
        new SpellSlot { spellName = "Spell 1", key = KeyCode.Alpha1 },
        new SpellSlot { spellName = "Spell 2", key = KeyCode.Alpha2 },
        new SpellSlot { spellName = "Spell 3", key = KeyCode.Alpha3 },
        new SpellSlot { spellName = "Spell 4", key = KeyCode.Alpha4 },
    };

    PlayerMovement movement;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        for (int i = 0; i < spells.Length; i++)
        {
            if (Input.GetKeyDown(spells[i].key))
                TryCast(i);
        }
    }

    void TryCast(int index)
    {
        SpellSlot spell = spells[index];

        if (Time.time < spell.nextCastTime) return;
        spell.nextCastTime = Time.time + spell.cooldown;

        int facing = movement != null ? movement.facing : 1;
        Vector2 direction = new Vector2(facing, 0f);

        Debug.Log($"{name} cast {spell.spellName} ({(facing == 1 ? "right" : "left")})");

        if (spell.prefab != null)
        {
            Vector3 pos = transform.position + new Vector3(spawnOffset.x * facing, spawnOffset.y, 0f);
            GameObject obj = Instantiate(spell.prefab, pos, Quaternion.identity);

            // Tell the spell which way to go, if it has a script that wants to know
            ISpell spellScript = obj.GetComponent<ISpell>();
            if (spellScript != null)
                spellScript.Init(direction, gameObject);
        }
    }
}