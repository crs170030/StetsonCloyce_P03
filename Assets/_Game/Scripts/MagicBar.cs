using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Text barText;

    [SerializeField] BattleSM _bsm = null;
    CharacterBase[] players = null;
    HealthBase hb = null;

    float pastMana = 35f;

    private void Awake()
    {
        //get reference to the health base
        //_bsm = GetComponent<BattleSM>();
        if (_bsm != null)
        {

        }
        else
        {
            Debug.Log("AAAH! BattleSM is Null!");
        }
        //SetMaxMana(100f);

        //get alerted for player hurting
        players = FindObjectsOfType<CharacterBase>();
        foreach (CharacterBase charBase in players)
        {
            hb = charBase.GetComponent<HealthBase>();
            if (hb != null)
            {
                hb.Damaged += PlayerHurt;
            }
        }
    }

    public void FixedUpdate()
    {
        if(_bsm != null && _bsm.mana != pastMana)
        {
            pastMana = _bsm.mana;
            ChangeValue(pastMana);
        }
    }

    public void SetMaxMana(float health)
    {
        //Debug.Log("Health bar maxed out at " + health);
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void ChangeValue(float newValue)
    {
        if (newValue > slider.maxValue)
            newValue = slider.maxValue;

        slider.value = newValue;
        fill.color = gradient.Evaluate(slider.normalizedValue);

        barText.text = newValue + "\n%"; 

       //Debug.Log("Magic bar now at " + slider.value + " out of " + slider.maxValue);
    }

    void PlayerHurt(float damageTaken)
    {
        _bsm.mana += damageTaken / 4;
    }
}
