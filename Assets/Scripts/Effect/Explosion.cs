using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    private TurnSequencer ts;
    private CombatSequencer combatSeq;
    private TileManager tileMgr;
    private LockManager lockMgr;
    public AudioClip explosionClip;
    public AudioSource audioSource;
    public int explodeRadius = 1;
    public int explosionMinDamage = 0;
    public int explosionMaxDamage = 1;
    public GameObject exploder;

    void Awake()
    {
        GameObject managers = GameObject.Find("System Managers");
        
        ts = managers.GetComponent<TurnSequencer>();
        combatSeq = managers.GetComponent<CombatSequencer>();
        tileMgr = managers.GetComponent<TileManager>();

        lockMgr = GetComponent<LockManager>();

        explosionClip = Resources.Load<AudioClip>("Sounds/Explode");
    }

    public void InitExplosion(int explodeRadius, int explosionMinDamage, int explosionMaxDamage, GameObject exploder)
    {
        
        this.explodeRadius = explodeRadius;
        this.explosionMinDamage = explosionMinDamage;
        this.explosionMaxDamage = explosionMaxDamage;
        this.exploder = exploder;
    }

    public void SetExplosion()
    {
        
        lockMgr.AcquireCombatLock();
        lockMgr.AcquireTurnLock();

        Vector2Int explosionCenter = exploder.GetComponent<ObjectLocation>().coord;
        List<Tile> affectedTiles = tileMgr.GetTilesInRadius(explosionCenter, explodeRadius);

        foreach (Tile tile in affectedTiles)
        {

            foreach (GameObject target in new HashSet<GameObject>(tile.entitiesOnTile))
            {

                if (target == null || target == exploder) continue;

                CharacterHealth ch = target.GetComponent<CharacterHealth>();

                if (ch == null ) continue;

                Attack attack = new(exploder, target, explosionMinDamage, explosionMaxDamage, 0);
                combatSeq.ExecuteAttack(attack);
            }
        }

        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {

        audioSource.PlayOneShot(explosionClip);

        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }
}
