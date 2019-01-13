using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletScript : MonoBehaviour {

    #region Fields

    //-------------- Public
    public string OriginName;
    public int ShootForce;

    //-------------- Private
    int team;
    GameObject _target;
    Rigidbody _rigidBody => GetComponent<Rigidbody>();
    float _time;
    Vector3 _direction;

    #endregion

    #region Properties

    public GameObject Origin { get; set; }

    #endregion

    #region Methods

    // Use this for initialization
    void Start () {

        if (_target != null)
        {
            Vector3 offset = _target.GetComponent<AIScript>() != null ? _target.GetComponent<AIScript>().Velocity : Vector3.zero;
            _direction = (_target.transform.position + offset - Origin.transform.position).normalized;
        }
        else
        {
            _direction = transform.forward;
        }
        _time = 0;
	}
	
	// Update is called once per frame
	void Update () {

        _rigidBody.velocity = _direction * ShootForce;

        _time += Time.deltaTime;

        if (_time >= 5) Destroy(gameObject);

	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Origin // Don't execute if bullet collides with own origin.
            || IsShootingFriendly(other.gameObject) // Don't execute if bullet collides with own team.
            || other.isTrigger // Don't execute if the collider is detection trigger.
        ) return;

        //Debug.Log(
        //    string.Format(
        //        "The bullet collided with {0} / {1} and came from {2} / {3}",
        //        other.tag,
        //        other.gameObject.name,
        //        Origin.tag,
        //        Origin.name
        //    )
        //);

        switch (other.tag)
        {
            case "Tank":
                AIScript _theScript = other.GetComponent<AIScript>()??null;
                //if (_theScript == null) _theScript = other.GetComponentInParent<AIScript>() ?? null;

                AIScript _aiScript = other.GetComponent<AIScript>();

                // Handles currency reward for killing
                if (_aiScript.Health - 1 <= 0)
                {
                    // Check if player
                    if (Origin.GetComponent<VariableController>() != null) Origin.GetComponent<VariableController>().AddGold(1);

                    // Add to player belonging to same team
                    else
                    {
                        foreach (GameObject player in LevelController.Instance.Players)
                        {
                            VariableController _varController = player.GetComponent<VariableController>();
                            if (_varController.Player == team) _varController.AddGold(1);
                        }
                    }
                }
                _aiScript.RemoveHP(1);
                break;
            case "Turret":
                TurretScript _turret = other.GetComponent<TurretScript>();

                // Handles currency reward for killing
                if (_turret.Health - 1 <= 0)
                {
                    _turret.TurretDestroyedSet();
                    Debug.Log("Team " + team + " Destroyed a turret from team " + _turret.TeamNumber);

                    // Check if player
                    if (Origin.GetComponent<VariableController>() != null) Origin.GetComponent<VariableController>().AddGold(2);

                    // Add to player belonging to same team
                    else LevelController.Instance.Players[team - 1].GetComponent<VariableController>().AddGold(2);
                }
                _turret.RemoveHP(1);
                break;
            case "Player":
                VariableController Target = other.GetComponent<VariableController>();

                // Handles currency reward for killing
                if (Target.Health - 1 <= 0)
                {
                    // Check if player
                    if (Origin.GetComponent<VariableController>() != null) Origin.GetComponent<VariableController>().AddGold(5);
                    // Add to player belonging to same team
                    else LevelController.Instance.Players[team - 1].GetComponent<VariableController>().AddGold(5);
                }

                if (Origin.tag == "Player")
                {
                    Target.RemoveHP(3);
                }
                else { Target.RemoveHP(1); }
                break;
            default:
                break;
        }

        Destroy(gameObject);

    }

    private bool IsShootingFriendly(GameObject _collider)
    {
        int _numberToCheck = FindTeamNumberOfObject(_collider);
        //if (_numberToCheck == 0) Debug.Break();
        return (_numberToCheck == team);
    }

    private int FindTeamNumberOfObject(GameObject collider)
    {
        return collider.GetComponent<AIScript>() != null ? collider.GetComponent<AIScript>().TeamNumber :
            collider.GetComponent<TurretScript>() != null ? collider.GetComponent<TurretScript>().TeamNumber : 0;
    }

    public void Shoot(GameObject target, GameObject barrel, int _team)
    {
        _target = target;

        //GameObject.FindGameObjectWithTag("Player").transform.position = Target.transform.position;

        Origin = barrel;
        team = _team;

        //Debug.Log(
        //    string.Format(
        //        "Bullet details: POS: {0} / {1} / {2} / {3}",
        //        transform,
        //        target,
        //        barrel,
        //        team
        //    )
        //);

        //Debug.Break();
    }

    public void ShootStraight(GameObject player, int _team)
    {
        Origin = player;
        team = _team;
    }

    #endregion

}
