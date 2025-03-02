using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacterSheet : CharacterSheet
{
    public int hunger;
    public int maxHunger = 1000;
    private int hungerBuffer = 0;
    public int totalXP = 0;
    public int mana;
    public int maxMana = 30;
    public int levelUpBreakpoint = 50;
    public int freeStatPoints = 0;
    public AudioClip stepAudioClip;
    public AudioClip levelUpAudioClip;
    public int accuracyBonus = 0;
    public int minDamageBonus = 0;
    public int maxDamageBonus = 0;
    public int speedBonus = 0;
    public int critChanceBonus = 0;
    public int critMultiplierBonus = 0;
    public int vitalityBonus = 0;
    public int strengthBonus = 0;
    public int dexterityBonus = 0;
    public int intelligenceBonus = 0;
    public int armorBonus = 0;
    public int evasionBonus = 0;
    public int maxHealthBonus = 0;
    public int maxManaBonus = 0;
    public Dictionary<SpellType, int> knownSpells = new();
    private DungeonManager dum;
    private InventoryManager im;
    private CombatManager cbm;
    [SerializeField] private UpdateUIElements updateStats;
    [SerializeField] private StatusNotificationManager snm;

    public override void Start()
    {
        
        base.Start();
        maxHealth = 20;
        health = maxHealth;
        accuracy = 1000;
        minDamage = 1;
        maxDamage = 3;
        level = 1;
        speed = 10;
        hunger = maxHunger;
        mana = maxMana;
        armor = 0;
        evasion = 50;
        
        attackClip = Resources.Load<AudioClip>("Sounds/Strike");
        stepAudioClip = Resources.Load<AudioClip>("Sounds/Step");
        levelUpAudioClip = Resources.Load<AudioClip>("Sounds/LevelUp");

        GetComponent<MoveToTarget>().SetNoise(audioSource, stepAudioClip);

        GameObject managers = GameObject.Find("System Managers");
        dum = managers.GetComponent<DungeonManager>();
        im = managers.GetComponent<InventoryManager>();
        cbm = managers.GetComponent<CombatManager>();        

        updateStats.RefreshUI();
    }

    public void GainXP(int XP)
    {

        totalXP += XP;

        if(totalXP >= levelUpBreakpoint)
        {

            GetComponent<TextNotificationManager>().CreateNotificationOrder(transform.position, 3f, "Level Up!", Color.yellow);

            while(totalXP >= levelUpBreakpoint)
            {

                LevelUp();
                totalXP -= levelUpBreakpoint;
                levelUpBreakpoint += ((2 * levelUpBreakpoint) / 3);
            }

        }else
        {

            GetComponent<TextNotificationManager>().CreateNotificationOrder(transform.position, 2f, XP.ToString() + " XP", Color.green);
        }

        updateStats.RefreshUI();
    }

    public void RegainMana(int regainValue)
    {

        mana = System.Math.Min(maxMana, mana + regainValue);

        updateStats.RefreshUI();
    }

    public void LevelUp()
    {
        
        audioSource.PlayOneShot(levelUpAudioClip);

        Heal(maxHealth / 4);
        freeStatPoints += 5;

        level += 1;

        updateStats.RefreshUI();
    }

    public int GetCurrentLevelUpBreakpoint()
    {

        return levelUpBreakpoint;
    }

    public void SatiateHunger(int hungerValue)
    {

        hunger = System.Math.Min(maxHunger, hunger + hungerValue);

        UpdateUI();
    }

    public void BecomeHungrier(int hungerValue = 1)
    {

        hunger -= hungerValue;
        hungerBuffer += 1;

        if(hungerBuffer >= 10)
        {
            if(hunger > 0)
            {

                Heal(1);
                int regainManaCheck = UnityEngine.Random.Range(0, 2);
                
                if(regainManaCheck == 1)
                {

                    RegainMana(1);
                }

            }else
            {

                hunger = 0;
                TakeDamage(1);
                GetComponent<TextNotificationManager>().CreateNotificationOrder(transform.position, 2f, "Hungry!", Color.red);
            }

            hungerBuffer = 0;
        }

        UpdateUI();
    }

    public void DecrementCooldowns()
    {

        foreach(SpellType spellType in knownSpells.Keys.ToList())
        {

            if(knownSpells[spellType] > 0)
            {   

                knownSpells[spellType] -= 1;
            }
        }
    }

    public bool AttackCharacter(GameObject defender)
    {

        bool attackOccured = false;
        bool attackResult;

        CharacterSheet defendingCharacter = defender.GetComponent<CharacterSheet>();

        Equipment mainHandWeapon = (Equipment)im.equipmentSlotsDictionary[ItemSlotType.MainHand].item;
        Equipment offHandWeapon = (Equipment)im.equipmentSlotsDictionary[ItemSlotType.OffHand].item;

        if(mainHandWeapon == null && (offHandWeapon == null || offHandWeapon is Shield))
        {

            attackResult = cbm.AddMeleeAttack(
                                              this.gameObject,
                                              defender,
                                              minDamage, 
                                              maxDamage,
                                              speed
                                             );

            attackOccured = attackResult || attackOccured;
  
        }else 
        {
            
            if(mainHandWeapon != null)
            {
                
                if(mainHandWeapon is not RangedWeapon)
                {
                    
                    attackResult = cbm.AddMeleeAttack(
                                                      this.gameObject,
                                                      defender,
                                                      mainHandWeapon.bonusStatDictionary[StatType.MinDamage], 
                                                      mainHandWeapon.bonusStatDictionary[StatType.MaxDamage],
                                                      speed
                                                     );

                    attackOccured = attackResult || attackOccured; 

                }else if(mainHandWeapon is RangedWeapon rangedWeapon)
                {
                    
                    attackResult = cbm.AddProjectileAttack( 
                                                            this.gameObject, 
                                                            defender, 
                                                            rangedWeapon.bonusStatDictionary[StatType.Range], 
                                                            rangedWeapon.bonusStatDictionary[StatType.MinDamage], 
                                                            rangedWeapon.bonusStatDictionary[StatType.MaxDamage],
                                                            speed,
                                                            rangedWeapon.projectile
                                                           );   

                    attackOccured = attackResult || attackOccured;                  
                }
            }
            

            if(offHandWeapon != null && offHandWeapon is not Shield)
            {

                if(offHandWeapon is not RangedWeapon)
                {
                    
                    attackResult = cbm.AddMeleeAttack(
                                                      this.gameObject,
                                                      defender,
                                                      offHandWeapon.bonusStatDictionary[StatType.MinDamage], 
                                                      offHandWeapon.bonusStatDictionary[StatType.MaxDamage],
                                                      speed / 2
                                                     );

                    attackOccured = attackResult || attackOccured;

                }else if(offHandWeapon is RangedWeapon rangedWeapon)
                {
                                                
                    attackResult = cbm.AddProjectileAttack( 
                                                            this.gameObject, 
                                                            defender, 
                                                            rangedWeapon.bonusStatDictionary[StatType.Range], 
                                                            rangedWeapon.bonusStatDictionary[StatType.MinDamage], 
                                                            rangedWeapon.bonusStatDictionary[StatType.MaxDamage],
                                                            speed / 2,
                                                            rangedWeapon.projectile
                                                          );       

                    attackOccured = attackResult || attackOccured;             
                }
            }
        }
        
        if(!attackOccured)//move towards defender
        {

            List<Vector2Int> pathToDestination = PathFinder.FindPath(coord, defendingCharacter.coord, dum.dungeonCoords);            
            Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), dum.occupiedlist);
        }

        return attackOccured;
    }

    public void UpdateUI()
    {

        updateStats.RefreshUI();
    }

    public override void ProcessStatusEffects()
    {

        sem.ProcessStatusEffects();
        UpdateStatusNotifications();
    }

    public void UpdateStatusNotifications()
    {

        snm.UpdateStatusNotifications(sem.GetStatusEffects());
    }

    public override void Heal(int healValue)
    {

        base.Heal(healValue);
        UpdateUI();
    }

    public override int TakeDamage(int damage)
    {

        int damageTaken = base.TakeDamage(damage);
        UpdateUI();

        return damageTaken;
    }
}