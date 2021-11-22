using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public HealthBase[] TargetGroup = null;
    public float baseDamage = 25;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BaseAttack(float damage = 25)
    {
        //apply base Damage to list of targets
        if (TargetGroup != null)
        {
            foreach (HealthBase target in TargetGroup)
            {
                Debug.Log(this.name + " deals " + damage + " damage to " + target);
                target.TakeDamage(damage);
            }

            ClearTargets();
        }
        else
        {
            Debug.Log(this.name + " target's group is null!");
        }
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
