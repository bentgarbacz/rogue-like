using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    public bool fighting = false;
    public float attackTime = 0.2f;
    private DungeonManager dum;
    private List<(GameObject, GameObject)> combatBuffer;    


    void Start()
    {

        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
        combatBuffer = new List<(GameObject, GameObject)>();
    }

    public void CommenceCombat()
    {

        if(combatBuffer.Count > 0 && fighting == false)
        {
            
            fighting = true;

            SortBuffer();

            StartCoroutine(CombatTurns());
        }
    }

    private IEnumerator CombatTurns()
    {

        while(combatBuffer.Count > 0)
        {

            Character attacker = combatBuffer[0].Item1.GetComponent<Character>();
            Character defender = combatBuffer[0].Item2.GetComponent<Character>();
            
            attacker.Attack(defender);

            //kills defender of attack if it's health falls below 1
            if(defender.health <= 0)
            {
                
                for(int i = 1; i < combatBuffer.Count; i++ )
                {

                    if(combatBuffer[0].Item2 == combatBuffer[i].Item1)
                    {

                        combatBuffer.RemoveAt(i);
                        i--;
                    }
                }

                dum.Smite(combatBuffer[0].Item2, defender.pos);                                                                    
            }
            
            combatBuffer.RemoveAt(0);
            yield return new WaitForSeconds(attackTime);
            
        }
        
        fighting = false;
    }

    public void AddToCombatBuffer(GameObject attacker, GameObject defender)
    {

        combatBuffer.Add((attacker, defender));        
    }

    private void SortBuffer()
    {

        int bufferIndex = combatBuffer.Count;

        while(bufferIndex > 1)
        {

            for(int i = 0; i < bufferIndex - 1; i++)
            {

                if(combatBuffer[i].Item1.GetComponent<Character>().speed < combatBuffer[i+1].Item1.GetComponent<Character>().speed)
                {

                    (combatBuffer[i], combatBuffer[i+1]) = (combatBuffer[i+1], combatBuffer[i]);
                }
            }

            bufferIndex--;
        }
    }
}
