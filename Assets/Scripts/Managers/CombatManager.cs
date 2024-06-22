using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    public bool fighting = false;
    public float attackTime = 0.5f;
    private DungeonManager dum;
    private InventoryManager im;
    private ProjectileManager pm;
    private List<Attack> combatBuffer;    


    void Start()
    {

        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
        im = GameObject.Find("System Managers").GetComponent<InventoryManager>();
        pm = GameObject.Find("System Managers").GetComponent<ProjectileManager>();
        combatBuffer = new List<Attack>();
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

    private IEnumerator CombatTurns()
    {        

        while(combatBuffer.Count > 0)
        {
            
            ProcessAttack(combatBuffer[0]);
            combatBuffer.RemoveAt(0);
            yield return new WaitForSeconds(attackTime);            
        }
        
        //signals that fighting for the turn is over and regular gameplay can commence
        fighting = false;
    }

    public bool AddToCombatBuffer(GameObject attacker, GameObject defender)
    {
        
        bool attackOccured = false;

        Character attackingCharacter = attacker.GetComponent<Character>();
        Character defendingCharacter = defender.GetComponent<Character>();

        Equipment mainHandWeapon = (Equipment)im.equipmentSlotsDictionary["Main Hand"].item;
        Equipment offHandWeapon = (Equipment)im.equipmentSlotsDictionary["Off Hand"].item;

        if(mainHandWeapon == null && offHandWeapon == null && PathFinder.GetNeighbors(defendingCharacter.coord, dum.dungeonCoords).Contains(attackingCharacter.coord) || attackingCharacter is not PlayerCharacter)
        {

            attackOccured = true;
            combatBuffer.Add( new Attack(
                                        attacker, 
                                        defender,
                                        attackingCharacter.minDamage, 
                                        attackingCharacter.maxDamage, 
                                        attackingCharacter.speed
                                        ));

        }

        if(mainHandWeapon != null && attackingCharacter is PlayerCharacter)
        {
            
            if(mainHandWeapon is not RangedWeapon && PathFinder.GetNeighbors(defendingCharacter.coord, dum.dungeonCoords).Contains(attackingCharacter.coord))
            {
                
                attackOccured = true;
                combatBuffer.Add( new Attack(
                                            attacker, 
                                            defender,
                                            mainHandWeapon.bonusStatDictionary["Min Damage"], 
                                            mainHandWeapon.bonusStatDictionary["Max Damage"], 
                                            attackingCharacter.speed
                                            ));

            }else if(mainHandWeapon is RangedWeapon rangedWeapon && mainHandWeapon.bonusStatDictionary["Range"] >= Vector3.Distance(defendingCharacter.transform.position, dum.hero.transform.position))
            {
                
                if(LineOfSight.HasLOS(dum.hero, defender))
                {
                    
                    attackOccured = true;
                    combatBuffer.Add( new Attack(
                                                attacker, 
                                                defender,
                                                mainHandWeapon.bonusStatDictionary["Min Damage"], 
                                                mainHandWeapon.bonusStatDictionary["Max Damage"], 
                                                attackingCharacter.speed,
                                                rangedWeapon.projectile
                                                ));
                }

            }
        }
        

        if(offHandWeapon != null && attackingCharacter is PlayerCharacter)
        {
            if(offHandWeapon is not Shield)
            {

                if(offHandWeapon is not RangedWeapon && PathFinder.GetNeighbors(defendingCharacter.coord, dum.dungeonCoords).Contains(attackingCharacter.coord))
                {
                    
                    attackOccured = true;
                    combatBuffer.Add( new Attack(
                                                attacker, 
                                                defender,
                                                offHandWeapon.bonusStatDictionary["Min Damage"], 
                                                offHandWeapon.bonusStatDictionary["Max Damage"], 
                                                attackingCharacter.speed / 2
                                                ));

                }else if(offHandWeapon is RangedWeapon rangedWeapon && offHandWeapon.bonusStatDictionary["Range"] >= Vector3.Distance(defendingCharacter.transform.position, dum.hero.transform.position))
                {
                    
                    if(LineOfSight.HasLOS(dum.hero, defender))
                    {
                        
                        attackOccured = true;
                        combatBuffer.Add( new Attack(
                                                    attacker, 
                                                    defender,
                                                    offHandWeapon.bonusStatDictionary["Min Damage"], 
                                                    offHandWeapon.bonusStatDictionary["Max Damage"], 
                                                    attackingCharacter.speed / 2,
                                                    rangedWeapon.projectile
                                                    ));
                    }
                }
            }
        }
        
        if(!attackOccured)//move towards defender
        {

            List<Vector2Int> pathToDestination = PathFinder.FindPath(attackingCharacter.coord, defendingCharacter.coord, dum.dungeonCoords);            
            attackingCharacter.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), dum.occupiedlist);
        }

        return attackOccured;
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

    private void ProcessAttack(Attack attack)
    {

        Character attacker = attack.attacker.GetComponent<Character>();
        Character defender = attack.defender.GetComponent<Character>();

        //turn towards target
        //attacker.transform.rotation = Quaternion.Euler(0, Orientation.DetermineRotation(attacker.pos, defender.pos), 0);
        attacker.transform.LookAt(defender.transform);

        if(attack.projectileType != "None")
        {

            GameObject projectile = pm.CreateProjectile(attack.projectileType, attacker.transform.position, attacker.transform.rotation);
            projectile.GetComponent<Projectile>().Shoot(defender.pos, attacker.audioSource);
        }

        float hitChance = ((float)attacker.accuracy - (float)defender.evasion) / (float)attacker.accuracy * 100f;

        //roll to see if you hit
        if(Random.Range(0, 100) <= hitChance)
        {

            //Skip sound for projectile attack, handled by projectile script
            if(attack.projectileType == "None")
            {

                attacker.audioSource.PlayOneShot(attacker.attackClip);
            }

            int damage = Random.Range(attack.minDamage, attack.maxDamage + 1);

            //Determine if attack was critical         
            if(Random.Range(0, 100) < attacker.critChance)
            {

                damage *= attacker.critMultiplier;
                defender.GetComponent<TextPopup>().CreatePopup(defender.transform.position, 2f, damage.ToString(), Color.yellow);

            }else
            {

                defender.GetComponent<TextPopup>().CreatePopup(defender.transform.position, 2f, damage.ToString(), Color.red);
            }

            defender.TakeDamage(damage);
        
        }else
        {

            //take 0 damage on miss
            defender.audioSource.PlayOneShot(defender.missClip);
            defender.GetComponent<TextPopup>().CreatePopup(defender.transform.position, 2f, "Miss", Color.white);
        }

        attacker.GetComponent<AttackAnimation>().MeleeAttack(); 

        //kills defender of attack if it's health falls below 1
        if(defender.health <= 0)
        {
            
            //Attacks to and from dead combatants removed from buffer
            for(int i = 1; i < combatBuffer.Count; i++ )
            {

                if(combatBuffer[0].defender == combatBuffer[i].defender || combatBuffer[0].defender == combatBuffer[i].attacker)
                {
                    
                    combatBuffer.RemoveAt(i);
                    i--;
                }
            }

            dum.Smite(combatBuffer[0].defender, defender.pos);                                                                    
        }
    }
}

public class Attack
{

    public GameObject attacker;
    public GameObject defender;
    public int minDamage;
    public int maxDamage;
    public int speed;
    public string projectileType;

    public Attack(GameObject attacker, GameObject defender, int minDamage, int maxDamage, int speed, string projectileType = "None")
    {

        this.attacker = attacker;
        this.defender = defender;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.speed = speed;
        this.projectileType = projectileType;
    }
}