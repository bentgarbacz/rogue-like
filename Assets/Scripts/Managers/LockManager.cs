using UnityEngine;

public class LockManager : MonoBehaviour
{

    TurnSequencer turnSeq;
    int turnLockCount = 0;
    CombatSequencer combatSeq;
    int combatLockCount = 0;

    void Awake()
    {

        GameObject managers = GameObject.Find("System Managers");

        turnSeq = managers.GetComponent<TurnSequencer>();
        combatSeq = managers.GetComponent<CombatSequencer>();
    }

    public void AcquireTurnLock(int count = 1)
    {

        if(count <= 0)
        {
            
            return;
        }

        turnSeq.IncrementTurnLock(count);
        turnLockCount += count;
    }

    public void ReleaseTurnLock(int count = 1)
    {
        
        if(count <= 0 || turnLockCount < count)
        {
            
            return;
        }

        turnSeq.DecrementTurnLock(count);
        turnLockCount = Mathf.Max(turnLockCount - count, 0);
    }

    public void AcquireCombatLock(int count = 1)
    {
        
        if(count <= 0)
        {
            
            return;
        }

        combatSeq.IncrementCombatLock(count);
        combatLockCount += count;
    }

    public void ReleaseCombatLock(int count = 1)
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
        
        ReleaseCombatLock(combatLockCount);
        ReleaseTurnLock(turnLockCount);
    }
}
