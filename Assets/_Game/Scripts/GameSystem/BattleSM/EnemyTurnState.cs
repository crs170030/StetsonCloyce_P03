using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyTurnState : BattleState
{
    public static event Action EnemyTurnBegan;
    public static event Action EnemyTurnEnded;

    [SerializeField] float _pauseDuration = 2f;
    //[SerializeField] float _enemyHealth = 50f;

    EnemyBase[] enemies = null;
    EnemyBase activeEnemy = null;
    int activeEnemyNum = 0;
    CharacterBase[] characters = null;
    bool _activated = false;
    bool foundTarget = false;
    CharacterBase target = null;
    float hmove = 50f;

    public override void Enter()
    {
        //Debug.Log("Enemy Turn: ...Enter");
        EnemyTurnBegan?.Invoke();

        //get all alive enemies
        enemies = FindObjectsOfType<EnemyBase>();
        activeEnemyNum = 0;
        //get all alive players
        characters = FindObjectsOfType<CharacterBase>();

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
            StartCoroutine(EnemyThinkingRoutine(_pauseDuration));
            //Debug.Log("Enemy Turn: ...Updating...");
        }
        //Debug.Log("Enemy Turn: ...Updating...");
    }

    public override void Exit()
    {
        _activated = false;
        //Debug.Log("Enemy Turn: Exit...");
    }

    IEnumerator EnemyThinkingRoutine(float pauseDuration)
    {
        //Debug.Log("Enemy thinking...");
        
        //turn over. go back to player
        //StateMachine.ChangeState(StateMachine.PlanState);
        for (activeEnemyNum = 0; activeEnemyNum < enemies.Length; activeEnemyNum++) {
            activeEnemy = enemies[activeEnemyNum];
            //move active enemy forward a bit
            activeEnemy.transform.position = activeEnemy.transform.position + new Vector3(-hmove, 0, 0);

            yield return new WaitForSeconds(pauseDuration);
            //Debug.Log("Enemy "+ activeEnemyNum +" performs action");
            foundTarget = false;
            Attack();
            yield return new WaitForSeconds(pauseDuration);
            Outcome();

            //move active enemy back
            activeEnemy.transform.position = activeEnemy.transform.position + new Vector3(hmove, 0, 0);
        }
        //yield return new WaitForSeconds(pauseDuration/2);
        EnemyTurnEnded?.Invoke();
        if(StateMachine.playersAlive > 0)
            StateMachine.ChangeState(StateMachine.PlanState);
    }

    void Attack()
    {
        //randomly select number between 1 and 3
        //check if selected player number is alive
        //if they are, then attack
        //else, repeat
        while(!foundTarget && StateMachine.playersAlive > 0)
        {
            var targetNum = (int)UnityEngine.Random.Range(0, 3);
            //activeEnemy = enemies[activeEnemyNum];
            target = characters[targetNum];
            //Debug.Log("Targeting player " + targetNum + ". Who is " + target);
            if (target != null && target.alive)
            {
                foundTarget = true;
                var hb = target.GetComponent<HealthBase>();
                if(hb != null)
                {
                    activeEnemy.AddTarget(hb);
                    //check if they are defending
                    if(target.defending)
                        activeEnemy.BaseAttack(activeEnemy.baseDamage / target.defense);
                    else
                        activeEnemy.BaseAttack(activeEnemy.baseDamage);
                }
                else
                {
                    Debug.Log("Warning: Character " + target + " hb is null!");
                }
            }
        }
    }

    void Outcome()
    {
        if (StateMachine.playersAlive <= 0)  //StateMachine.attackPlan == "lose"
        {
            activeEnemyNum = enemies.Length; //makes sure thinking loop doesn't run again!
            StateMachine.ChangeState(StateMachine.Lose);
        }
        else
        {
            if (activeEnemyNum >= enemies.Length) { 
                //continue loop
                StateMachine.ChangeState(StateMachine.PlanState);
            }
        }
    }
}
