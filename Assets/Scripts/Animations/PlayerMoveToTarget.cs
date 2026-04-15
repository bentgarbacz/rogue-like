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
    public override void SetTarget(Vector3 target, float waitTime = 0f)
    {

        this.waitTime = waitTime;
        initialY = 0.1f;
        moving = true;
        this.target = target;
        distance = Vector3.Distance(new Vector3(target.x, 0, target.z), new Vector3(transform.position.x, 0, transform.position.z));

        lockMgr.AcquireTurnLock();
        lockMgr.AcquireCombatLock();
    }

    protected override void OnArrive()
    {

        if (jumpsWhileMoving)
        {

            if (makesNoise)
            {
                float startPitch = audioSource.pitch;
                audioSource.pitch = Random.Range(-3f, 3f);
                audioSource.PlayOneShot(stepAudioClip);
                audioSource.pitch = startPitch;
            }

            transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
        }

        moving = false;
        lockMgr.ReleaseTurnLock();
        lockMgr.ReleaseCombatLock();
        ts.SignalAction();
        pc.RevealAroundPC();
    }
}