using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    public bool fighting = false;
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
            CharacterHealth attackerHealth = attack.attacker.GetComponent<CharacterHealth>();
            AttackAnimation attackerAnimation = attacker.GetComponent<AttackAnimation>();

            CharacterSheet defender = attack.defender.GetComponent<CharacterSheet>();
            CharacterHealth defenderHealth = attack.defender.GetComponent<CharacterHealth>();
            TextNotificationManager defenderNotifier = defender.GetComponent<TextNotificationManager>();

            float hitChance = ((float)attacker.accuracy - (float)defender.evasion) / (float)attacker.accuracy * 100f;
            bool hitSuccessful = Random.Range(0, 100) <= hitChance;

            Vector3 lookDirection = new(defender.transform.position.x, attacker.transform.position.y, defender.transform.position.z);
            attacker.transform.LookAt(lookDirection);

            if(attack.projectileType is not ProjectileType.None)
            {

                GameObject projectileObject = allProjectiles.CreateProjectile(attack.projectileType, attacker.transform.position, attacker.transform.rotation);
                Projectile projectile = projectileObject.GetComponent<Projectile>();

                float projectileTime = ProjectileAirTime(projectile.speed, attacker.loc.coord, defender.loc.coord);

                if(projectileTime > waitTime)
                {

                    waitTime = projectileTime;
                }

                projectile.Shoot(defender.gameObject, attacker.audioSource);
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
            //yield return new WaitForSeconds(waitTime);

            if(defender != null)
            {

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

                    defenderHealth.TakeDamage(damage);
                
                }else
                {

                    //take 0 damage on miss
                    defenderNotifier.CreateNotificationOrder(defender.transform.position, 2f, "Miss", Color.white);
                }

                yield return new WaitForSeconds(trimTime); //Wait out time trimmed from above
            }

            combatBuffer.RemoveAt(0);            
        }
        
        //signals that fighting for the turn is over and regular gameplay can resume
        fighting = false;
    }

    public bool AddMeleeAttack(GameObject attacker, GameObject defender, int minDamage, int maxDamage, int speed)
    {

        //attack occurs only if defender in in a neighboring tile of the attacker
        if(PathFinder.GetNeighbors(defender.GetComponent<CharacterSheet>().loc.coord, tileMgr.levelCoords).Contains(attacker.GetComponent<CharacterSheet>().loc.coord))
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

    public void ExecuteAttack(GameObject attacker, GameObject defender, int minDamage, int maxDamage, int speed, ProjectileType projectileType = ProjectileType.None)
    {
        // Get the CharacterSheet components of the attacker and defender
        CharacterSheet attackerSheet = attacker.GetComponent<CharacterSheet>();
        CharacterHealth attackerHealth = attacker.GetComponent<CharacterHealth>();

        CharacterSheet defenderSheet = defender.GetComponent<CharacterSheet>();
        CharacterHealth defenderHealth = defender.GetComponent<CharacterHealth>();        
        TextNotificationManager defenderNotifier = defender.GetComponent<TextNotificationManager>();

        // Calculate hit chance
        float hitChance = ((float)attackerSheet.accuracy - (float)defenderSheet.evasion) / (float)attackerSheet.accuracy * 100f;
        bool hitSuccessful = Random.Range(0, 100) <= hitChance;

        if (hitSuccessful)
        {
            // Calculate damage
            int damage = Random.Range(minDamage, maxDamage + 1);

            // Determine if the attack is critical
            if (Random.Range(0, 100) < attackerSheet.critChance)
            {
                damage *= attackerSheet.critMultiplier;
                defenderNotifier.CreateNotificationOrder(defender.transform.position, 2f, damage.ToString(), Color.yellow); // Critical hit notification
            }
            else
            {
                defenderNotifier.CreateNotificationOrder(defender.transform.position, 2f, damage.ToString(), Color.red); // Regular hit notification
            }

            // Apply damage to the defender
            defenderHealth.TakeDamage(damage);
        }
        else
        {
            
            // Miss notification
            defenderNotifier.CreateNotificationOrder(defender.transform.position, 2f, "Miss", Color.white);
        }
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