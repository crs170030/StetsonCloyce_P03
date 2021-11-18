using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCommand : ICommand
{
    //sets character action for this turn

    ICharacterCommand _characterBase;
    string _attackPlan;

    public CharacterCommand(ICharacterCommand characterBase, string attackPlan)
    {
        _characterBase = characterBase;
        _attackPlan = attackPlan;
    }

    public void Execute()
    {
        _characterBase.SetAction(_attackPlan);
    }

    public void Undo()
    {
        _characterBase.ResetAction();
    }
}
