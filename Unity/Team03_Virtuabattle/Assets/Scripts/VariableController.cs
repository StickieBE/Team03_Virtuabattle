using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableController : MonoBehaviour {

    public int Player;
    public int Health { get; private set; } = 50;
    public int Gold { get; private set; } = 20;
    //public int AmmunitionType1 { get; set; }
    //public int AmmunitionType2 { get; set; }
    //public int AmmunitionType3 { get; set; }
    //public int ActivePickup { get; set; }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RemoveHP(int amount)
    {
        Debug.Log(
            string.Format(
                "Player {0} got hit by a bullet, new health is {1}.",
                Player,
                Health
            )
        );
        Health -= amount;
    }

    public void AddGold(int amount)
    {
        Gold += amount;
    }

    public void RemoveGold(int amount)
    {
        Gold -= amount;
    }
}
