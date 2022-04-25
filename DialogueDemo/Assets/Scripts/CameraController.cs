using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Look Data")]
    public float mSensitivity = 100.0f;
    public Transform mTrans;
    public bool mLockLook = false;
    private float mRotX = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (mLockLook)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            //Mouse Controls camera
            float lookX = Input.GetAxis("Mouse X") * mSensitivity * Time.deltaTime;
            float lookY = Input.GetAxis("Mouse Y") * mSensitivity * Time.deltaTime;

            mRotX -= lookY;
            mRotX = Mathf.Clamp(mRotX, -90.0f, 90.0f);

            transform.localRotation = Quaternion.Euler(mRotX, 0.0f, 0.0f);
            mTrans.Rotate(Vector3.up * lookX);
        }
    }
}
