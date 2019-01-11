using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour {

    // Turret parts
    public GameObject Barrel { get; set; }
    [HideInInspector] public GameObject turretHead, turretBase;
    Transform defaultHeadPosition;

    // Team-related
    public int TeamNumber { get; set; }
    public Renderer TurretMaterial;

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
    private float _capturing = 0;
    public bool Captured;
    public int Health = 50;
    public Material[] stateMaterials = new Material[2];

    //Destruction-Related
    private float _destroyTimer = 0;
    public bool TurretDestroyed = false;
    public bool BaseTurret = false;

    //private LayerMask _layermask;

    private void Awake()
    {
        FindAllTurretParts();
    }

    // Use this for initialization
    void Start () {

        Timer = ShootTime;
        defaultHeadPosition = turretHead.transform;
        if (BaseTurret)
        {
            Captured = true;
            TurretDestroyedSet();
            TeamNumber = GetComponentInParent<BaseController>().TeamNumber;
        }
        if (!Captured)
        {
            turretHead.GetComponent<Renderer>().material = stateMaterials[0]; 
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (!TurretDestroyed)
        {
            Timer -= Time.deltaTime;

            if (EnemiesNear)

            {
                HandleCombat();
                SetEnemyTarget();
            }
            else
            {
                if (!headRotationReset && !HasTarget) ResetHead();
            }
        }
        else
        { TurretDestroyedTimer(); }
        



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
        if (HasTarget && Captured)
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

        if (Captured)
        {
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
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject _gameObject = other.gameObject;
        if (IsValidEnemy(_gameObject))
        {
            if (CheckIfAlreadyAdded(_gameObject))
            {
                _gameObject.GetComponent<AIScript>().Target = null;
                _gameObject.GetComponent<AIScript>().HasTarget = false;
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

    public void RemoveHP(int amount)
    {
        Health -= amount;
    }

    public void Capture(int teamnumber)
    {
        Debug.Log(_capturing);
        _capturing += Time.deltaTime;
        if (_capturing >= 3)
        {
            Captured = true;
            TeamNumber = teamnumber;
            Debug.Log(teamnumber);
            turretHead.GetComponent<Renderer>().material = LevelController.Instance.TeamColors[teamnumber - 1];
        }
    }

    private void SetEnemyTarget()
    {
        foreach (GameObject enemy in _enemies)
        {
            if (enemy != null)
            {
                AIScript _AIScript = enemy.GetComponent<AIScript>();
                if (_AIScript.HasTarget == false)
                {
                    _AIScript.Target = turretHead;
                    _AIScript.HasTarget = true;
                }
            }

        }
    }

    public void TurretDestroyedSet()
    {
        TurretDestroyed = true;
        _destroyTimer = 5;
        turretHead.GetComponent<Renderer>().material = stateMaterials[1];
    }

    private void TurretDestroyedTimer()
    {
        _destroyTimer -= Time.deltaTime;
        if (_destroyTimer <= 0)
        {
            if (BaseTurret)
            {
                turretHead.GetComponent<Renderer>().material = LevelController.Instance.TeamColors[TeamNumber - 1];
            }
            else
            {
                turretHead.GetComponent<Renderer>().material = stateMaterials[0];
                Captured = false;
                TeamNumber = 0;
            }
            TurretDestroyed = false;
            Health = 50;
            _capturing = 0;
        }
    }
}
