using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerTurnBattleState : BattleState
{
    public static event Action PlayerAttackTurnBegan;
    public static event Action PlayerAttackTurnEnded;

    [SerializeField] Text _playerTurnTextUI = null;

    [SerializeField] float _pauseDuration = 1f;

    public override void Enter()
    {
        Debug.Log("Player Attack: ...Entering");
        PlayerAttackTurnBegan?.Invoke();
        if (_playerTurnTextUI != null)
        {
            _playerTurnTextUI.gameObject.SetActive(true);
            //_playerTurnTextUI.text = "Player Attacks for " 
            //   + StateMachine.PlanState._damageAmount.ToString() + " damage!";
        }
        StartCoroutine(PlayerAttackingRoutine(_pauseDuration));
    }

    public override void Exit()
    {
        if (_playerTurnTextUI != null)
            _playerTurnTextUI.gameObject.SetActive(false);
        Debug.Log("Player Attack: Exiting...");
    }

    IEnumerator PlayerAttackingRoutine(float pauseDuration)
    {
        Debug.Log("The player prepares to attack...");
        yield return new WaitForSeconds(pauseDuration);

        Debug.Log("The player attacks!");
        PlayerAttackTurnEnded?.Invoke();
        //turn over. go to enemy's turn
        //StateMachine.ChangeState(StateMachine.EnemyAttackState);
        AttackOutcome();
    }

    void AttackOutcome()
    {
        if (StateMachine.attackPlan == "win")
        {
            StateMachine.ChangeState(StateMachine.Win);
        }
        else
        {
            //continue loop
            StateMachine.ChangeState(StateMachine.EnemyAttackState);
        }
    }
}
