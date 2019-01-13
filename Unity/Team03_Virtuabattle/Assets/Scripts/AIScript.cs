using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIScript : MonoBehaviour {

    public GameObject Barrel;
    public int TeamNumber { get; set; }
    public Vector3 Velocity { get; set; }

    public float Health;

    GameObject[] spawnPoints;

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

    private void Awake()
    {
        spawnPoints = LevelController.Instance.Spawns;
    }


    // Use this for initialization
    void Start () {


        _timer = ShootTime;

        _rndPathNumber = Random.Range(0, 4);

        while (TeamNumber-1 == _rndPathNumber)
        {
            _rndPathNumber = Random.Range(0, 4);
        }

        transform.position = spawnPoints[TeamNumber - 1].transform.position;
        _destination = spawnPoints[_rndPathNumber].transform;


        _agent = GetComponent<NavMeshAgent>();
        _agent.enabled = true;
        _agent.destination = _destination.position;
        //_agent.speed = 10;


    }
	
	// Update is called once per frame
	void Update () {

        Velocity = _agent.velocity;

        if (Health <= 0) Destroy(gameObject);

        _timer -= Time.deltaTime;


        if (Target != null)
        {
            gameObject.transform.LookAt(Target.transform);
            _shootPos = transform.position + (Target.transform.position - transform.position).normalized;
            if (_timer <= 0)
            {
                GameObject Bullet = Instantiate(BulletPrefab, transform.position, Barrel.transform.rotation);
                Bullet.GetComponent<BulletScript>().Shoot(Target, Barrel, TeamNumber);
                _timer = ShootTime;
            }
        }

        if (Vector3.Distance(_agent.destination, _agent.transform.position) <= 4f)
        {


            Vector3 _destination2;
            float _rndDestinationOffsetX = Random.Range(-17f, 17f);
            float _rndDestinationOffsetZ = Random.Range(-17f, 17f);

            _destination2 = new Vector3(_destination.position.x + _rndDestinationOffsetX, _destination.position.y, _destination.position.z + _rndDestinationOffsetZ);
            _agent.destination = _destination2;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, 0.5f);
    //    Gizmos.DrawWireSphere(transform.position, 2.5f);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AI" && other.gameObject.GetComponent<AIScript>() != null && other.gameObject.GetComponent<AIScript>().TeamNumber != TeamNumber) Target = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Target)
        {
            Target = null;
        }

    }

    public void RemoveHP(int amount)
    {
        Health -= amount;
    }
}
