using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnPlanState : BattleState
{
    [SerializeField] GameObject _playerUI = null;
    [SerializeField] GameObject _actionUI = null;
    [SerializeField] Text _playerTurnTextUI = null;
    //[SerializeField] Text _playerChoiceTextUI = null; //TODO: Remove Text UI?
    [SerializeField] Text _magicCostTextUI = null;
    [SerializeField] Image _attackButton = null;
    [SerializeField] Image _magicButton = null;
    [SerializeField] Image _defendButton = null;
    [SerializeField] Image _selectionBorder = null;
    [SerializeField] Image _selectionPlayer = null;
    [SerializeField] Image _selectionEnemy = null;

    [SerializeField] AudioClip _selectSound = null;
    [SerializeField] AudioClip _confirmSound = null;
    [SerializeField] AudioClip _noCanDoSound = null;

    [SerializeField] CharacterBase _char1 = null;
    [SerializeField] CharacterBase _char2 = null;
    [SerializeField] CharacterBase _char3 = null;
    int activeCharNum = 1;
    CharacterBase activeChar = null;
    int selectMode = 0; //0-select action, 1-select target
    EnemyBase[] enemies = null;
    EnemyBase activeTarget = null;
    int activeTargetNum = 0;
    public float defenseManaAmount = 20f;
    float buttonDistance = 20f;

    //[SerializeField] float _damageAmount = 25f;

    int _playerTurnCount = 0;

    public override void Enter()
    {
        //Debug.Log("Player Choose Action: ...Entering");
        _playerUI.SetActive(true);
        _actionUI.SetActive(true);
        SetNextActiveChar(1, true);
        selectMode = 0;
        _selectionPlayer.transform.position = activeChar.transform.position;
        //_selectionBorder.transform.position = _attackButton.transform.position;
        SetActiveButtonValues("attack");
        enemies = FindObjectsOfType<EnemyBase>();
        _selectionEnemy.transform.position = enemies[0].transform.position;
        _selectionEnemy.gameObject.SetActive(false);

        activeTargetNum = 0;
        //Debug.Log("first enemy = " + enemies[activeTargetNum]);

        _playerTurnCount++;
        _playerTurnTextUI.text = "Player Turn: " + _playerTurnCount.ToString();
        //hook into events
        StateMachine.Input.PressedConfirm += OnPressedConfirm;
        StateMachine.Input.PressedCancel += OnPressedCancel;
        StateMachine.Input.PressedLeft += OnPressedLeft;
        StateMachine.Input.PressedRight += OnPressedRight;
        StateMachine.Input.PressedUp += OnPressedUp;
        StateMachine.Input.PressedDown += OnPressedDown;
    }

    public override void Exit()
    {
        if(_playerUI != null)
            _playerUI.SetActive(false);

        activeCharNum = 1;
        //unhook from events
        StateMachine.Input.PressedConfirm -= OnPressedConfirm;
        StateMachine.Input.PressedCancel -= OnPressedCancel;
        StateMachine.Input.PressedLeft -= OnPressedLeft;
        StateMachine.Input.PressedRight -= OnPressedRight;
        StateMachine.Input.PressedUp -= OnPressedUp;
        StateMachine.Input.PressedDown -= OnPressedDown;

        //Debug.Log("Player Choose Action: Exiting...");
    }

    void OnPressedConfirm()
    {
        if (selectMode == 0)//0-select action, 1-select target
        {
            //if selected magic, check if team mp is high enough
            //if not, play sound
            if (activeChar._attackPlan == "magic")
            {
                if (StateMachine.mana < activeChar.spellCost)
                {
                    AudioHelper.PlayClip2D(_noCanDoSound, .5f);
                    return; //don't continue if magic is too low!
                }
                else
                {
                    StateMachine.mana -= activeChar.spellCost; //deduct magic points from total!
                }
            }
            

            selectMode = 1;
            _actionUI.SetActive(false);
            _selectionEnemy.gameObject.SetActive(true);
            if (enemies.Length > 0)
            {
                activeTarget = enemies[enemies.Length-1];
                SetCharacterTarget(activeTarget);
            }
            if (activeChar.defending)
            {
                StateMachine.mana += defenseManaAmount;
                OnPressedConfirm();
            }
        }
        else
        {
            //if 1, got to next player
            switch (activeCharNum)
            {
                case 1://go to character 2
                    SetNextActiveChar(2, true);
                    break;

                case 2://go to character 3
                    SetNextActiveChar(3, true);
                    break;

                case 3://go to attack phase
                    //Debug.Log("Attempt to Enter Player Attack State!");
                    StateMachine.ChangeState(StateMachine.PlayerAttackState);
                    break;
            }
            _selectionPlayer.transform.position = activeChar.transform.position;
            selectMode = 0;
            _actionUI.SetActive(true);
            _selectionEnemy.gameObject.SetActive(false);
        }
        AudioHelper.PlayClip2D(_confirmSound, .2f);
    }

    void OnPressedCancel()
    {
        if (selectMode == 0)//0-select action, 1-select target
        {
            //if 0, got to previous player
            switch (activeCharNum)
            {
                case 1://cant go back further
                    return;
                    //break;

                case 2:
                    //go to character 1 
                    SetNextActiveChar(1, false);
                    //if (activeChar.defending)
                        //StateMachine.mana -= defenseManaAmount; 
                    break;

                case 3:
                    //go to character 2
                    SetNextActiveChar(2, false);
                    //if (activeChar.defending)
                        //StateMachine.mana -= defenseManaAmount;
                    break;
            }
            _selectionPlayer.transform.position = activeChar.transform.position;
            _actionUI.SetActive(false);
            _selectionEnemy.gameObject.SetActive(true);
            selectMode = 1;
            if (activeChar.defending)
            {
                //try to make sure previous character is alive before removing mana
                if((activeChar == _char2 && _char1.alive) || (activeChar == _char3 && _char2.alive) 
                    || (activeChar == _char3 && _char1.alive) || activeChar == _char1)
                    StateMachine.mana -= defenseManaAmount;
                OnPressedCancel();
            }
        }
        else
        {
            //if 1, go back to action selection
            selectMode = 0;
            _actionUI.SetActive(true);
            _selectionEnemy.gameObject.SetActive(false);
            if (activeChar._attackPlan == "magic")
                StateMachine.mana += activeChar.spellCost;
        }
    }

    void OnPressedLeft()
    {
        if (selectMode == 0) { 
            if (Vector3.Distance(_selectionBorder.transform.position, _defendButton.transform.position) <= buttonDistance)
            {
                //go to magic button
                SetActiveButtonValues("magic");
            }
            else if (Vector3.Distance(_selectionBorder.transform.position, _magicButton.transform.position) <= buttonDistance)
            {
                //go to attack button
                SetActiveButtonValues("attack");
            }
            else if (Vector3.Distance(_selectionBorder.transform.position, _attackButton.transform.position) <= buttonDistance)
            {
                //go to defend button
                SetActiveButtonValues("defend");
            }
        }
    }

    void OnPressedRight()
    {
        if (selectMode == 0)
        {
            if (Vector3.Distance(_selectionBorder.transform.position, _attackButton.transform.position) <= buttonDistance)
            {
                //go to magic button
                SetActiveButtonValues("magic");
            }
            else if (Vector3.Distance(_selectionBorder.transform.position, _magicButton.transform.position) <= buttonDistance)
            {
                //go to defend button
                SetActiveButtonValues("defend");
            }
            else if (Vector3.Distance(_selectionBorder.transform.position, _defendButton.transform.position) <= buttonDistance)
            {
                //go to attack button
                SetActiveButtonValues("attack");
            }
        }
    }

    void OnPressedUp()
    {
        if (selectMode == 1)
        {
            if (activeTargetNum <= enemies.Length - 2)
            {
                //Debug.Log("set target to higher in list");
                activeTargetNum++;
            }
            else
            {
                //Debug.Log("set target to end of list");
                activeTargetNum = 0;
            }
            activeTarget = enemies[activeTargetNum];
            SetCharacterTarget(activeTarget);
        }
    }

    void OnPressedDown()
    {
        if (selectMode == 1)
        {
            if (activeTargetNum - 1 >= 0)
            {
                //Debug.Log("set target to higher in list");
                activeTargetNum--;
            }
            else
            {
                //Debug.Log("set target to end of list");
                activeTargetNum = enemies.Length - 1;
            }
            activeTarget = enemies[activeTargetNum];
            SetCharacterTarget(activeTarget);
        }
    }
    

    void SetActiveButtonValues(string button)
    {
        AudioHelper.PlayClip2D(_selectSound, .1f);
        switch (button)
        {
            case "attack":
                //alter stage after attack
                StateMachine.attackPlan = "win";
                //_playerChoiceTextUI.text = "Player's Action: " + StateMachine.attackPlan;
                _magicCostTextUI.gameObject.SetActive(false);
                _selectionBorder.transform.position = _attackButton.transform.position;
                activeChar._attackPlan = "attack";
                activeChar.defending = false;
                break;

            case "magic":
                //alter stage after attack
                StateMachine.attackPlan = "nothing";
                //_playerChoiceTextUI.text = "Player's Action: " + StateMachine.attackPlan;
                _selectionBorder.transform.position = _magicButton.transform.position;
                activeChar._attackPlan = "magic";
                activeChar.defending = false;

                //show cost text
                if(_magicCostTextUI != null)
                {
                    _magicCostTextUI.gameObject.SetActive(true);
                    _magicCostTextUI.text = "Cost: " + activeChar.spellCost + " MP";
                }
                break;

            case "defend":
                //alter stage after attack
                StateMachine.attackPlan = "lose";
                //_playerChoiceTextUI.text = "Player's Action: " + StateMachine.attackPlan;
                _magicCostTextUI.gameObject.SetActive(false);
                _selectionBorder.transform.position = _defendButton.transform.position;
                activeChar._attackPlan = "defend";
                activeChar.defending = true;
                break;
        }
        //Debug.Log(activeChar + " set to " + button + ", defending == " + activeChar.defending);
    }

    void SetCharacterTarget(EnemyBase target)
    {
        _selectionEnemy.transform.position = target.transform.position;
        HealthBase targetHB = target.GetComponent<HealthBase>();

        if (targetHB != null) {
            //activeChar.TargetGroup[0] = targetHB;
            activeChar.AddTarget(targetHB);
        }
    }

    void SetNextActiveChar(int nextCharNum = 1, bool forward = true)
    {
        switch (nextCharNum)
        {
            case 1:
                if (_char1 == null)
                    break;

                if (_char1.alive)
                {
                    activeCharNum = 1;
                    activeChar = _char1;
                    if(forward)
                        SetActiveButtonValues("attack");
                }
                else
                {
                    if (forward)
                    {
                        nextCharNum = 2;
                        SetNextActiveChar(nextCharNum, forward);
                    }
                }
                break;

            case 2:
                if (_char2 == null)
                    break;

                if (_char2.alive)
                {
                    activeCharNum = 2;
                    activeChar = _char2;
                    if (forward)
                        SetActiveButtonValues("attack");
                }
                else
                {
                    if (forward)
                        nextCharNum = 3;
                    else
                        nextCharNum = 1;

                    SetNextActiveChar(nextCharNum, forward);
                }
                break;

            case 3:
                if (_char3 == null)
                    break;

                if (_char3.alive)
                {
                    activeCharNum = 3;
                    activeChar = _char3;
                    if (forward)
                        SetActiveButtonValues("attack");
                }
                else
                {
                    if (!forward)
                    {
                        nextCharNum = 2;
                        SetNextActiveChar(nextCharNum, forward);
                    }
                    else
                    {
                        //go to attack phase
                        //Debug.Log("Attempt to Enter Player Attack State!");
                        StateMachine.ChangeState(StateMachine.PlayerAttackState);
                    }
                }
                break;
        }
        //SetActiveButtonValues("attack");
    }
}
