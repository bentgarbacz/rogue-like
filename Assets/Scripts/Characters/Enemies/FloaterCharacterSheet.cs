using System.Collections.Generic;
using UnityEngine;

// Enemy that explodes on death and damages nearby characters
public class FloaterCharacterSheet : EnemyCharacterSheet
{

    public float explodeRadius = 2f;
    public int explosionMinDamage = 4;
    public int explosionMaxDamage = 6;
    public GameObject explosionPrefab;
    public AudioClip explosionClip;
    [SerializeField] private Levitating levitating;

    public override void Awake()
    {

        base.Awake();
        maxHealth = 8;
        accuracy = 100;
        minDamage = 1;
        maxDamage = 3;
        level = 3;
        speed = 11;
        evasion = 50;

        characterHealth.InitHealth(maxHealth);

        dropTable = DropTableType.Goblin;
        title = "Floater";

        attackClip = Resources.Load<AudioClip>("Sounds/Frog");
        explosionClip = Resources.Load<AudioClip>("Sounds/Explode");

        levitating.StartLevitating();
    }

    public override void OnDeath()
    {
        
        GameObject explosionObject = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Explosion explosionComponent = explosionObject.GetComponent<Explosion>();

        explosionComponent.InitExplosion(explodeRadius, explosionMinDamage, explosionMaxDamage, this.gameObject);
        explosionComponent.SetExplosion();

        base.OnDeath();
    }
}
