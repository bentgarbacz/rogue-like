using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementManager : MonoBehaviour
{

    private Dictionary<CharacterSheet, MovementCommand> movementCommands = new();
    public float baseWaitTime = 0.025f;
    public float incrementWaitTime = 0.025f;
    public const float maxWaitTime = 0.1f;
    [SerializeField] private TurnSequencer ts;

    public void AddMovement(CharacterSheet targetCharacter, Vector2Int targetCoord, bool isTeleport=false)
    {

        MovementCommand newMovementCommand = new(targetCoord, isTeleport);
        bool addSuccessful= movementCommands.TryAdd(targetCharacter, newMovementCommand);

        if(addSuccessful)
        {

           return; 
        }

        if(movementCommands[targetCharacter].isLocked)
        {
            
            return;

        }
        else
        {
            
            movementCommands[targetCharacter] = newMovementCommand;
        }
    }

    public void ProcessMovement()
    {

        float waitTime = baseWaitTime;

        foreach(CharacterSheet currChar in movementCommands.Keys)
        {
            
            MovementCommand currMove =  movementCommands[currChar];

            if(currMove.isTeleport)
            {
                
                currChar.Teleport(currMove.targetCoord);

            }
            else
            {
                
                currChar.Move(currMove.targetCoord, waitTime);
                waitTime = Math.Min(waitTime + incrementWaitTime, maxWaitTime);
            }
        }

        movementCommands = new();
    }

    public bool RemoveCharacter(CharacterSheet characterSheet)
    {

        return movementCommands.Remove(characterSheet);
    }

    public void Refresh()
    {

        movementCommands = new();
    }
}

public class MovementCommand
{
    
    public Vector2Int targetCoord;
    public bool isTeleport;
    public bool isLocked;

    public MovementCommand(Vector2Int targetCoord, bool isTeleport=false, bool isLocked = false)
    {
        
        this.targetCoord = targetCoord;
        this.isTeleport = isTeleport;
        this.isLocked = isLocked;
    }
}
