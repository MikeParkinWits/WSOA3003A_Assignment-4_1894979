using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBattleSpawn : MonoBehaviour
{

	[SerializeField] private SubClass[] myArray;
	public void SetValue(int index, SubClass subClass)
	{

		// Perform any validation checks here.
		myArray[index] = subClass;
	}
	public SubClass GetValue(int index)
	{
		// Perform any validation checks here.
		return myArray[index];
	}

	void Start()
	{
		Debug.Log(GetValue(1));
	}
}
[System.Serializable]
public class SubClass
{
	public int number;
	public Transform spawnPoint;
}

