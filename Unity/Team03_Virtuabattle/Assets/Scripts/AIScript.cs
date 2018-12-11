using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIScript : MonoBehaviour {

    public int TeamNumber;
    public float health;

    [HideInInspector]
    public Transform[] SpawnPoints;

    private Collider[] _enemies;

    private int _rndPathNumber;
    private Transform _destination;
    private NavMeshAgent _agent;
    private float _rndDestinationOffset;

    public bool HasTarget = false;
    public GameObject Target;

    private float _timer;
    public float ShootTime;
    private Vector3 _shootPos;

    private float _checkTimer = 0;
    public float _checkTimerCooldown;
    public GameObject BulletPrefab;


    // Use this for initialization
    void Start () {

        _timer = ShootTime;


        _rndPathNumber = Random.Range(0, 4);

        while (TeamNumber-1 == _rndPathNumber)
        {
            _rndPathNumber = Random.Range(0, 4);
        }

        transform.position = SpawnPoints[TeamNumber - 1].position;
        _destination = SpawnPoints[_rndPathNumber];


        _agent = GetComponent<NavMeshAgent>();
        _agent.destination = _destination.position;
        //gameObject.layer = TeamNumber + 9;

    }
	
	// Update is called once per frame
	void Update () {

        

        if (health <=0)
        { Destroy(gameObject); }

        //_timer-= Time.deltaTime;


        //if (HasTarget == true && Target != null)
        //{
        //    gameObject.transform.LookAt(Target.transform);
        //    _shootPos = transform.position + (Target.transform.position - transform.position).normalized;
        //    if (_timer <=0)
        //    {
        //        TurretScript.Shoot(BulletPrefab, _shootPos, Target.transform.position, gameObject.transform.rotation);
        //        _timer = ShootTime;
        //    }

        //    if (Vector3.Distance(transform.position, Target.transform.position) >= 3f)
        //    {
        //        Target = null;
        //        HasTarget = false;
        //    }

            
        //}

        //else
        //{
        //    _checkTimer -= Time.deltaTime;
        //    if (_checkTimer <=0)
        //    {
        //        _enemies = Physics.OverlapSphere(transform.position, 2.5f, TurretScript.DefineLayerMask(TeamNumber));
        //    }

        //    if (_enemies.Length>0)
        //    {
        //        Target = _enemies[_enemies.Length-1].gameObject;
        //        HasTarget = true;
        //    }

        //    else
        //    {
        //        _checkTimer = _checkTimerCooldown;
        //    }

        //}




        if (Vector3.Distance(_agent.destination, _agent.transform.position) <=0.3f)
        {
            if (Target != null)
            {
                _rndDestinationOffset = Random.Range(0f, 1f);
                _agent.destination = new Vector3(Target.transform.position.x + _rndDestinationOffset, Target.transform.position.y + _rndDestinationOffset, Target.transform.position.z + _rndDestinationOffset);
            }

        }
	}




    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
