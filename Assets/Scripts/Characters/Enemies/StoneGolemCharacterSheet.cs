using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TextNotificationManager))]
[RequireComponent(typeof(DropLoot))]
public class StoneGolemCharacterSheet : EnemyCharacterSheet
{
    private int skipTurnCount = 0;
    private ObjectHighlighter highlighter;
    private Color originalColor;

    public override void Awake()
    {
        base.Awake();
        
        // Stone Golem: Large stone statue that hits hard but moves slowly
        title = "Stone Golem";
        maxHealth = 50; // High health
        minDamage = 8;  // High damage
        maxDamage = 12;
        speed = 3;      // Slower speed
        armor = 5;      // High armor
        evasion = 0;    // No evasion
        level = 5;      // Higher level

        characterHealth.InitHealth(maxHealth);

        dropTable = DropTableType.None;

        attackClip = Resources.Load<AudioClip>("Sounds/BigSmack");

        highlighter = GetComponent<ObjectHighlighter>();
        originalColor = highlighter.GetComponent<Renderer>().material.color;
    }

    public override void AggroBehavior()
    {
        if (skipTurnCount < 2)
        {
            skipTurnCount += 1;
            
            // Visual indication: darken color and show "..." notification
            StartCoroutine(ShowSkipTurnEffect());
            
            return;
        }

        if (!GetAggroStatus())
        {
            entityMgr.aggroEnemies.Remove(this.gameObject);
            return;
        }
        
        List<Dictionary<Vector2Int, float>> mapsOfInterest = new(){djm.npcMap, djm.playerMap};
        Vector2Int targetCoord = GetNeighborTileOfMostInterest(loc.coord, mapsOfInterest);
        float tileInterestVal = Mathf.Min(djm.GetPlayerMapValue(targetCoord), djm.GetNpcMapValue(targetCoord));
        
        if(tileInterestVal == 0)
        {
            AttackEntity(targetCoord);
        }
        else
        {
            
            Vector2Int direction = targetCoord - loc.coord;
            Vector2Int twoStepCoord = loc.coord + direction * 2;
            Vector2Int attackCoord;
            
            if (tileMgr.levelCoords.Contains(twoStepCoord) && !tileMgr.occupiedlist.Contains(twoStepCoord))
            {
                //movementManager.AddMovement(this, twoStepCoord);
                Move(twoStepCoord);
                attackCoord = twoStepCoord;
            }
            else
            {
                //movementManager.AddMovement(this, targetCoord);
                Move(targetCoord);
                attackCoord = targetCoord;
            }

            Vector2Int targetCoordAfter = GetNeighborTileOfMostInterest(attackCoord, mapsOfInterest);
            float tileInterestValAfter = Mathf.Min(djm.GetPlayerMapValue(targetCoordAfter), djm.GetNpcMapValue(targetCoordAfter));

            if(tileInterestValAfter == 0)
            {
                
                AttackEntity(targetCoordAfter);
            }
        }

        skipTurnCount = 0;
    }

    private IEnumerator ShowSkipTurnEffect()
    {
        // Darken the color
        Renderer renderer = highlighter.GetComponent<Renderer>();
        renderer.material.color = originalColor * 0.2f; // Darker version
        
        // Show "..." notification
        notificationManager.CreateNotificationOrder(2f, "...", Color.gray);
        
        // Wait a bit then reset color
        yield return new WaitForSeconds(0.5f);
        renderer.material.color = originalColor;
    }
}