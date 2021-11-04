using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinState : BattleState
{
    [SerializeField] Text _winTextUI = null;

    public override void Enter()
    {
        Debug.Log("Win State: ...Entering");
        if (_winTextUI != null)
            _winTextUI.gameObject.SetActive(true);

        //hook into events
        StateMachine.Input.PressedConfirm += OnPressedConfirm;
    }

    public override void Exit()
    {
        if (_winTextUI != null)
            _winTextUI.gameObject.SetActive(false);
        //unhook from events
        StateMachine.Input.PressedConfirm -= OnPressedConfirm;

        Debug.Log("Win State: Exiting to Main Menu...");
    }

    void OnPressedConfirm()
    {
        //Debug.Log("Attempt to Enter Player Attack State!");
        //change to player attack state
        StateMachine.ChangeState(StateMachine.MainMenu);
    }
}
