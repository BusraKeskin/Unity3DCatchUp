using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float LANE_DISTANCE = 3.0f;
    private CharacterController controller;
    private Animator anim;
    //public Rigidbody rb;
    //public float sidewayForce = 500f;
    //public float forwardForce = 2000f;
    public Vector3 jump;
    public float jumpForce = 8.0f;
    public float speed = 7.0f;
    private float gravity = 12.0f;
    private float verticalVelocity;
    public bool isGrounded = true;
    private int desiredLane = 1; 
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(0, 0, forwardForce * Time.deltaTime);

        if(MobileInput.Instance.SwipeLeft)
            MoveLane(false);
        if(MobileInput.Instance.SwipeRigth)
            MoveLane(true);

        Vector3 targetPositon = transform.position.z * Vector3.forward;
        if(desiredLane == 0)
            targetPositon += Vector3.left * LANE_DISTANCE;
        else if(desiredLane == 2)
            targetPositon += Vector3.right * LANE_DISTANCE;

        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPositon - transform.position).normalized.x * speed;

        bool isGrounded = IsGrounded();
        anim.SetBool("Grounded", isGrounded);

        if(IsGrounded()){   
            verticalVelocity = -0.1f;
            if(MobileInput.Instance.SwipeUp){
                anim.SetTrigger("Jump");
                verticalVelocity = jumpForce;

            }
        }
        else
        {
            verticalVelocity -=  (gravity * Time.deltaTime);
        }
        moveVector.y = -0.1f;
        moveVector.z = speed;

        //Move the player
        controller.Move(moveVector * Time.deltaTime);

        
    }
    private void MoveLane(bool goingRight){
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane += Mathf.Clamp(desiredLane, 0, 2);

    }
    private bool IsGrounded(){
        Ray groundRay = new Ray(
            new Vector3(controller.bounds.center.x,
            (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f,
            controller.bounds.center.z),
            Vector3.down);
            
        return Physics.Raycast(groundRay, 0.2f + 0.1f);
            
    }

}
