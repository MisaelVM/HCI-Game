using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityAction { ATTACK, DEFEND, SPECIAL, IDLE }

public class EntityAttributes : MonoBehaviour
{
    public string entityName = "Mob";

    public int maxHealth = 100;
    public int health = 100;

    public int atk = 20;
    public int def = 15;

    public EntityAction action = EntityAction.IDLE;

    public int TakeDamage(EntityAttributes attacker) {
        int damage = Mathf.Max((attacker.atk - this.def) / ((action == EntityAction.DEFEND) ? 2 : 1), 1);
        health = Mathf.Max(health - damage, 0);

        return damage;
    }
}
