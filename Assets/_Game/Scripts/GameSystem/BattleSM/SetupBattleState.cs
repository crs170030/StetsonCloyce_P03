using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupBattleState : BattleState
{
    [SerializeField] Transform UIGroup = null;
    [SerializeField] int _numberOfEnemies = 3;
    [SerializeField] EnemyBase enemyPrefab = null;
    /*
    [SerializeField] CharacterBase char1 = null;
    [SerializeField] CharacterBase char2 = null;
    [SerializeField] CharacterBase char3 = null;
    */
    //[SerializeField] float _playerStartHealth = 100;

    float playerX = -400;
    float enemyX = 800;
    bool _activated = false;

    public override void Enter()
    {
        Debug.Log("Setup: ...Entering");
        Debug.Log("Starting a battle with " + _numberOfEnemies + " enemies.");
        StateMachine.enemiesLeft = _numberOfEnemies;
        StateMachine.BattleUI.SetActive(true);

        SpawnCharacters();
        //restore health of all
        var entities = FindObjectsOfType<HealthBase>();
        foreach (HealthBase hb in entities)
        {
            hb.restoreHealth();
        }
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
            //Debug.Log("Setup: ...Updating...");
        }
        Debug.Log("Setup: ...Updating...");
    }

    public override void Exit()
    {
        _activated = false;
        Debug.Log("Setup: Exiting...");
    }

    void SpawnCharacters()
    {
        //spawn enemies
        var spawnY = 600;
        var spawnPosition = new Vector3(playerX, spawnY, 0);
        var offsetX = 0;

        for (int i = 0; i < _numberOfEnemies; i++)
        {
            if (i % 2 == 0)
                offsetX = 25; //enemies are pushed to the right
            else
                offsetX = 0;

            spawnPosition = new Vector3(enemyX - offsetX, spawnY, 0);
            //Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, UIGroup);
            var inst = Instantiate(enemyPrefab, UIGroup);
            inst.transform.position = spawnPosition;
            spawnY -= 160;
        }
    }
}
