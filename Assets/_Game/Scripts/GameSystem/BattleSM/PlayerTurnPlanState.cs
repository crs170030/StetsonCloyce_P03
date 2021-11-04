using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnPlanState : BattleState
{
    [SerializeField] Text _playerTurnTextUI = null;
    [SerializeField] Text _playerChoiceTextUI = null;
    //[SerializeField] float _damageAmount = 25f;

    int _playerTurnCount = 0;

    public override void Enter()
    {
        Debug.Log("Player Choose Action: ...Entering");
        _playerTurnTextUI.gameObject.SetActive(true);
        _playerChoiceTextUI.gameObject.SetActive(true);

        _playerTurnCount++;
        _playerTurnTextUI.text = "Player Turn: " + _playerTurnCount.ToString();
        //hook into events
        StateMachine.Input.PressedConfirm += OnPressedConfirm;
        StateMachine.Input.PressedLeft += OnPressedLeft;
        StateMachine.Input.PressedRight += OnPressedRight;
        StateMachine.Input.PressedUp += OnPressedUp;
    }

    public override void Exit()
    {
        if (_playerTurnTextUI != null)
        {
            _playerTurnTextUI.gameObject.SetActive(false);
        }
        if(_playerChoiceTextUI != null)
            _playerChoiceTextUI.gameObject.SetActive(false);
        //unhook from events
        StateMachine.Input.PressedConfirm -= OnPressedConfirm;

        Debug.Log("Player Choose Action: Exiting...");
    }

    void OnPressedConfirm()
    {
        //Debug.Log("Attempt to Enter Player Attack State!");
        //change to player attack state
        StateMachine.ChangeState(StateMachine.PlayerAttackState);
    }

    void OnPressedLeft()
    {
        //alter stage after attack
        StateMachine.attackPlan = "nothing";
        _playerChoiceTextUI.text = "Player's Action: "+ StateMachine.attackPlan;
    }

    void OnPressedRight()
    {
        //alter stage after attack
        StateMachine.attackPlan = "lose";
        _playerChoiceTextUI.text = "Player's Action: " + StateMachine.attackPlan;
    }

    void OnPressedUp()
    {
        //alter stage after attack
        StateMachine.attackPlan = "win";
        _playerChoiceTextUI.text = "Player's Action: " + StateMachine.attackPlan;
    }
}
