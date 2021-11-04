using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupBattleState : BattleState
{
    [SerializeField] int _numberOfEnemies = 3;
    //[SerializeField] float _playerStartHealth = 100;

    bool _activated = false;

    public override void Enter()
    {
        Debug.Log("Setup: ...Entering");
        Debug.Log("Starting a battle with " + _numberOfEnemies + " enemies.");
        //StateMachine.enemiesLeft = _numberOfEnemies;
        //StateMachine.playerHealth = _playerStartHealth;
        //Cant change state while in Enter or Exit
        //don't but change state here
        _activated = false;
    }

    public override void Tick()
    {
        StateDuration += Time.deltaTime;
        //bad method: makes delays
        if (_activated == false)
        {
            _activated = true;
            StateMachine.ChangeState(StateMachine.PlanState);
        }
        Debug.Log("Setup: ...Updating...");
    }

    public override void Exit()
    {
        _activated = false;
        Debug.Log("Setup: Exiting...");
    }
}
