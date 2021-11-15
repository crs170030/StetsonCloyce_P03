using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthBase : MonoBehaviour, IDamageable<float>
{
    [SerializeField] float maxHealth = 10f;
    [SerializeField] GameObject _artGroup = null;
    [SerializeField] GameObject _deathEffect = null;
    [SerializeField] AudioClip _deathSound = null;
    [SerializeField] AudioClip _hurtSound = null;
    public float currentHealth;
    int repeatNum = 5;
    bool calledHalfHealth = false;
    public bool flashActive = true;

    public event Action<float> Damaged = delegate { };
    public event Action<float> Healed = delegate { };
    public event Action<int> HalfHealth = delegate { };

    void Start()
    {
        restoreHealth();
    }

    public void restoreHealth()
    {
        currentHealth = maxHealth;
        Healed.Invoke(maxHealth);
    }

    public void AtHalfHeath()
    {
        HalfHealth?.Invoke(1);
    }

    public virtual void TakeDamage(float damageTaken)
    {
        //ouch chihuahua
        //OnTakeDamage?.Invoke();
        Damaged.Invoke(damageTaken);

        currentHealth -= damageTaken;
        if (currentHealth <= (maxHealth / 2) && !calledHalfHealth)//call half health warnings
        {
            AtHalfHeath();
            calledHalfHealth = true;
        }
        //Debug.Log(this.name + " was hit! Health is now " + currentHealth);
        //flash art group
        if (_artGroup != null)
            StartCoroutine(flashArt(_artGroup));

        if (currentHealth <= 0)
        {
            Kill();
        }
        else
        {
            if (_hurtSound != null)
                AudioHelper.PlayClip2D(_hurtSound, .7f);
        }
    }

    public void Kill()
    {
        //time to die
        //Debug.Log("You've... killed me... The Great " + this.name);

        if (_deathEffect != null)
        {
            //make hit effect
            GameObject deathGO = Instantiate(_deathEffect, transform.position, transform.rotation);
            Destroy(deathGO, 3f); //eventually destroy
        }
        if (_deathSound != null)
        {
            //play sounds
            AudioHelper.PlayClip2D(_deathSound, 1f);
        }

        Destroy(gameObject, .2f);
    }

    private IEnumerator flashArt(GameObject artGroup)
    {
        //do we still want things to flash when they take damage?
        if (flashActive)
        {
            //Debug.Log("Starting flash");
            for (int i = 0; i < repeatNum; i++)
            {
                artGroup.SetActive(false);
                yield return new WaitForSeconds(.05f);
                artGroup.SetActive(true);
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}
