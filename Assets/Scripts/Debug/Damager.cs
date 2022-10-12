using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Entities { PLAYER, ENEMY }

public class Damager : MonoBehaviour
{
    public int damage = 15;
    public Entities damages = Entities.PLAYER;
    /*// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
