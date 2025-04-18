using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    public GameObject player;
    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x+3, 50, player.transform.position.z-1);
    }
}
