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
    public int playersAlive = 0;
    public float mana = 0;
    CharacterBase[] players = null;
    HealthBase hb = null;

    private void Awake()
    {
        //create my states
        //SetupState = new SetupBattleState
        //Debug.Log("BattleSM: Awaken my Masters!");
        SetupState = GetComponent<SetupBattleState>();
        PlanState = GetComponent<PlayerTurnPlanState>();
        PlayerAttackState = GetComponent<PlayerTurnBattleState>();
        EnemyAttackState = GetComponent<EnemyTurnState>();
        Win = GetComponent<WinState>();
        Lose = GetComponent<LoseState>();
        MainMenu = GetComponent<MainMenuState>();

        //get alerted for player deaths
        players = FindObjectsOfType<CharacterBase>();
        foreach(CharacterBase charBase in players){
            //Debug.Log("BattleSM: added " + charBase + " to players");
            hb = charBase.GetComponent<HealthBase>();
            if (hb != null)
            {
                hb.OnDeath += HandleDeath;
                hb.Healed += HandleRevive;
            }
        }
    }

    private void Start()
    {
        ChangeState(MainMenu);
    }

    void HandleDeath()
    {
        playersAlive -= 1;
        Debug.Log("State Machine - Death: players Alive == " + playersAlive);
    }

    void HandleRevive(float maxHealth)
    {
        if (playersAlive < 3)
        {
            playersAlive += 1;
            Debug.Log("State Machine - Revive: players Alive == " + playersAlive);
        }
    }
}
