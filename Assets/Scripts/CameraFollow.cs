using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameObject player;
    Vector3 playerPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position;
        transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);        
    }
}
