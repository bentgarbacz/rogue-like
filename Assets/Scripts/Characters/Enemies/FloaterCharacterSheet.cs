using System.Collections.Generic;
using UnityEngine;

// Enemy that explodes on death and damages nearby characters
public class FloaterCharacterSheet : EnemyCharacterSheet
{

    public int explodeRadius = 2;
    public int explosionMinDamage = 4;
    public int explosionMaxDamage = 6;
    public GameObject explosionPrefab;
    public AudioClip explosionClip;
    [SerializeField] private Levitating levitating;

    public override void Awake()
    {

        base.Awake();
        stats.maxHealth = 8;
        stats.maxBarrier = 12;
        stats.accuracy = 100;
        stats.minDamage = 1;
        stats.maxDamage = 3;
        level = 3;
        stats.speed = 11;
        stats.evasion = 50;

        dropTable = DropTableType.Goblin;
        title = "Floater";

        attackClip = Resources.Load<AudioClip>("Sounds/Frog");
        explosionClip = Resources.Load<AudioClip>("Sounds/Explode");

        levitating.StartLevitating();
    }

    public override void OnDeath()
    {
        
        GameObject explosionObject = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Explosion explosion = explosionObject.GetComponent<Explosion>();

        explosion.InitExplosion(explodeRadius, explosionMinDamage, explosionMaxDamage, this.gameObject);
        explosion.SetExplosion();

        base.OnDeath();
    }
}
