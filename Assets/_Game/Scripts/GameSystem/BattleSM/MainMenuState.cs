using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuState : BattleState
{
    [SerializeField] GameObject _mainUI = null;
    [SerializeField] AudioClip _menuMusic = null;
    [SerializeField] AudioClip _battleMusic = null;
    bool _activated = false;
    AudioSource audSauce = null;

    public override void Enter()
    {
        //Debug.Log("Main menu State: ...Entering");
        if (_mainUI != null)
            _mainUI.SetActive(true);

        //play menu music
        if(_menuMusic != null)
        {
            audSauce = AudioHelper.PlayClip2D(_menuMusic, .6f);
        }

        StateMachine.BattleUI.SetActive(false);
        //hook into events
        _activated = false;
        //Debug.Log("enter activated == " + _activated);
        StateMachine.Input.PressedConfirm += OnPressedConfirm;
        StateMachine.Input.PressedCancel += OnPressedCancel;
    }

    public override void Tick()
    {
        StateDuration += Time.deltaTime;
        //bad method: makes delays
        if (_activated == false && StateDuration > 1)
        {
            _activated = true;
            //Debug.Log("update activated == " + _activated);
        }
    }

    public override void Exit()
    {
        if (_mainUI != null)
            _mainUI.SetActive(false);
        //unhook from events
        StateMachine.Input.PressedConfirm -= OnPressedConfirm;
        StateMachine.Input.PressedCancel -= OnPressedCancel;
        _activated = false;
        //Debug.Log("Main Menu State: Exiting...");
    }

    void OnPressedConfirm()
    {
        //Debug.Log("Attempt to Enter Player Attack State!");
        //change to player attack state
        if (_activated)
        {
            //Debug.Log("confirm activated == " + _activated);
            if (_battleMusic != null)
            {
                //play background music
                //TODO: Make the music loop!!
                audSauce.Stop();
                AudioHelper.PlayClip2D(_battleMusic, .3f);
            }

            StateMachine.ChangeState(StateMachine.SetupState); //TODO: Change scene and then go to state!
        }
    }

    void OnPressedCancel()
    {
        if (_activated)
            Application.Quit();
    }
}
