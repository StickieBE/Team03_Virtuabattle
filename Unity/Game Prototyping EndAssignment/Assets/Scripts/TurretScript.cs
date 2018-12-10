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

                _layermask = DefineLayerMask(TeamNumber);

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
                    Shoot(BulletPrefab, SpawnPosition, Enemies[Enemies.Length - 1].transform.position + Enemies[Enemies.Length - 1].transform.forward * 0.6f, Head.transform.rotation);
                    Timer = ShootTime;
                }

                foreach (Collider enemy in Enemies)
                {
                    if (enemy.GetComponent<AIScript>().HasTarget == false)
                    {
                        enemy.GetComponent<AIScript>().Target = Head;
                        enemy.GetComponent<AIScript>().HasTarget = true;
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

    public static void Shoot(GameObject Projectile, Vector3 SpawnPos, Vector3 Target, Quaternion Rotation)
    {

        GameObject Bullet = Instantiate(Projectile, SpawnPos, Rotation);
        Bullet.GetComponent<BulletScript>().Target = Target;
        Bullet.GetComponent<BulletScript>().Origin = SpawnPos;

    }

    public static LayerMask DefineLayerMask(int ObjectTeamNumber)
    {
        LayerMask Layermask = new LayerMask();

        switch (ObjectTeamNumber)
        {
            case 1:
                 Layermask = ((1 << 11) | (1 << 12) | (1 << 13));
                break;
            case 2:
                Layermask = ((1 << 10) | (1 << 12) | (1 << 13));
                break;
            case 3:
                Layermask = ((1 << 11) | (1 << 10) | (1 << 13));
                break;
            case 4:
                Layermask = ((1 << 11) | (1 << 12) | (1 << 10));
                break;
            default:
                break;
        }
        return Layermask;
    }
}
