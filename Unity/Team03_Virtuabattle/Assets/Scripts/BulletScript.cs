using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public string OriginName;
    public GameObject Target;

    public GameObject Origin { get; set; }
    public int ShootForce;

    private float _time;
    private Vector3 _direction;

    Rigidbody _rigidBody => GetComponent<Rigidbody>();

    int team;

    // Use this for initialization
    void Start () {

        Vector3 offset = Target.GetComponent<AIScript>() != null ? Target.GetComponent<AIScript>().Velocity : Vector3.zero;
        _direction = (Target.transform.position + offset - Origin.transform.position).normalized;
        _time = 0;
	}
	
	// Update is called once per frame
	void Update () {

        _rigidBody.velocity = _direction * ShootForce;

        _time += Time.deltaTime;

        //if (_time >= 5) Destroy(gameObject);

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
                other.GetComponent<AIScript>().Health--;
                break;
            case "Turret":
                other.GetComponent<TurretScript>().Health -= 1;
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
        Target = target;

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
}
