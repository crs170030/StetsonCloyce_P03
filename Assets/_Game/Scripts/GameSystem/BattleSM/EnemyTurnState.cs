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

    public override void Enter()
    {
        Debug.Log("Enemy Turn: ...Enter");
        EnemyTurnBegan?.Invoke();

        StartCoroutine(EnemyThinkingRoutine(_pauseDuration));
    }

    public override void Exit()
    {
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
        AttackOutcome();
    }

    void AttackOutcome()
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
