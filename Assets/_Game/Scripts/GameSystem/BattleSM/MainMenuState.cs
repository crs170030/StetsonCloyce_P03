using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuState : BattleState
{
    [SerializeField] Text _mainTextUI = null;
    bool _activated = false;

    public override void Enter()
    {
        Debug.Log("Main menu State: ...Entering");
        if (_mainTextUI != null)
            _mainTextUI.gameObject.SetActive(true);

        //hook into events
        _activated = false;
        Debug.Log("enter activated == " + _activated);
        StateMachine.Input.PressedConfirm += OnPressedConfirm;
    }

    public override void Tick()
    {
        StateDuration += Time.deltaTime;
        //bad method: makes delays
        if (_activated == false && StateDuration > 1)
        {
            _activated = true;
            Debug.Log("update activated == " + _activated);
        }
    }

    public override void Exit()
    {
        if (_mainTextUI != null)
            _mainTextUI.gameObject.SetActive(false);
        //unhook from events
        StateMachine.Input.PressedConfirm -= OnPressedConfirm;
        _activated = false;
        Debug.Log("Main Menu State: Exiting...");
    }

    void OnPressedConfirm()
    {
        //Debug.Log("Attempt to Enter Player Attack State!");
        //change to player attack state
        if (_activated)
        {
            Debug.Log("confirm activated == " + _activated);
            StateMachine.ChangeState(StateMachine.SetupState); //TODO: Change scene and then go to state!
        }
    }
}
