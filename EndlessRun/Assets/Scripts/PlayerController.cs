using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed=10;
    public float maxSpeed;
    private int desiredLine = 1;//0=left,1=middle,2=right
    public float laneDistance = 4;//the distance betweent two lanes
    public float JumpForce=10;
    public float Gravity = -20;
    public Transform targetPosition;

    private bool isSliding = false;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!PlayerManager.isGamestarted) return;

        //Increase Speed
        if(forwardSpeed<maxSpeed)
          forwardSpeed += 0.1f * Time.deltaTime;

        direction.z = forwardSpeed;
        animator.SetBool("IsGameStarted", true);

        if (controller.isGrounded)
        {
            animator.SetBool("IsGround", true);
            if (Input.GetKeyDown(KeyCode.UpArrow)||SwipeManager.swipeUp)
            {
                Jump();
                
            }
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
            
            animator.SetBool("IsGround", false);
        }



        if (Input.GetKeyDown(KeyCode.DownArrow) || SwipeManager.swipeDown&&!isSliding)
        {
            StartCoroutine(Slide());
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
        if (!PlayerManager.isGamestarted)
            return;
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
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }
    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("IsSliding", true);
        controller.center = new Vector3(0, -0.8f, 0);
        controller.height = 1;
        yield return new WaitForSeconds(1.5f);

        controller.center = new Vector3(0, 0, 0);
        controller.height = 1.7f;

        animator.SetBool("IsSliding", false);
        isSliding = false;
    }
}
