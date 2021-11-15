using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnPlanState : BattleState
{
    [SerializeField] GameObject _playerUI = null;
    [SerializeField] Text _playerTurnTextUI = null; //TODO: Remove Text UI?
    [SerializeField] Text _playerChoiceTextUI = null;
    [SerializeField] Image _attackButton = null;
    [SerializeField] Image _magicButton = null;
    [SerializeField] Image _defendButton = null;
    [SerializeField] Image _selectionBorder = null;

    //[SerializeField] float _damageAmount = 25f;

    int _playerTurnCount = 0;

    public override void Enter()
    {
        Debug.Log("Player Choose Action: ...Entering");
        _playerUI.SetActive(true);

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
        if(_playerUI != null)
            _playerUI.SetActive(false);
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
        _selectionBorder.transform.position = _magicButton.transform.position;
    }

    void OnPressedRight()
    {
        //alter stage after attack
        StateMachine.attackPlan = "lose";
        _playerChoiceTextUI.text = "Player's Action: " + StateMachine.attackPlan;
        _selectionBorder.transform.position = _defendButton.transform.position;
    }

    void OnPressedUp()
    {
        //alter stage after attack
        StateMachine.attackPlan = "win";
        _playerChoiceTextUI.text = "Player's Action: " + StateMachine.attackPlan;
        _selectionBorder.transform.position = _attackButton.transform.position;
    }
}
