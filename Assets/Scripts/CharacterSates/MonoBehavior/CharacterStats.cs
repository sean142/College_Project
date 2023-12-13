using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdataHealthBarOnAttack;

    public CharacterData_SO templateData;

    public CharacterData_SO characterData;

    public AttackData_SO attackData;

    [HideInInspector]
    public bool isCritical;

    //public void Update()
    //{
    //    if (UpdataHealthBarOnAttack != null)
    //    {
    //        UpdataHealthBarOnAttack.Invoke(CurrentHealth, MaxHealth);
    //    }
    //}

    private void Awake()
    {
        if (templateData != null)
            characterData = Instantiate(templateData);
    }
    
    public int MaxHealth
    {
        get
        {
            if (characterData != null)
            {
                return characterData.maxHealth;
            }
            else return  0;
        }
        set
        {
            characterData.maxHealth = value;
        }
    }
    
    public int CurrentHealth
    {
        get
        {
            if (characterData != null)
            {
                return characterData.currentHealth;
            }
            else return  0;
        }
        set
        {
            characterData.currentHealth = value;
        }
    }

    public int BaseDefence
    {
        get
        {
            if (characterData != null)
            {
                return characterData.baseDefence;
            }
            else return 0;
        }
        set
        {
            characterData.baseDefence = value;
        }
    }

    public int CurrentDefence
    {
        get
        {
            if (characterData != null)
            {
                return characterData.currentDefence;
            }
            else return 0;
        }
        set
        {
            characterData.currentDefence = value;
        }
    }

    public void TakeDamage(CharacterStats attacker ,CharacterStats defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurrentDefence,0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (attacker.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }

        // Update UI
        if (UpdataHealthBarOnAttack != null)
        {
            UpdataHealthBarOnAttack.Invoke(CurrentHealth, MaxHealth);
        }
        //TODO �g��updata
    }

    private int CurrentDamage()
    {
        //�H���q�̤p��̤j�������ˮ`������ܤ@�Ӽƭ�
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);

        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
            Debug.Log("�z��" + coreDamage);
        }
        return (int)coreDamage;
    }
}
