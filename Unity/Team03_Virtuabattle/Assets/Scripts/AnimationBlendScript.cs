using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBlendScript : MonoBehaviour
{

    private CharController _charController;
    private Animator _bunnyAnimator;
    // Start is called before the first frame update
    void Start()
    {
        _charController = GetComponentInParent<CharController>();
        _bunnyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _bunnyAnimator.SetFloat("Speed", _charController.InputMovement.normalized.magnitude);

    }
}
