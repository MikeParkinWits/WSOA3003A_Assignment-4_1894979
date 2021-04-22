using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public enum UnitType { PLAYER, ENEMY }

public class Unit : MonoBehaviour
{
    [Header("Unit Information")]
    public string unitName;
    public int unitLevel;

    public int unitDamage;

    public int unitMaxHP;
    public int unitCurrentHP;

    public int unitSpeed;

    public string playerType;

    private BattleSystem battleSystem;

    public UnitType unitType;

    private Actions actions;

    public int uniqueNum;

    public int attackPower;

    public int defensePower;

    public GameObject turnIndicator;

    void Awake()
    {
        battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();

        battleSystem.turnOrder.Add(this);

        uniqueNum++;
        //unitSpeed *= UnityEngine.Random.Range(1, 10); //FOR TESTING, TAKE OUT WHEN FINAL
    }

    public void TakeDamage(int damage)
    {
        unitCurrentHP -= damage;
    }

    public bool IsDead()
    {
        if (unitCurrentHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Heal(int healAmount)
    {
        unitCurrentHP += healAmount;

        if (unitCurrentHP >= unitMaxHP)
        {
            unitCurrentHP = unitMaxHP;
        }

        int speedChange = battleSystem.turnOrder.ElementAt(1).unitSpeed;

        foreach (var x in battleSystem.turnOrder)
        {
            x.unitSpeed -= speedChange;
        }

        battleSystem.turnOrder.First().unitSpeed += (speedChange) + 20;


        battleSystem.turnOrder = battleSystem.turnOrder.OrderBy(w => w.unitSpeed).ToList();
    }

    public void NormalAttackSpeed()
    {

        int speedChange = battleSystem.turnOrder.ElementAt(1).unitSpeed;

        foreach (var x in battleSystem.turnOrder)
        {
            x.unitSpeed -= speedChange;
        }

        battleSystem.turnOrder.First().unitSpeed += (speedChange) + battleSystem.currentAttackSpeed;

        Debug.Log("ATTACK SPEED PLAYER: " + battleSystem.currentAttackSpeed);


        battleSystem.turnOrder = battleSystem.turnOrder.OrderBy(w => w.unitSpeed).ToList();
    }

    public void EnemyAttackSpeed()
    {

        int speedChange = battleSystem.turnOrder.ElementAt(1).unitSpeed;

        foreach (var x in battleSystem.turnOrder)
        {
            x.unitSpeed -= speedChange;
        }

        battleSystem.turnOrder.First().unitSpeed += (speedChange) + battleSystem.currentAttackSpeed;

        Debug.Log("ATTACK SPEED ENEMY: " + battleSystem.currentAttackSpeed);

        battleSystem.turnOrder = battleSystem.turnOrder.OrderBy(w => w.unitSpeed).ToList();
    }

}
