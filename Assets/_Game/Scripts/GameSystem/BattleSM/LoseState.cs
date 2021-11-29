using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoseState : BattleState
{
    [SerializeField] Text _loseTextUI = null;
    [SerializeField] AudioClip _loseSound = null;
    //AudioSource audSauce = null;

    public override void Enter()
    {
        //Debug.Log("Lose State: ...Entering");
        if (_loseTextUI != null)
            _loseTextUI.gameObject.SetActive(true);

        //try to stop audio and play tune
        //audSauce = AudioHelper.PlayClip2D(_loseSound, 1f);
        //audSauce.Stop();
        AudioHelper.PlayClip2D(_loseSound, 1f);

        //hook into events
        StateMachine.Input.PressedConfirm += OnPressedConfirm;
    }

    public override void Exit()
    {
        if (_loseTextUI != null)
            _loseTextUI.gameObject.SetActive(false);
        //unhook from events
        StateMachine.Input.PressedConfirm -= OnPressedConfirm;

        //Debug.Log("Lose State: Exiting to Main Menu...");
    }

    void OnPressedConfirm()
    {
        //Debug.Log("Attempt to Enter Player Attack State!");
        //change to player attack state
        //StateMachine.ChangeState(StateMachine.MainMenu);

        //reload scene!
        //Application.LoadLevel(Application.loadedLevel);
        SceneManager.LoadScene("GameScene");
    }
}
