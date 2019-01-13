using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{

    private float _timer;
    // Start is called before the first frame update
    void Start()
    {
        _timer = 0;

    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >=5)
        {
            Destroy(GameObject.FindGameObjectWithTag("GameController"));
            SceneManager.LoadScene("menu");
        }
    }
}
