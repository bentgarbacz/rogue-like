using UnityEngine;

public class PlayerMoveToTarget : MoveToTarget
{

    private PlayerCharacterSheet pc;
    private TurnSequencer ts;

    void Start()
    {

        distance = 0;

        GameObject managers = GameObject.Find("System Managers");
        ts = managers.GetComponent<TurnSequencer>();
        pc = GetComponent<PlayerCharacterSheet>();
    }

    protected override void OnArrive()
    {

        base.OnArrive();
        ts.AggroNearbyEnemies();
        pc.RevealAroundPC();
    }
}