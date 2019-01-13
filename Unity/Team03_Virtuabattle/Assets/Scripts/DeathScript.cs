using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour
{

    private VariableController _varContr;
    // Start is called before the first frame update
    void Start()
    {
        _varContr = GetComponent<VariableController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_varContr.Health <=0)
        {
            Death();
            
        }
    }

    public void Death()
    {
        if(_varContr.Gold >= 20)
        {
            _varContr.AddHP(50);
            _varContr.RemoveGold(20);
            gameObject.transform.position = LevelController.Instance.Spawns[_varContr.Player-1].transform.position;

        }

        else if (_varContr.Gold < 20)
        {
            LevelController.Instance.Spawns[_varContr.Player-1].GetComponentInChildren<LoseConditionCheckScript>().Lost();
        }

    }
}

