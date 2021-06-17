using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed=10;
    private int desiredLine = 1;//0=left,1=middle,2=right
    public float laneDistance = 4;//the distance betweent two lanes
    public float JumpForce=10;
    public float Gravity = -20;
    public Transform targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        direction.z = forwardSpeed;

        if (controller.isGrounded)
        {
           
            if (Input.GetKeyDown(KeyCode.UpArrow)||SwipeManager.swipeUp)
            {
                Jump();
            }
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }

       


        if (Input.GetKeyDown(KeyCode.RightArrow)||SwipeManager.swipeRight)
        {
            desiredLine++;
            if (desiredLine == 3)
                desiredLine = 2;
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)||SwipeManager.swipeLeft)
        {
            desiredLine--;
            if (desiredLine == -1)
                desiredLine = 0;
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLine == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }else if (desiredLine == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        // transform.position = Vector3.Lerp(transform.position,targetPosition,70*Time.deltaTime);
        if (transform.position == targetPosition) return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
        {
            controller.Move(moveDir);
        }
        else
            controller.Move(diff);
        
    }
    private void FixedUpdate()
    {
        controller.Move(direction * Time.fixedDeltaTime);
    }
    private void Jump()
    {
        direction.y = JumpForce;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
        }
    }
}
