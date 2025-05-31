using UnityEngine;

public class PlayerMoveToTarget : MoveToTarget
{

    private PlayerCharacterSheet pc;

    void Start()
    {

        distance = 0;
        pc = GetComponent<PlayerCharacterSheet>();
    }

    protected override void OnArrive()
    {

        base.OnArrive();
        pc.RevealAroundPC();
    }
}