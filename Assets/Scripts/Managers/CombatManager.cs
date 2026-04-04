using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
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

                attack.defenderCS.audioSource.PlayOneShot(attack.defenderCS.missClip);
            }

            yield return new WaitForSeconds(trimTime); //Wait out time trimmed from above
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

            // Determine if the attack is critical
            if (Random.Range(0, 100) < attack.attackerCS.critChance)
            {
                damage *= attack.attackerCS.critMultiplier;
                textColor = Color.yellow;
            }

            text = damage.ToString();

            // Apply damage to the defender
            attack.defenderCS.characterHealth.TakeDamage(damage);
        }
        else
        {

            textColor = Color.white;
            text = "Miss";
        }

        Vector3 defenderLoc= attack.defender.GetComponent<ObjectLocation>().Coord3d();
        Vector3 notificationPos = defenderLoc + new Vector3(0f, attack.defender.transform.position.y, 0f);

        //attack.defender.GetComponent<TextNotificationManager>().CreateNotificationOrder(attack.defender.transform.position, 2f, text, textColor);
        attack.defender.GetComponent<TextNotificationManager>().CreateNotificationOrder(notificationPos, 2f, text, textColor);
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
    }
}