using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour {
    public GameObject Head;
    public GameObject Base;
    public Collider[] Enemies;
    public GameObject BulletPrefab;
    public int TeamNumber;
    public bool Captured;
    public int Health;

    public Vector3 SpawnPosition { get; private set; }

    public Material[] TurretColor;
    private LayerMask _layermask;

    private Vector3 _spawnPosition;

    public float ShootTime;
    private float Timer;
    // Use this for initialization
    void Start () {
        Timer = ShootTime;
        Captured = false;
    }
	
	// Update is called once per frame
	void Update () {
        SpawnPosition = Head.transform.TransformPoint(Vector3.forward * 1.5f);
        CheckCaptured();



	}

    private void CheckCaptured()
    {
        if (!Captured)
        {
            _layermask = ((1 << 10) | (1 << 11) | (1 << 12) | (1 << 13));
            Enemies = Physics.OverlapSphere(Base.transform.position, 5, _layermask);
            if (Enemies.Length > 0)
            {
                TeamNumber = Enemies[0].gameObject.GetComponent<AIScript>().TeamNumber;
                switch (TeamNumber)
                {
                    case 1:
                        _layermask = ((1 << 11) | (1 << 12) | (1 << 13));
                        break;
                    case 2:
                        _layermask = ((1 << 10) | (1 << 12) | (1 << 13));
                        break;
                    case 3:
                        _layermask = ((1 << 11) | (1 << 10) | (1 << 13));
                        break;
                    case 4:
                        _layermask = ((1 << 11) | (1 << 12) | (1 << 10));
                        break;
                    default:
                        break;
                }
                foreach (Renderer material in Head.GetComponentsInChildren<Renderer>())
                {
                    material.material = TurretColor[TeamNumber - 1];
                }
                Captured = true;
            }
        }else
        {
            Enemies = Physics.OverlapSphere(Base.transform.position, 5, _layermask);
            if (Enemies.Length > 0)
            {
                Head.transform.LookAt(Enemies[Enemies.Length - 1].transform);
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    Shoot(SpawnPosition, Enemies[Enemies.Length - 1].transform.position + Enemies[Enemies.Length - 1].transform.forward * 0.6f, Head.transform.rotation);
                    Timer = ShootTime;
                }

                foreach (Collider enemy in Enemies)
                {
                    if (enemy.GetComponent<AIScript>()._hasTarget == false)
                    {
                        enemy.GetComponent<AIScript>().Target = Head;
                        enemy.GetComponent<AIScript>()._hasTarget = true;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Base.transform.position, 5);
        Gizmos.color = Color.red;
        foreach (Collider enemy in Enemies)
        {
            if (enemy != null) Gizmos.DrawWireSphere(enemy.transform.position, 0.6f);
        }

    }

    public void Shoot(Vector3 SpawnPos, Vector3 Target, Quaternion Rotation)
    {
        GameObject Bullet = Instantiate(BulletPrefab, SpawnPos, Rotation);
        Bullet.GetComponent<BulletScript>().Target = Target;
        Bullet.GetComponent<BulletScript>().Origin = SpawnPos;

    }
}
