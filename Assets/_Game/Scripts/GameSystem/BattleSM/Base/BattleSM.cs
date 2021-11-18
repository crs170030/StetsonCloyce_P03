using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSM : StateMachineMB
{
    //[SerializeField]
    //private LightBotController _controller;

    //public SetupBattleState SetupState { get; private set; }  
    public SetupBattleState SetupState { get; private set; }
    public PlayerTurnPlanState PlanState { get; private set; }
    public PlayerTurnBattleState PlayerAttackState { get; private set; }
    public EnemyTurnState EnemyAttackState { get; private set; }
    public WinState Win { get; private set; }
    public LoseState Lose { get; private set; }
    public MainMenuState MainMenu { get; private set; }

    [SerializeField] InputController _input = null;
    [SerializeField] GameObject _battleUI = null;
    public InputController Input => _input;
    public GameObject BattleUI => _battleUI;

    //public int enemiesLeft = 0; //used to determine win con
    //public float playerHealth = 100; //TODO: Replace with health tracking for 3 characters
    public string attackPlan = "nothing";
    public int enemiesLeft = 0;
    public int playersAlive = 3;

    private void Awake()
    {
        //create my states
        //SetupState = new SetupBattleState
        //();
        SetupState = GetComponent<SetupBattleState>();
        PlanState = GetComponent<PlayerTurnPlanState>();
        PlayerAttackState = GetComponent<PlayerTurnBattleState>();
        EnemyAttackState = GetComponent<EnemyTurnState>();
        Win = GetComponent<WinState>();
        Lose = GetComponent<LoseState>();
        MainMenu = GetComponent<MainMenuState>();
    }

   private void Start()
    {
        ChangeState(MainMenu);
    }
}
