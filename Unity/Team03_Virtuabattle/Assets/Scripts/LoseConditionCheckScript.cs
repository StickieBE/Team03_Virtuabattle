using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseConditionCheckScript : MonoBehaviour {

    public List<GameObject> Enemies;
    public int TeamNumber;
    private float _timer;
    private bool _destroyed = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Enemies.RemoveAll(GameObject => GameObject == null);
        if (Enemies.Count >= 10 && _destroyed == false)
        {
            _timer += Time.deltaTime;
            if (_timer >= 10)
            {
                Lost();
            }
        }

	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tank" && other.gameObject.GetComponent<AIScript>().TeamNumber != TeamNumber && !CheckIfAlreadyAdded(other.gameObject))
        {
            Enemies.Add(other.gameObject);
        }
    }

    private bool CheckIfAlreadyAdded(GameObject collider)
    {
        for (int i = 0; i < Enemies.Count; i++)
            if (collider == Enemies[i]) return true;
        return false;
    }

    public void Lost()
    {

                GameObject[] _tanks;
                Debug.Log("Team " + TeamNumber + " Lost");
                _tanks = GameObject.FindGameObjectsWithTag("Tank");
                foreach (GameObject Tank in _tanks)
                {
                    if (Tank.GetComponent<AIScript>().TeamNumber == TeamNumber)
                    {
                        Destroy(Tank);
                    }
                }

                GameObject[] _players;
                _players = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject player in _players)
                {
                    if (player.GetComponent<VariableController>().Player == TeamNumber)
                    {
                        player.GetComponentInChildren<Camera>().transform.parent = GameObject.Find("Players").transform;
                        Destroy(player);
                    }
                }

                GameObject[] _turrets;

                _turrets = GameObject.FindGameObjectsWithTag("Turret");
                foreach (GameObject turret in _turrets)
                {
                    if (turret.GetComponent<TurretScript>() != null)
                    {
                        TurretScript _turretscript = turret.GetComponent<TurretScript>();
                        if (turret.GetComponent<TurretScript>().TeamNumber == TeamNumber)
                        {

                            Destroy(turret);
                            //turret.GetComponent<TurretScript>().BaseTurret = false;
                            //turret.GetComponent<TurretScript>().TurretDestroyedSet();
                        }
                    }
                }


                _destroyed = true;
                LevelController.Instance.WinConditionCheck();
                Debug.Log("Win");
            }

}
