using UnityEngine;

public class LockManager : MonoBehaviour
{

    TurnSequencer turnSeq;
    int turnLockCount = 0;
    CombatManager combatSeq;
    int combatLockCount = 0;

    void Awake()
    {

        GameObject managers = GameObject.Find("System Managers");

        turnSeq = managers.GetComponent<TurnSequencer>();
        combatSeq = managers.GetComponent<CombatManager>();
    }

    public void TakeTurnLock(int count = 1)
    {

        if(count <= 0)
        {
            
            return;
        }

        turnSeq.IncrementTurnLock(count);
        turnLockCount += count;
    }

    public void GiveTurnLock(int count = 1)
    {
        
        if(count <= 0 || turnLockCount < count)
        {
            
            return;
        }

        turnSeq.DecrementTurnLock(count);
        turnLockCount = Mathf.Max(turnLockCount - count, 0);
    }

    public void TakeCombatLock(int count = 1)
    {
        
        if(count <= 0)
        {
            
            return;
        }

        combatSeq.IncrementCombatLock(count);
        combatLockCount += count;
    }

    public void GiveCombatLock(int count = 1)
    {
        
        if(count <= 0 || combatLockCount < count)
        {
            
            return;
        }

        combatSeq.DecrementCombatLock(count);
        combatLockCount = Mathf.Max(combatLockCount - count, 0);
    }

    void OnDestroy()
    {
        
        GiveCombatLock(combatLockCount);
        GiveTurnLock(turnLockCount);
    }


}
