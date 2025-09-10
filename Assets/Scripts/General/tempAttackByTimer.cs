using UnityEngine;

public class tempAttackByTimer : MonoBehaviour
{

    private TimerTrigger _attackTimer;


    void Awake()
    {
        //_attackTimer = new TimerTrigger(
        _attackTimer = new TimerTrigger(
        duration: 0.5f,

        //onStart: () => Debug.Log("Начало атаки"),
        onTick: () => AttackHit(),
        onComplete: () => EndAttack(),
        looped: true
        ); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseManager.IsPaused) 
        {
            _attackTimer.Update(Time.deltaTime);
        }
   


        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartAttack();
        }
    }

    public void EndAttack()
    {
        _attackTimer.Reset();
        Debug.Log("Атака ");
    }

    public void AttackHit()
    {
        //_attackTimer.Reset();
        Debug.Log("Hit!!!");
    }

    public void StartAttack()
    {
        Debug.Log("Начало атаки");
        _attackTimer.Start();
    }
}
