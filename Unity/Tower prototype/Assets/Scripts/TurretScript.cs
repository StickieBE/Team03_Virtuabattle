using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour {

    private Transform Target;

    [Header("Attributes")]

    public float Range = 3f;
    public float FireRate = 1f;
    private float FireCountdown = 0f;

    [Header("Unity Setup Fields")]

    public string EnemyTag = "Enemy";
    public Transform PartToRotate;

    [Header("Bullet fields")]

    public GameObject BulletPrefab;
    public Transform FirePoint;


	// Use this for initialization
	void Start () {

        InvokeRepeating("UpdateTarget", 0f, 0.5f);

	}
	

    void UpdateTarget ()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {

                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;

            }
        }


        if (nearestEnemy != null && shortestDistance <= Range)
        {

            Target = nearestEnemy.transform;

        }
        else
        {
            Target = null;
        }


    }

	// Update is called once per frame
	void Update () {

        if (Target == null)
            return;

        Vector3 dir = Target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        PartToRotate.rotation = Quaternion.Euler(0f, rotation.y + 90f, 0f);


        if (FireCountdown <= 0f)
        {

            Shoot();
            FireCountdown = 1f / FireRate;

        }

        FireCountdown -= Time.deltaTime;


	}

    void Shoot()
    {
        Debug.Log(" SHOOT ");
        GameObject bulletGO = (GameObject)Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);

        BulletScript bullet = bulletGO.GetComponent<BulletScript>();

        if (bullet != null)
            bullet.Seek(Target);
    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);


    }
}
