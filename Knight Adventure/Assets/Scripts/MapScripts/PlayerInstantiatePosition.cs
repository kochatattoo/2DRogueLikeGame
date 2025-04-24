using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstantiatePosition : MonoBehaviour
{
    private Vector3 player_instatiate_position;
    // Start is called before the first frame update
    void Start()
    {
        player_instatiate_position = transform.position;
    }

    
}
