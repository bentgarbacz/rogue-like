using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicChest : Loot
{

    private NPCGenerator npcGenerator;
    private AudioClip cantSpawnClip;
    private CombatSequencer combatSeq;
    private MiniMapManager miniMapMgr;
    public int minDamage = 2;
    public int maxDamage = 7;
    public int speed = 5;

    void Awake()
    {

        GameObject managers = GameObject.Find("System Managers");
        entityMgr = managers.GetComponent<EntityManager>();
        combatSeq = managers.GetComponent<CombatSequencer>();        
        miniMapMgr = managers.GetComponent<UIActiveManager>().mapPanel.GetComponent<MiniMapManager>();
        npcGenerator = GameObject.Find("Map Generator").GetComponent<NPCGenerator>();

        cantSpawnClip = Resources.Load<AudioClip>("Sounds/clack");
        entityMgr.itemContainers.Add(this.gameObject);
    }

    public override bool Interact()
    {
        
        // Spawn the mimic at this location
        GameObject mimic = npcGenerator.CreateEnemy(NPCType.Mimic, transform.position);
        
        if (mimic != null)
        {

            Attack attack = new(mimic, entityMgr.hero, minDamage, maxDamage, speed);
            combatSeq.ExecuteAttack(attack);

            Vector3 lookDirection = new(entityMgr.hero.transform.position.x, mimic.transform.position.y, entityMgr.hero.transform.position.z);
            mimic.transform.LookAt(lookDirection);

            miniMapMgr.AddIcon(mimic);
            
            entityMgr.aggroEnemies.Add(mimic);
            entityMgr.TossContainer(this.gameObject);
            return true;
        }

        audioSource.PlayOneShot(cantSpawnClip);
        return false;        
    }
}
