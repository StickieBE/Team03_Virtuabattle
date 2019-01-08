using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    private Transform target;

    public float Speed = 70f;



    public void Seek(Transform _target)
    {

        target = _target;

    }


	void Update () {
		
        if (target == null)
        {

            Destroy(gameObject);
            return;

        }


        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = Speed * Time.deltaTime;


        if (dir.magnitude <= distanceThisFrame)
        {

            HitTarget();
            return;

        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);


	}

    private void HitTarget()
    {
        Debug.Log("HIT");
        Destroy(gameObject);

    }
}
