using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public string OriginName;
    public Vector3 Target;

    public LayerMask IgnoreLayer;

    public Vector3 Origin { get; set; }
    public int ShootForce;

    private float _time;
    private Vector3 _direction;
	// Use this for initialization
	void Start () {
        _direction = (Target - Origin).normalized;
        _time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Rigidbody>().velocity = _direction *ShootForce;

        _time+=Time.deltaTime;

        if (_time>=5)
        {
            Destroy(gameObject);
        }

	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == OriginName) return;

        
        //Debug.Log("collider");
        if (other.tag == "AI" && other.gameObject.layer != IgnoreLayer)
        {
            Debug.Log("damage");
            AIScript _help = other.GetComponent<AIScript>();
            if (_help != null)
            {
                _help.health--;
                Destroy(gameObject);
            }
        }
        else if (other.tag == "Turret")
        {
            Debug.Log("turret");
            other.GetComponent<TurretScript>().Health -= 1;
            Destroy(gameObject);
        }
        else if (other.tag == "Bullet")
        {
            Debug.Log("Bullet");
        }
        else
        {
            Debug.Log(other.tag);
            Destroy(gameObject);
        }

    }
}
