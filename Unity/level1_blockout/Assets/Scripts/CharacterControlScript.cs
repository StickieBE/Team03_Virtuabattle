using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterControlScript : MonoBehaviour
{
    public PlayerControlScript[] players;
    private List<int> activePlayers = new List<int>();

    

    void Update()
    {
        
        CheckInput();
    }

   

    void CheckInput()
    {
        //to join
        for (int i = 1; i < players.Length + 1; ++i)
        {
            if (!activePlayers.Contains(i) && Input.GetButtonDown("Select_P" + i))
            {
                activePlayers.Add(i);
                players[GetDevil()].Active(i);
            }
        }

        //to back out
        for (int i = 1; i < 5; ++i)
        {
            if (activePlayers.Contains(i) && Input.GetButtonDown("Back_P" + i))
            {
                activePlayers.Remove(i);
                DeactiveDevil(i);
            }
        }
    }

    int GetDevil()
    {
        for (int i = 0; i < players.Length; ++i)
        {
            if (!players[i].enabled)
            {
                return i;
            }
        }
        return 0;
    }

    void DeactiveDevil(int ctrl)
    {
        for (int i = 0; i < players.Length; ++i)
        {
            if (players[i].enabled && players[i].controllerID.Equals(ctrl))
            {
                players[i].DeActive();
                return;
            }
        }
    }
}
