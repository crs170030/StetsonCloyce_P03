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
    [SerializeField] CharacterBase _char1 = null;
    [SerializeField] CharacterBase _char2 = null;
    [SerializeField] CharacterBase _char3 = null;
    int activeCharNum = 1;
    CharacterBase activeChar = null;

    bool _activated = false;

    public override void Enter()
    {
        Debug.Log("Player Attack: ...Entering");
        PlayerAttackTurnBegan?.Invoke();

        activeCharNum = 1;
        CharacterBase activeChar = _char1;
        if (_playerTurnTextUI != null)
        {
            _playerTurnTextUI.gameObject.SetActive(true);
        }
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
            StartCoroutine(PlayerAttackingRoutine(_pauseDuration));
        }
        Debug.Log("Player Attack: ...Updating...");
    }

    public override void Exit()
    {
        if (_playerTurnTextUI != null)
            _playerTurnTextUI.gameObject.SetActive(false);
        Debug.Log("Player Attack: Exiting...");
    }

    IEnumerator PlayerAttackingRoutine(float pauseDuration)
    {
        while (activeCharNum < 4) {
            switch (activeCharNum)
            {
                case 1: activeChar = _char1;
                    break;
                case 2:
                    activeChar = _char2;
                    break;
                case 3:
                    activeChar = _char3;
                    break;
            }

            Debug.Log("The player " + activeChar.name + " prepares to attack...");
            yield return new WaitForSeconds(pauseDuration);
            
            Debug.Log("The player attacks!");
            PlayerAttackTurnEnded?.Invoke();
            //turn over. go to enemy's turn
            //StateMachine.ChangeState(StateMachine.EnemyAttackState);
            AttackOutcome();
        }
    }

    void AttackOutcome()
    {
        //check if target's health will kill it.
        //if so, then reduce the amount of battle sm enemies by 1

        //call the player attack method
        activeChar.BaseAttack();

        if (StateMachine.enemiesLeft <= 0)//StateMachine.attackPlan == "win"
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
