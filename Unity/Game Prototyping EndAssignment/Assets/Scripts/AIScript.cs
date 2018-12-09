using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIScript : MonoBehaviour {

    public int TeamNumber;
    public float health;

    public Transform[] _spawnPoints;

    private int _rndPathNumber;
    private Transform _destination;
    private NavMeshAgent _agent;

    public bool _hasTarget = false;
    public GameObject Target;

    private float _timer;
    public float ShootTime;
    private Vector3 _shootPos;
    // Use this for initialization
    void Start () {

        _timer = ShootTime;


        _rndPathNumber = Random.Range(0, 4);

        while (TeamNumber-1 == _rndPathNumber)
        {
            _rndPathNumber = Random.Range(0, 4);
        }

        transform.position = _spawnPoints[TeamNumber - 1].position;
        _destination = _spawnPoints[_rndPathNumber];


        _agent = GetComponent<NavMeshAgent>();
        _agent.destination = _destination.position;
        gameObject.layer = TeamNumber + 9;

    }
	
	// Update is called once per frame
	void Update () {

        

        if (health <=0)
        { Destroy(gameObject); }

        _timer-= Time.deltaTime;

        if (_hasTarget == true)
        {
            gameObject.transform.LookAt(Target.transform);
            _shootPos = transform.position + (Target.transform.position - transform.position).normalized;
            if (_timer <=0)
            {
                Target.GetComponentInParent<TurretScript>().Shoot(_shootPos, Target.transform.position, gameObject.transform.rotation);
                _timer = ShootTime;
            }

            if (Vector3.Distance(transform.position, Target.transform.position) >= 3f)
            {
                Target = null;
                _hasTarget = false;
            }
        }




        if (Vector3.Distance(_agent.destination, _agent.transform.position) <=0.3f)
        {
            Destroy(gameObject);
        }
	}




    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
