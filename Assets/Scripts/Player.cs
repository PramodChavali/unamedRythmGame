using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private metronome metronome;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float distanceFromGroundToJump;


    public void Jump()
    {
        Debug.Log("jump called");
       

        rb.AddForce(transform.up * jumpHeight);
        Debug.Log("Jumping");

    }


    private bool IsInTime(float playerInputTime, float lastMetronomeTime)
    {
        return false;
    }
}
