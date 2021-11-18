using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterCommand
{
    void SetAction(string attackPlan);
    void ResetAction();
}
