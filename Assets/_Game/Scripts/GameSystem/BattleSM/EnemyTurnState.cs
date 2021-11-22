using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyTurnState : BattleState
{
    public static event Action EnemyTurnBegan;
    public static event Action EnemyTurnEnded;

    [SerializeField] float _pauseDuration = 2f;
    //[SerializeField] float _enemyHealth = 50f;

    bool _activated = false;

    public override void Enter()
    {
        Debug.Log("Enemy Turn: ...Enter");
        EnemyTurnBegan?.Invoke();

        _activated = false;
    }

    public override void Tick()
    {
        StateDuration += Time.deltaTime;
        //bad method: makes delays
        if (_activated == false)
        {
            _activated = true;
            //StateMachine.ChangeState(StateMachine.PlanState);
            StartCoroutine(EnemyThinkingRoutine(_pauseDuration));
            Debug.Log("Enemy Turn: ...Updating...");
        }
        //Debug.Log("Enemy Turn: ...Updating...");
    }

    public override void Exit()
    {
        _activated = false;
        Debug.Log("Enemy Turn: Exit...");
    }

    IEnumerator EnemyThinkingRoutine(float pauseDuration)
    {
        Debug.Log("Enemy thinking...");
        yield return new WaitForSeconds(pauseDuration);

        Debug.Log("Enemy performs action");
        EnemyTurnEnded?.Invoke();
        //turn over. go back to player
        //StateMachine.ChangeState(StateMachine.PlanState);
        Attack();
        yield return new WaitForSeconds(pauseDuration);
        Outcome();
    }

    void Attack()
    {
        
    }

    void Outcome()
    {
        if (StateMachine.playersAlive <= 0)  //StateMachine.attackPlan == "lose"
        {
            StateMachine.ChangeState(StateMachine.Lose);
        }
        else
        {
            //continue loop
            StateMachine.ChangeState(StateMachine.PlanState);
        }
    }
}
