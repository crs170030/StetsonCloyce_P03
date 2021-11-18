using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour, ICharacterCommand
{
    public string _attackPlan = "none";
    public float baseDamage = 25;
    public float spellCost = 25;
    //float mana = 0;
    //float maxMana = 50;
    HealthBase[] TargetGroup = null;

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
            Debug.Log(this.name + " uses " + spellCost + " mana to cast a spell on " + target);
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
}
