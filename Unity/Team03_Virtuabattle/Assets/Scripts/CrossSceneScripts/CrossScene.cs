using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossScene :MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


}

