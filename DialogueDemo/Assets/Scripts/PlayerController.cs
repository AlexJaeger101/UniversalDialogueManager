using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move Data")]
    public float mMoveSpeed = 12.0f;
    private CharacterController mController;
    public bool mLockMovement = false;
    public float mGrav = -9.8f;
    private Vector3 mVel;
    public Transform mGroundChecker;
    public float mGroundDist = 0.5f;
    public LayerMask mGroundMask;

    [Header("Interact Data")]
    public float mInteractRange = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        mController = gameObject.GetComponent<CharacterController>();        
    }

    // Update is called once per frame
    void Update()
    {
        //Check if grounded
        if (Physics.CheckSphere(mGroundChecker.position, mGroundDist, mGroundMask) && mVel.y < 0)
        {
            mVel.y = -2.0f;
        }

        if (!mLockMovement)
        {
            //Movement Calc
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");
            Vector3 moveVec = transform.right * moveX + transform.forward * moveY;
            mController.Move(moveVec * mMoveSpeed * Time.deltaTime);
        }

        //Gravity Calc
        mVel.y += mGrav * Time.deltaTime;
        mController.Move(mVel * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, mInteractRange))
            {
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.red);
                if (hit.transform.gameObject.CompareTag("Interact"))
                {
                    Dialogue dialogue = hit.transform.gameObject.GetComponent<Talker>().mTalkerDialogue;
                    DialogueManager.Instance.StartDialogue(dialogue);
                }
            }
        }
    }
}
