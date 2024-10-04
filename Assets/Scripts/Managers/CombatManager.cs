using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    public bool fighting = false;
    public float attackTime = 0.5f;
    public float trimTime = 0.25f;
    public List<Attack> combatBuffer= new();
    private DungeonManager dum;
    private InventoryManager im;
    private ProjectileReferences pm;

    void Start()
    {

        GameObject managers = GameObject.Find("System Managers");
        dum = managers.GetComponent<DungeonManager>();
        im = managers.GetComponent<InventoryManager>();
        pm = managers.GetComponent<ProjectileReferences>();
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

            Attack attack = combatBuffer[0];
            float waitTime = attackTime;
            CharacterSheet attacker = attack.attacker.GetComponent<CharacterSheet>();
            CharacterSheet defender = attack.defender.GetComponent<CharacterSheet>();
            float hitChance = ((float)attacker.accuracy - (float)defender.evasion) / (float)attacker.accuracy * 100f;
            bool hitSuccessful = Random.Range(0, 100) <= hitChance;

            //turn towards target
            attacker.transform.LookAt(defender.transform);

            if(attack.projectileType is not ProjectileType.None)
            {

                float projectileSpeed = pm.projectileDict[attack.projectileType].GetComponent<Projectile>().speed;
                Vector2Int attackerCoord = attack.attacker.GetComponent<CharacterSheet>().coord;
                Vector2Int defenderCoord = attack.defender.GetComponent<CharacterSheet>().coord;

                float projectileTime = ProjectileAirTime(projectileSpeed, attackerCoord, defenderCoord);

                if(projectileTime > waitTime)
                {

                    waitTime = projectileTime;
                }

                GameObject projectile = pm.CreateProjectile(attack.projectileType, attacker.transform.position, attacker.transform.rotation);
                projectile.GetComponent<Projectile>().Shoot(defender.pos, attacker.audioSource);
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

            attacker.GetComponent<AttackAnimation>().MeleeAttack();

            yield return new WaitForSeconds(waitTime - trimTime); //Trim some time here because it feels more responsive

            //Calculate damage 
            if(hitSuccessful)
            {

                int damage = Random.Range(attack.minDamage, attack.maxDamage + 1);

                //Determine if attack was critical         
                if(Random.Range(0, 100) < attacker.critChance)
                {

                    damage *= attacker.critMultiplier;
                    defender.GetComponent<TextNotificationManager>().CreateNotificationOrder(defender.transform.position, 2f, damage.ToString(), Color.yellow);

                }else
                {

                    defender.GetComponent<TextNotificationManager>().CreateNotificationOrder(defender.transform.position, 2f, damage.ToString(), Color.red);
                }

                defender.TakeDamage(damage);
            
            }else
            {

                //take 0 damage on miss
                defender.GetComponent<TextNotificationManager>().CreateNotificationOrder(defender.transform.position, 2f, "Miss", Color.white);
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

    public bool AddToCombatBuffer(GameObject attacker, GameObject defender)
    {
        
        bool attackOccured = false;

        CharacterSheet attackingCharacter = attacker.GetComponent<CharacterSheet>();
        CharacterSheet defendingCharacter = defender.GetComponent<CharacterSheet>();

        Equipment mainHandWeapon = (Equipment)im.equipmentSlotsDictionary["Main Hand"].item;
        Equipment offHandWeapon = (Equipment)im.equipmentSlotsDictionary["Off Hand"].item;

        if(mainHandWeapon == null && PathFinder.GetNeighbors(defendingCharacter.coord, dum.dungeonCoords).Contains(attackingCharacter.coord) || attackingCharacter is not PlayerCharacterSheet)
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

        if(mainHandWeapon != null && attackingCharacter is PlayerCharacterSheet)
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
        

        if(offHandWeapon != null && attackingCharacter is PlayerCharacterSheet)
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