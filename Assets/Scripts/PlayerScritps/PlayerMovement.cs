using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField]
    float playerSpeed = 1.0f;
    float currentSpeed = 0;
    [SerializeField]
    float jumpForce = 5.0f;
    [SerializeField]
    float gravity = 8.6f;
    CharacterController charController = null;

   float currentVerticalForce = 0.0f;
   float airDrag = 1.0f;

	// Use this for initialization
	void Start () {
        charController = GetComponent<CharacterController>();
        currentSpeed = playerSpeed;
    }
	
	// Update is called once per frame
	void Update () {

        CheckJump();
        Move();
       
	}
    private void CheckJump() {
        currentVerticalForce -= gravity * Time.deltaTime;
        if (charController.isGrounded)
        {
            airDrag = 0;
            currentSpeed = playerSpeed;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentVerticalForce = jumpForce;
            }

        }
        else {
            if (currentSpeed - 0.05f >= 0) {
                currentSpeed -= 0.05f;
            }
        }
    }
    
    private void Move()
    {      
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * z * currentSpeed + transform.right * x * currentSpeed + transform.up * currentVerticalForce;
        movement -= new Vector3(x * -airDrag, 0, z * -airDrag);
        charController.Move(movement * Time.deltaTime);
    }

}
