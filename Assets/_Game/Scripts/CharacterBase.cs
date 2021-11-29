using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour, ICharacterCommand
{
    [SerializeField] Image _charImage = null;
    [SerializeField] Sprite _normalArt = null;
    [SerializeField] Sprite _deadArt = null;
    public string _attackPlan = "none";
    public float baseDamage = 25;
    public float spellCost = 25;
    public float spellDamage = 50;
    public bool alive = true;
    public bool defending = false;
    public float defense = 3;
    //float mana = 0;
    //float maxMana = 50;
    public HealthBase[] TargetGroup = null;
    //ref to change state machine tracker

    // Start is called before the first frame update
    void Start()
    {
        _attackPlan = "none";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAction(string attackPlan)
    {
        _attackPlan = attackPlan;
    }

    public void ResetAction()
    {
        _attackPlan = "none";
    }

    public void BaseAttack()
    {
        //apply base Damage to list of targets
        if (TargetGroup != null)
        {
            foreach (HealthBase target in TargetGroup)
            {
                Debug.Log(this.name + " deals " + baseDamage + " damage to " + target);
                target.TakeDamage(baseDamage);
            }

            ClearTargets();
        }
        else
        {
            Debug.Log(this.name + " target's group is null!");
        }
    }

    public void MagicAttack()
    {
        //takes list of targets and applies effects to them
        //each character needs a different spell, use switch statement on parent name?
        foreach (HealthBase target in TargetGroup)
        {
            Debug.Log(this.name + " uses " + spellCost + " mana to cast a spell on " + target 
                + " dealing " + spellDamage + " damage.");
            target.TakeDamage(spellDamage);
        }
        ClearTargets();
    }

    public void AddTarget(HealthBase target)
    {
        //TargetGroup.Add(target);
        TargetGroup = new HealthBase[1];
        TargetGroup[0] = target;
    }

    void ClearTargets()
    {
        //TargetGroup.Clear();
        TargetGroup = null;
    }

    public void ToggleAppearance()
    {
        //change sprite if the player is dead
        if (alive)
        {
            if (_charImage != null && _normalArt != null)
                _charImage.sprite = _normalArt;
        }
        else
        {
            if (_charImage != null && _deadArt != null)
                _charImage.sprite = _deadArt;
        }
    }

    public void Die(bool kill = true)
    {
        if (kill)
        {
            Debug.Log("Player " + this.name + " has fallen!");
            alive = false;
            //state machine already looks for character death event, it should update
        }
        else
        {
            alive = true;
        }
        ToggleAppearance();
    }
}
