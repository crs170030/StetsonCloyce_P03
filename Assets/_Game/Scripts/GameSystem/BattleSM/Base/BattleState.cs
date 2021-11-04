using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BattleSM))]
public class BattleState : State
{
    protected BattleSM StateMachine { get; private set; }

    void Awake()
    {
        StateMachine = GetComponent<BattleSM>();
    }
}
