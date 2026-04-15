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
    public int visibilityRadius = 5;
    public Dictionary<SpellType, int> knownSpells = new();
    private InventoryManager im;
    private CombatSequencer combatSeq;
    [SerializeField] private UpdateUIElements updateStats;
    [SerializeField] private StatusNotificationManager snm;
    [SerializeField] private TurnSequencer turnSequencer;

    public override void Awake()
    {

        base.Awake();
        maxHealth = 20;
        accuracy = 1000;
        minDamage = 1;
        maxDamage = 3;
        level = 1;
        speed = 10;
        hunger = maxHunger;
        mana = maxMana;
        armor = 0;
        evasion = 50;
        title = "Player";

        characterHealth.InitHealth(maxHealth);

        attackClip = Resources.Load<AudioClip>("Sounds/Strike");
        stepAudioClip = Resources.Load<AudioClip>("Sounds/Step");
        levelUpAudioClip = Resources.Load<AudioClip>("Sounds/LevelUp");

        GetComponent<MoveToTarget>().SetNoise(audioSource, stepAudioClip);

        GameObject managers = GameObject.Find("System Managers");
        entityMgr = managers.GetComponent<EntityManager>();
        im = managers.GetComponent<InventoryManager>();
        combatSeq = managers.GetComponent<CombatSequencer>();

        updateStats.RefreshUI();
    }

    public void GainXP(int XP)
    {

        Vector3 notificationPos = GetComponent<ObjectLocation>().Coord3d() + new Vector3(0f, transform.position.y, 0f);

        totalXP += XP;

        if (totalXP >= levelUpBreakpoint)
        {

            GetComponent<TextNotificationManager>().CreateNotificationOrder(notificationPos, 3f, "Level Up!", Color.yellow);

            while (totalXP >= levelUpBreakpoint)
            {

                LevelUp();
                totalXP -= levelUpBreakpoint;
                levelUpBreakpoint += (2 * levelUpBreakpoint) / 3;
            }

        }
        else
        {

            GetComponent<TextNotificationManager>().CreateNotificationOrder(notificationPos, 2f, XP.ToString() + " XP", Color.green);
        }

        updateStats.RefreshUI();
    }

    public void GainMana(int gainValue)
    {

        mana = System.Math.Min(maxMana, mana + gainValue);

        updateStats.RefreshUI();
    }

    public void LevelUp()
    {

        audioSource.PlayOneShot(levelUpAudioClip);

        characterHealth.Heal(maxHealth / 4);
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

        if (hungerBuffer >= 10)
        {
            if (hunger > 0)
            {

                characterHealth.Heal(1);
                int gainManaCheck = UnityEngine.Random.Range(0, 2);

                if (gainManaCheck == 1)
                {

                    GainMana(1);
                }

            }
            else
            {

                hunger = 0;
                characterHealth.TakeDamage(1);
                GetComponent<TextNotificationManager>().CreateNotificationOrder(transform.position, 2f, "Hungry!", Color.red);
            }

            hungerBuffer = 0;
        }

        UpdateUI();
    }

    public void DecrementCooldowns()
    {

        foreach (SpellType spellType in knownSpells.Keys.ToList())
        {

            if (knownSpells[spellType] > 0)
            {

                knownSpells[spellType] -= 1;
            }
        }
    }

    private (int minDamage, int maxDamage) GetWeaponDamage(Equipment weapon)
    {
        int minDamage = weapon.bonusStatDictionary[StatType.MinDamage];
        int maxDamage = weapon.bonusStatDictionary[StatType.MaxDamage];
        return (minDamage, maxDamage);
    }

    private bool TryQueueMeleeAttack(Equipment weapon, GameObject defender, CharacterSheet defendingCharacter, int attackSpeed)
    {
        if (!GameFunctions.IsAdjacent(loc.coord, defendingCharacter.loc.coord))
        {
            return false;
        }

        (int minDamage, int maxDamage) = GetWeaponDamage(weapon);
        return combatSeq.AddMeleeAttack(this.gameObject, defender, minDamage, maxDamage, attackSpeed);
    }

    private bool TryQueueRangedAttack(RangedWeapon rangedWeapon, GameObject defender, int attackSpeed)
    {
        (int minDamage, int maxDamage) = GetWeaponDamage(rangedWeapon);
        int range = rangedWeapon.bonusStatDictionary[StatType.Range];
        
        return combatSeq.AddProjectileAttack(
            this.gameObject,
            defender,
            range,
            minDamage,
            maxDamage,
            attackSpeed,
            rangedWeapon.projectile
        );
    }

    private bool QueueAttacksForEquipment(Equipment mainHandWeapon, Equipment offHandWeapon, GameObject defender, CharacterSheet defendingCharacter)
    {
        bool attackOccurred = false;

        // Attempt main-hand attack
        if (mainHandWeapon != null)
        {
            if (mainHandWeapon is RangedWeapon rangedWeapon)
            {
                attackOccurred = TryQueueRangedAttack(rangedWeapon, defender, speed) || attackOccurred;
            }
            else
            {
                attackOccurred = TryQueueMeleeAttack(mainHandWeapon, defender, defendingCharacter, speed) || attackOccurred;
            }
        }

        // Attempt off-hand attack (if not a shield)
        if (offHandWeapon != null && offHandWeapon is not Shield)
        {
            int offHandSpeed = speed / 2; // 0.5x speed penalty for off-hand

            if (offHandWeapon is RangedWeapon rangedOffHand)
            {
                attackOccurred = TryQueueRangedAttack(rangedOffHand, defender, offHandSpeed) || attackOccurred;
            }
            else
            {
                attackOccurred = TryQueueMeleeAttack(offHandWeapon, defender, defendingCharacter, offHandSpeed) || attackOccurred;
            }
        }

        return attackOccurred;
    }

    public bool AttackCharacter(GameObject defender)
    {
        CharacterSheet defendingCharacter = defender.GetComponent<CharacterSheet>();
        
        Equipment mainHandWeapon = (Equipment)im.equipmentSlotsDictionary[ItemSlotType.MainHand].item;
        Equipment offHandWeapon = (Equipment)im.equipmentSlotsDictionary[ItemSlotType.OffHand].item;

        // Determine if using bare hands (no weapons or only shield equipped)
        bool isBareHanded = mainHandWeapon == null && (offHandWeapon == null || offHandWeapon is Shield);

        bool attackOccurred = false;

        // Attempt bare-hands melee attack
        if (isBareHanded)
        {
            if (GameFunctions.IsAdjacent(loc.coord, defendingCharacter.loc.coord))
            {
                attackOccurred = combatSeq.AddMeleeAttack(
                    this.gameObject,
                    defender,
                    minDamage,
                    maxDamage,
                    speed
                );
            }
        }
        // Attempt equipped weapon attacks
        else
        {
            attackOccurred = QueueAttacksForEquipment(mainHandWeapon, offHandWeapon, defender, defendingCharacter);
        }

        // If no attack succeeded, move towards defender
        if (!attackOccurred)
        {
            List<Vector2Int> pathToDestination = PathFinder.FindPath(loc.coord, defendingCharacter.loc.coord, tileMgr.levelCoords);
            Move(pathToDestination[1]);
        }

        return attackOccurred;
    }

    public void UpdateUI()
    {

        updateStats.RefreshUI();
    }

    public override void ProcessStatusEffects()
    {

        statusEffectMgr.ProcessStatusEffects();
        UpdateStatusNotifications();
    }

    public void UpdateStatusNotifications()
    {

        snm.UpdateStatusNotifications(statusEffectMgr.GetStatusEffects());
    }

    public override bool Move(Vector2Int newCoord, float waitTime = 0f)
    {

        if (base.Move(newCoord, waitTime))
        {

            RevealAroundPC();
            return true;
        }

        return false;
    }

    public override bool Teleport(Vector2Int newCoord)
    {
        
        if (base.Teleport(newCoord))
        {
            
            RevealAroundPC();
            return true;
        }

        return false;
    }

    public void RevealAroundPC()
    {

        tileMgr.RevealTiles(GameFunctions.GetCircleCoords(loc.coord, visibilityRadius));
    }

    public override void OnDeath()
    {

        turnSequencer.DeadLock();
    }
}