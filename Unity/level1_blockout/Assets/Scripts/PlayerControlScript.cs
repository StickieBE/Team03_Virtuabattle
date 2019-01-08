using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerControlScript : MonoBehaviour
{

    public int playerID = 1;
    public int controllerID = 1;

    public Material ActiveMat;
    public Material DeactiveMat;
    private MeshRenderer meshRenderer;



    public Text txtInfo;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
       
    }

    public void Active(int ctrl)
    {
        this.enabled = true;
        controllerID = ctrl;

        meshRenderer.material = ActiveMat;


        txtInfo.text = "Press 'B' to back out";

        
    }

    public void DeActive()
    {
        this.enabled = false;
        meshRenderer.material = DeactiveMat;

        txtInfo.text = "Press 'A' to join";

        
    }

   
}
