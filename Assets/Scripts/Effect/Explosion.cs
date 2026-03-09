using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    TurnSequencer ts;
    CombatManager combatMgr;
    EntityManager entityMgr;
    public AudioClip explosionClip;
    public AudioSource audioSource;
    public float explodeRadius = 1f;
    public int explosionMinDamage = 0;
    public int explosionMaxDamage = 1;
    public GameObject exploder;

    void Awake()
    {
        GameObject managers = GameObject.Find("System Managers");
        
        ts = managers.GetComponent<TurnSequencer>();
        combatMgr = managers.GetComponent<CombatManager>();
        entityMgr = managers.GetComponent<EntityManager>();

        explosionClip = Resources.Load<AudioClip>("Sounds/Explode");
    }

    public void InitExplosion(float explodeRadius, int explosionMinDamage, int explosionMaxDamage, GameObject exploder)
    {
        
        this.explodeRadius = explodeRadius;
        this.explosionMinDamage = explosionMinDamage;
        this.explosionMaxDamage = explosionMaxDamage;
        this.exploder = exploder;
    }

    public void SetExplosion()
    {
        
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {

        ts.gameplayHalted = true;
        combatMgr.combatPaused = true;

        audioSource.PlayOneShot(explosionClip);

        foreach (GameObject target in new HashSet<GameObject>(entityMgr.entitiesInLevel))
        {

            if (target == null || target == exploder) continue;

            CharacterSheet targetSheet = target.GetComponent<CharacterSheet>();
            CharacterHealth ch = target.GetComponent<CharacterHealth>();

            if (ch == null || targetSheet == null) continue;

            float distance = Vector3.Distance(target.transform.position, transform.position);

            if (distance <= explodeRadius)
            {

                combatMgr.ExecuteAttack(exploder, target, explosionMinDamage, explosionMaxDamage, 0);
            }
        }

        yield return new WaitForSeconds(0.5f);
            
        ts.gameplayHalted = false;
        combatMgr.combatPaused = false;
        Destroy(this.gameObject);
    }
}
