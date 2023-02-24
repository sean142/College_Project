using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    //public CharacterData_SO templateData;

    public CharacterData_SO characterData;

    public AttackData_SO attackData;

    public bool isCritical;
    /*
    private void Awake()
    {
        if (templateData != null)
            characterData = Instantiate(templateData);
    }
    */
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
            Debug.Log("test");
        }

        //TODO Update UI        
        //TODO 經驗updata
    }

    private int CurrentDamage()
    {
        //隨機從最小跟最大的攻擊傷害之間選擇一個數值
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);

        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
            Debug.Log("爆擊" + coreDamage);
        }
        return (int)coreDamage;
    }
}
