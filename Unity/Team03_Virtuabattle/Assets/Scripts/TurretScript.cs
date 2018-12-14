using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour {

    // Turret parts
    public GameObject Barrel { get; set; }
    GameObject turretHead, turretBase;
    Transform defaultHeadPosition;

    // Team-related
    public int TeamNumber { get; set; }
    public Color TurretColor;

    // Easy to read code
    bool HasTarget => ((_target == null) ? false : true);
    bool EnemiesNear => (_enemies.Count > 0);

    // Shooting-related
    public GameObject BulletPrefab;
    private List<GameObject> _enemies = new List<GameObject>();
    GameObject _target;
    public float ShootTime;
    private float Timer;
    public LayerMask RaycastIgnoreLayer;
    bool headRotationReset = true;

    // Turret variables
    public bool Captured;
    public int Health;

    //private LayerMask _layermask;

    private void Awake()
    {
        FindAllTurretParts();
    }

    // Use this for initialization
    void Start () {
        //if (TeamNumber != 2) Destroy(gameObject);
        Timer = ShootTime;
        Captured = false;
        defaultHeadPosition = turretHead.transform;
    }
	
	// Update is called once per frame
	void Update () {

        Timer -= Time.deltaTime;

        if (EnemiesNear) HandleCombat();
        else
        {
            //#if DEBUG
            //            Debug.Log(
            //                string.Format(
            //                    "{0} enemies near, scanning...",
            //                    _enemies.Count
            //                )
            //            );
            //#endif
            if (!headRotationReset && !HasTarget) ResetHead();
        }
        //CheckCaptured();

    }

    private void FindAllTurretParts()
    {
        foreach (Transform _turretPart in GetComponentInChildren<Transform>())
        {
            switch (_turretPart.name)
            {
                case ("Base"):
                    turretBase = _turretPart.gameObject;
                    break;
                case ("Head"):
                    turretHead = _turretPart.gameObject;
                    break;
                case ("Barrel"):
                    Barrel = _turretPart.gameObject;
                    break;
                default:
                    break;
            }
        }
        foreach (Transform _turretPart in turretHead.GetComponentInChildren<Transform>())
            if (_turretPart.name == "Barrel") Barrel = _turretPart.gameObject;
    }

    private void HandleCombat()
    {
        if (HasTarget)
        {
            //#if DEBUG
            //            Debug.Log(
            //                string.Format(
            //                    "{0} enemies near, focussing {1}",
            //                    _enemies.Count,
            //                    _target
            //                )
            //            );
            //#endif
            FocusTarget();
            //FilterTargetsBehindWalls();
            if (Timer <= 0) ShootTarget();
        }
        else
        {
            if (EnemiesNear) FindTarget();
        }
    }

    private void FilterTargetsBehindWalls()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            GameObject _enemyToCheck = _enemies[i];
            //Debug.Log(_enemyToCheck);
            if (_enemyToCheck != null)
                if (!RayCastCheck(_enemyToCheck)) RemoveEnemyFromList(i);
        }
    }

    private void RemoveEnemyFromList(int i)
    {
        Debug.Log("Removing enemy behind walls");
        _enemies.RemoveAt(i);
    }

    private bool RayCastCheck(GameObject enemyToCheck)
    { 
        Vector3 _origin = Barrel.transform.position, _direction = Barrel.transform.forward, _enemy = enemyToCheck.transform.position;
        float _traceLength = Vector3.Distance(_origin, _enemy);

        RaycastHit hit;
        Physics.Raycast(_origin, _direction, out hit, _traceLength, ~RaycastIgnoreLayer);
        Debug.DrawRay(_origin, _direction, Color.red, 10f);

        //Debug.Log(
        //    string.Format(
        //        "Raycast hit gameobject {0} with tag {1} and had a length of {2}",
        //        hit.transform.gameObject,
        //        hit.transform.gameObject.tag,
        //        _traceLength
        //    )
        //);

        if (hit.collider != null)
            if (hit.transform.gameObject.tag == "Tank")
                return true;
        return false;
    }

    private void FindTarget()
    {
        //Debug.Log("Looking for target");
         _target = _enemies[0];
    }

    private void ResetHead()
    {
        //Debug.Log("Reset turret head to default position");
        headRotationReset = true;
        turretHead.transform.LookAt(defaultHeadPosition.forward);
    }

    private void FocusTarget()
    {
        //Debug.Log("Focussing target");
        headRotationReset = false;
        turretHead.transform.LookAt(_target.transform);
    }

    private void ShootTarget()
    {
        //Debug.Log("Shooting at target");
        GameObject Bullet = Instantiate(BulletPrefab, Barrel.transform.position, Barrel.transform.rotation);
        Bullet.GetComponent<BulletScript>().Shoot(_target, Barrel, TeamNumber);
        Timer = ShootTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject _gameObject = other.gameObject;
        if (IsValidEnemy(_gameObject))
        {
            if (!CheckIfAlreadyAdded(_gameObject))
            {
                _enemies.Add(_gameObject);
                //Debug.Log(
                //    string.Format(
                //        "Added enemy {0} with tag {1}",
                //        _gameObject,
                //        _gameObject.tag
                //    )
                //);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject _gameObject = other.gameObject;
        if (IsValidEnemy(_gameObject))
        {
            if (CheckIfAlreadyAdded(_gameObject))
            {
                if (_target == _gameObject) _target = null;
                _enemies.Remove(_gameObject);
                //Debug.Log(
                //    string.Format(
                //        "Removed enemy {0} with tag {1}",
                //        _gameObject,
                //        _gameObject.tag
                //    )
                //);
            }
        }
    }

    private bool IsValidEnemy(GameObject _collider)
    {
        return (
            _collider.tag == "Tank"
            && _collider.GetComponent<AIScript>() != null
            && _collider.GetComponent<AIScript>().TeamNumber != TeamNumber
        );
    }

    private bool CheckIfAlreadyAdded(GameObject collider)
    {
        for (int i = 0; i < _enemies.Count; i++)
            if (collider == _enemies[i]) return true;
        return false;
    }

    private void CheckCaptured()
    {
        //if (!Captured)
        //{
        //    _layermask = ((1 << 10) | (1 << 11) | (1 << 12) | (1 << 13));
        //    Enemies = Physics.OverlapSphere(Base.transform.position, 5, _layermask);
        //    if (Enemies.Length > 0)
        //    {
        //        //TeamNumber = Enemies[0].gameObject.GetComponent<AIScript>().TeamNumber;

        //        //_layermask = DefineLayerMask(TeamNumber);

        //        foreach (Renderer material in Head.GetComponentsInChildren<Renderer>())
        //        {
        //            material.material = TurretColor[TeamNumber - 1];
        //        }
        //        Captured = true;
        //    }
        //}
        //else
        //{
        //    Enemies = Physics.OverlapSphere(Base.transform.position, 5, _layermask);
        //    if (Enemies.Length > 0)
        //    {
        //        Head.transform.LookAt(Enemies[Enemies.Length - 1].transform);
        //        Timer -= Time.deltaTime;
        //        if (Timer <= 0)
        //        {
        //            Shoot(BulletPrefab, SpawnPosition, Enemies[Enemies.Length - 1].transform.position + Enemies[Enemies.Length - 1].transform.forward * 0.6f, Head.transform.rotation, gameObject);
        //            Timer = ShootTime;
        //        }

        //        foreach (Collider enemy in Enemies)
        //        {
        //            if (enemy.GetComponent<AIScript>().HasTarget == false)
        //            {
        //                enemy.GetComponent<AIScript>().Target = Head;
        //                enemy.GetComponent<AIScript>().HasTarget = true;
        //            }
        //        }
        //    }
        //}
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(turretBase.transform.position, 5);
    //    Gizmos.color = Color.red;
    //    foreach (GameObject enemy in _enemies)
    //    {
    //        if (enemy != null) Gizmos.DrawWireSphere(enemy.transform.position, 0.6f);
    //    }

    //}

    //public static LayerMask DefineLayerMask(int ObjectTeamNumber)
    //{
    //    LayerMask Layermask = new LayerMask();

    //    switch (ObjectTeamNumber)
    //    {
    //        case 1:
    //             Layermask = ((1 << 11) | (1 << 12) | (1 << 13));
    //            break;
    //        case 2:
    //            Layermask = ((1 << 10) | (1 << 12) | (1 << 13));
    //            break;
    //        case 3:
    //            Layermask = ((1 << 11) | (1 << 10) | (1 << 13));
    //            break;
    //        case 4:
    //            Layermask = ((1 << 11) | (1 << 12) | (1 << 10));
    //            break;
    //        default:
    //            break;
    //    }
    //    return Layermask;
    //}

}
