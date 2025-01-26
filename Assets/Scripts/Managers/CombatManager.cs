using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    public bool fighting = false;
    public float attackTime = 0.5f;
    public float trimTime = 0.25f;
    private List<Attack> combatBuffer= new();
    private DungeonManager dum;
    private ProjectileReferences allProjectiles;

    void Start()
    {

        GameObject managers = GameObject.Find("System Managers");
        dum = managers.GetComponent<DungeonManager>();
        allProjectiles = managers.GetComponent<ProjectileReferences>();
    }

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

            Attack attack = combatBuffer[0];
            float waitTime = attackTime;

            CharacterSheet attacker = attack.attacker.GetComponent<CharacterSheet>();
            AttackAnimation attackerAnimation = attacker.GetComponent<AttackAnimation>();

            CharacterSheet defender = attack.defender.GetComponent<CharacterSheet>();
            TextNotificationManager defenderNotifier = defender.GetComponent<TextNotificationManager>();

            float hitChance = ((float)attacker.accuracy - (float)defender.evasion) / (float)attacker.accuracy * 100f;
            bool hitSuccessful = Random.Range(0, 100) <= hitChance;

            //turn towards target
            attacker.transform.LookAt(defender.transform);

            if(attack.projectileType is not ProjectileType.None)
            {

                GameObject projectileObject = allProjectiles.CreateProjectile(attack.projectileType, attacker.transform.position, attacker.transform.rotation);
                Projectile projectile = projectileObject.GetComponent<Projectile>();

                float projectileTime = ProjectileAirTime(projectile.speed, attacker.coord, defender.coord);

                if(projectileTime > waitTime)
                {

                    waitTime = projectileTime;
                }

                projectile.Shoot(defender.pos, attacker.audioSource);
            } 

            //Play combat noises
            if(hitSuccessful)
            {

                 //Skip sound for projectile attack, handled by projectile script
                if(attack.projectileType is ProjectileType.None)
                {

                    attacker.audioSource.PlayOneShot(attacker.attackClip);
                }
            
            }else
            {

                defender.audioSource.PlayOneShot(defender.missClip);
            }

            attackerAnimation.MeleeAttack();

            yield return new WaitForSeconds(waitTime - trimTime); //Trim some time here because it feels more responsive

            //Calculate damage 
            if(hitSuccessful)
            {

                int damage = Random.Range(attack.minDamage, attack.maxDamage + 1);
                

                //Determine if attack was critical         
                if(Random.Range(0, 100) < attacker.critChance)
                {

                    damage *= attacker.critMultiplier;
                    defenderNotifier.CreateNotificationOrder(defender.transform.position, 2f, damage.ToString(), Color.yellow);

                }else
                {

                    defenderNotifier.CreateNotificationOrder(defender.transform.position, 2f, damage.ToString(), Color.red);
                }

                defender.TakeDamage(damage);
            
            }else
            {

                //take 0 damage on miss
                defenderNotifier.CreateNotificationOrder(defender.transform.position, 2f, "Miss", Color.white);
            }

            yield return new WaitForSeconds(trimTime); //Wait out time trimmed from above

            //kills defender of attack if it's health falls below 1
            if(defender.health <= 0)
            {
                yield return new WaitForSeconds(0.05f); //Give the GameObject of dead character time to wrap up before it is destroyed
                dum.Smite(combatBuffer[0].defender, defender.pos);                                                                    
            }

            combatBuffer.RemoveAt(0);            
        }
        
        //signals that fighting for the turn is over and regular gameplay can resume
        fighting = false;
    }

    public bool AddMeleeAttack(GameObject attacker, GameObject defender, int minDamage, int maxDamage, int speed)
    {

        //attack occurs only if defender in in a neighboring tile of the attacker
        if(PathFinder.GetNeighbors(defender.GetComponent<CharacterSheet>().coord, dum.dungeonCoords).Contains(attacker.GetComponent<CharacterSheet>().coord))
        {

            combatBuffer.Add( new Attack(attacker, defender, minDamage, maxDamage, speed));
            return true;
        }

        return false;
    }

    public bool AddProjectileAttack(GameObject attacker, GameObject defender, int range, int minDamage, int maxDamage, int speed, ProjectileType projectile)
    {

        float distance = Vector3.Distance(attacker.transform.position, defender.transform.position);

        //attack occurs only if attacker has a line of sight on the target
        if(range >= distance && LineOfSight.HasLOS(attacker, defender))
        {
            
            combatBuffer.Add( new Attack(attacker, defender,minDamage, maxDamage, speed, projectile) );
            return true;
        }

        return false;
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
    public GameObject defender;
    public int minDamage;
    public int maxDamage;
    public int speed;
    public ProjectileType projectileType;

    public Attack(GameObject attacker, GameObject defender, int minDamage, int maxDamage, int speed, ProjectileType projectileType = ProjectileType.None)
    {

        this.attacker = attacker;
        this.defender = defender;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.speed = speed;
        this.projectileType = projectileType;
    }
}