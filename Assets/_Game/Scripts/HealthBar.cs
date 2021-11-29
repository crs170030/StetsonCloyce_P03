using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    [SerializeField] HealthBase _hb = null;
    [SerializeField] GameObject vignette = null;

    private void Awake()
    {
        //get reference to the health base
        //_hb = GetComponent<HealthBase>();
        if (_hb != null)
        {
            //subscribe to damage event
            _hb.Damaged += LowerHealth;
            //subscribe to heal event
            _hb.Healed += SetMaxHealth;
        }
        else
        {
            Debug.Log("AAAH! HealthBase is Null!");
        }
    }

    public void SetMaxHealth(float health)
    {
        //Debug.Log("Health bar maxed out at " + health);
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void LowerHealth(float health)
    { 
        slider.value -= health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        if(vignette != null)
            StartCoroutine("Flash");

        //Debug.Log("Health bar now at " + slider.value + " out of " + slider.maxValue);
    }

    //flash vignette
    IEnumerator Flash()
    {
        for (int i = 0; i < 2; i++) { 
            vignette.SetActive(true);
            yield return new WaitForSeconds(.1f);
            vignette.SetActive(false);
            yield return new WaitForSeconds(.1f);
        }
    }
}
