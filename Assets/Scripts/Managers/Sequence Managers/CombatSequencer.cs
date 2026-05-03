using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSequencer : MonoBehaviour
{

    public bool fighting = false;
    private bool combatLock = false;
    private int combatLockCount = 0;
    public float attackTime = 0.5f;
    public float trimTime = 0.25f;
    private List<Attack> combatBuffer = new();
    [SerializeField] private EntityManager entityMgr;
    [SerializeField] private TileManager tileMgr;
    [SerializeField] private ProjectileReferences allProjectiles;

    public void CommenceCombat()
    {

        if(combatBuffer.Count > 0 && fighting == false)
        {
            
            //Initiates combat and notifies listeners
            fighting = true;
            SortBuffer();
            StartCoroutine(CombatTurns());
        }
    }

    public void IncrementCombatLock(int count = 1)
    {
        
        if(count < 1)
        {
            
            return;
        }

        combatLockCount += count;

        if(combatLockCount > 0)
        {
            
            combatLock = true;
        }
    }

    public void DecrementCombatLock(int count = 1)
    {
        
        if(count < 1)
        {
            
            return;
        }

        combatLockCount = Mathf.Max(combatLockCount - count, 0);

        if(combatLockCount == 0)
        {
            
            combatLock = false;
        }
    }

    public bool CombatLockState()
    {
        
        return combatLock;
    }

    public bool AttacksAreQueued()
    {
        
        return combatBuffer.Count > 0;
    }

    //Removes target's attacks from combat buffer
    public void PruneCombatBuffer(GameObject target)
    {

        for(int i = 1; i < combatBuffer.Count; i++ )
        {

            if(target == combatBuffer[i].defender || target == combatBuffer[i].attacker)
            {
                
                combatBuffer.RemoveAt(i);
                i--;
            }
        }
    }

    private IEnumerator CombatTurns()
    {        

        while(combatBuffer.Count > 0)
        {

            if(combatLock)
            {
                yield return null;
                continue;
            }

            Attack attack = combatBuffer[0];
            float waitTime = attackTime;

            AttackAnimation attackerAnimation = attack.attacker.GetComponent<AttackAnimation>();

            Vector3 lookDirection = new(attack.defender.transform.position.x, attack.attacker.transform.position.y, attack.defender.transform.position.z);
            attack.attacker.transform.LookAt(lookDirection);

            if(attack.projectileType is not ProjectileType.None)
            {

                GameObject projectileObject = allProjectiles.CreateProjectile(attack.projectileType, attack.attacker.transform.position, attack.attacker.transform.rotation);
                Projectile projectile = projectileObject.GetComponent<Projectile>();

                float projectileTime = ProjectileAirTime(projectile.speed, attack.attackerCS.loc.coord, attack.defenderCS.loc.coord);

                if(projectileTime > waitTime)
                {

                    waitTime = projectileTime;
                }

                projectile.Shoot(attack.defender, attack.attackerCS.audioSource);
            } 


            attackerAnimation.MeleeAttack();

            yield return new WaitForSeconds(waitTime - trimTime); //Trim some time here because it feels more responsive
            

            
            if(ExecuteAttack(attack))
            {

                 //Skip sound for projectile attack, handled by projectile script
                if(attack.projectileType is ProjectileType.None)
                {

                    attack.attackerCS.audioSource.PlayOneShot(attack.attackerCS.attackClip);
                }
            
            }else
            {

                attack.attackerCS.audioSource.PlayOneShot(attack.attackerCS.missClip);
            }

            yield return new WaitForSeconds(trimTime); //Wait out time trimmed from above
            combatBuffer.RemoveAt(0);            
        }
        
        //signals that fighting for the turn is over and regular gameplay can resume
        fighting = false;
    }

    public bool CheckMeleeAttackValidity(GameObject attacker, GameObject defender)
    {

        HashSet<Vector2Int> targetNeighbors = PathFinder.GetNeighbors(defender.GetComponent<CharacterSheet>().loc.coord, tileMgr.levelCoords);

        return targetNeighbors.Contains(attacker.GetComponent<CharacterSheet>().loc.coord);
    }

    public bool CheckProjectileAttackValidity(GameObject attacker, GameObject defender, int range)
    {

        float distance = Vector3.Distance(attacker.transform.position, defender.transform.position);

        return range >= distance && LineOfSight.HasLOS(attacker, defender);
    }

    public void AddAttack(Attack attack)
    {

        combatBuffer.Add(attack);
    }

    public bool ExecuteAttack(Attack attack)
    {

        if(attack.defender == null)
        {

            return false;
        }

        // Calculate hit chance
        float hitChance = ((float)attack.attackerCS.accuracy - (float)attack.defenderCS.evasion) / (float)attack.attackerCS.accuracy * 100f;
        bool hitSuccessful = Random.Range(0, 100) <= hitChance;
        string text;
        Color textColor = Color.red;

        if (hitSuccessful)
        {
            // Calculate damage
            int damage = Random.Range(attack.minDamage, attack.maxDamage + 1);            

            float multiplier = attack.attackerCS.damageDealtMultiplier * attack.defenderCS.damageTakenMultiplier;

            damage = (int)(damage * multiplier);    

            // Determine if the attack is critical
            if (Random.Range(0, 100) < attack.attackerCS.critChance)
            {
                damage *= attack.attackerCS.critMultiplier;
                textColor = Color.yellow;
            }

            text = damage.ToString();

            // Apply damage to the defender
            attack.defenderCS.characterHealth.TakeDamage(damage);

            // Resolve status effects
            attack.ResolveEffects();

        }
        else
        {

            textColor = Color.white;
            text = "Miss";
        }

        
        //attack.defender.GetComponent<TextNotificationManager>().CreateNotificationOrder(attack.defender.transform.position, 2f, text, textColor);
        attack.defender.GetComponent<TextNotificationManager>().CreateNotificationOrder(2f, text, textColor);
        return hitSuccessful;
    }

    private void SortBuffer()
    {
        //sort combat buffer so fastest characters attack first
        int bufferIndex = combatBuffer.Count;

        while(bufferIndex > 1)
        {

            for(int i = 0; i < bufferIndex - 1; i++)
            {

                if(combatBuffer[i].speed < combatBuffer[i+1].speed)
                {

                    (combatBuffer[i], combatBuffer[i+1]) = (combatBuffer[i+1], combatBuffer[i]);
                }
            }

            bufferIndex--;
        }
    }

    private float ProjectileAirTime(float projectileSpeed, Vector2Int originCoord, Vector2Int destinationCoord)
    {

        return PathFinder.CalculateDistance(originCoord, destinationCoord) / projectileSpeed;
    }
}

public class Attack
{

    public GameObject attacker;
    public CharacterSheet attackerCS;
    public GameObject defender;
    public CharacterSheet defenderCS;
    public int minDamage;
    public int maxDamage;
    public int speed;
    public ProjectileType projectileType;
    public Dictionary<StatusEffect, float> statusEffects;

    public Attack(GameObject attacker, GameObject defender, int minDamage, int maxDamage, int speed, ProjectileType projectileType = ProjectileType.None)
    {

        this.attacker = attacker;
        attackerCS = attacker.GetComponent<CharacterSheet>();

        this.defender = defender;
        defenderCS = defender.GetComponent<CharacterSheet>();

        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.speed = speed;
        this.projectileType = projectileType;
        this.statusEffects = new();
    }

    public void AttachStatusEffect(StatusEffect statusEffect, float procChance)
    {

        //Percentage chance to proc is stored as a value between 1 and 0, where 1 = 100% and 0 = 0%
        procChance = Mathf.Max(procChance, 0);
        procChance = Mathf.Min(procChance, 1);

        statusEffects.Add(statusEffect, procChance);
    }

    public void ResolveEffects()
    {

        foreach (StatusEffect currentEffect in statusEffects.Keys)
        {

            if (Random.Range(0f, 1f) <= statusEffects[currentEffect])
            {
                
                defenderCS.statusEffectMgr.AddEffect(currentEffect);
            }
        }
    }
}
