using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandMoving : MonoBehaviour {

    [SerializeField]
    Transform handpart1 = null;
    [SerializeField]
    float handpart1OffsetX, handpart1OffsetY, handpart1OffsetZ;
    [SerializeField]
    Transform handpart2 = null;
    [SerializeField]
    float handpart2OffsetX, handpart2OffsetY, handpart2OffsetZ;
    [SerializeField]
    Transform handpart3 = null;
    [SerializeField]
    float handpart3OffsetX, handpart3OffsetY, handpart3OffsetZ;
    [SerializeField]
    Transform handpart4 = null;
    [SerializeField]
    float handpart4OffsetX, handpart4OffsetY, handpart4OffsetZ;
    Vector3 startPos;
    PlayerMovement pMovement;
    float movemntAmount = 0;
    // Use this for initialization
    void Start()
    {
       startPos = transform.position;
        pMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        float movement = Mathf.Lerp(transform.position.x, startPos.x + handpart1OffsetX, Time.deltaTime);
        movemntAmount += movement;
        handpart1.Translate( movement, 0, 0);
        if (movemntAmount > 0)
        {
            if (movemntAmount >=  handpart1OffsetX)
            {
                handpart1OffsetX = -handpart1OffsetX;
                movemntAmount = 0;
            }
        }
        else if (movemntAmount < 0) {
            if (movemntAmount <= handpart1OffsetX)
            {
                handpart1OffsetX = -handpart1OffsetX;
                movemntAmount = 0;
            }

        }
        

    }
}
