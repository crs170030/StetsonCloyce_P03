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

    [SerializeField] float _pauseDuration = 2f;

    [SerializeField] AudioClip _attackSound = null;
    [SerializeField] AudioClip _magicSound = null;
    [SerializeField] AudioClip _defendSound = null;

    [SerializeField] CharacterBase _char1 = null;
    [SerializeField] CharacterBase _char2 = null;
    [SerializeField] CharacterBase _char3 = null;
    int activeCharNum = 1;
    CharacterBase activeChar = null;
    EnemyBase[] enemies = null;
    float hmove = 50f;

    bool _activated = false;

    public override void Enter()
    {
        //Debug.Log("Player Attack: ...Entering");
        PlayerAttackTurnBegan?.Invoke();

        activeCharNum = 1;
        activeChar = _char1;
        enemies = null;
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
            //Debug.Log("Player Attack: ...Updating...");
        }
        //Debug.Log("Player Attack: ...Updating...");
    }

    public override void Exit()
    {
        _activated = false;
        if (_playerTurnTextUI != null)
            _playerTurnTextUI.gameObject.SetActive(false);
        //Debug.Log("Player Attack: Exiting...");
    }

    IEnumerator PlayerAttackingRoutine(float pauseDuration)
    {
        while (activeCharNum <= 3 && StateMachine.enemiesLeft > 0) {
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
            if (activeChar.alive)
            {
                //move active player forward a bit
                activeChar.transform.position = activeChar.transform.position + new Vector3(hmove, 0, 0);

                //Debug.Log(activeChar.name + " defending == " + activeChar.defending);
                if (!activeChar.defending)
                {
                    //Debug.Log("The player " + activeChar.name + " prepares to attack...");
                    yield return new WaitForSeconds(pauseDuration);

                    //Debug.Log(activeChar.name + " attacks!");

                    Attack();
                    yield return new WaitForSeconds(pauseDuration);
                }
                else
                {
                    //Debug.Log("The player " + activeChar.name + " holds a defensive stance...");
                    AudioHelper.PlayClip2D(_defendSound, 1f);
                    yield return new WaitForSeconds(pauseDuration);
                }

                //move player back to start position
                activeChar.transform.position = activeChar.transform.position + new Vector3(-hmove, 0, 0);
            }
            activeCharNum++;
            Outcome();
        }
        PlayerAttackTurnEnded?.Invoke();
    }

    void Attack()
    {
        //check if target's health will kill it.
        //if so, then reduce the amount of battle sm enemies by 1

        // check if the player's target is still alive
        // if not, then assign to first enemy on list
        if (activeChar.TargetGroup[0] == null) {
            EnemyBase newEnemy = FindObjectOfType<EnemyBase>();
            HealthBase newEnemyHB = null;
            if(newEnemy != null)
                newEnemyHB = newEnemy.GetComponent<HealthBase>();
            if(newEnemyHB != null)
            {
                //Debug.Log("Player " + activeChar + " Target Group == " + activeChar.TargetGroup[0]);
                activeChar.AddTarget(newEnemyHB);
            }
        }
        if (activeChar._attackPlan == "magic")
        {
            //call the player magic method
            AudioHelper.PlayClip2D(_magicSound, 1f); 
            //what if character spells have special sounds?
            //in the future, move sounds to player characters
            activeChar.MagicAttack();
        }
        else
        {
            //call the player attack method
            AudioHelper.PlayClip2D(_attackSound, 1f);
            activeChar.BaseAttack();
        }
    }

    void Outcome()
    {
        //check for num enemies in game, set enemies left to that number
        enemies = FindObjectsOfType<EnemyBase>();
        StateMachine.enemiesLeft = enemies.Length;
        //Debug.Log("Enemies array length: " + enemies.Length + ". EnemiesLeft = " + StateMachine.enemiesLeft);

        if (StateMachine.enemiesLeft <= 0)//StateMachine.attackPlan == "win"
        {
            StateMachine.ChangeState(StateMachine.Win);
        }
        else
        {
            //continue loop
            if (activeCharNum >= 4)
                StateMachine.ChangeState(StateMachine.EnemyAttackState);
        }

        PlayerAttackTurnEnded?.Invoke();
    }
}
