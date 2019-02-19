using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {

    public Transform playerBody;
    public float dpi;

    float xAxisClamp = 0.0f;

	void Awake () {
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update () {
        RotateCamera();
	}

    void RotateCamera()
    {
        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");

        float rotateX = MouseX * dpi;
        float rotateY = MouseY * dpi;

        xAxisClamp -= rotateY;

        Vector3 targetCam = transform.rotation.eulerAngles;
        Vector3 targetBody = playerBody.rotation.eulerAngles;

        targetCam.x -= rotateY;
        targetCam.z = 0;
        targetBody.y += rotateX;

        if (xAxisClamp > 90)
        {
            xAxisClamp = 90;
            targetCam.x = 90;
        }
        else if (xAxisClamp < -90)
        {
            xAxisClamp = -90;
            targetCam.x = 270;
        }


        transform.rotation = Quaternion.Euler(targetCam);
        playerBody.rotation = Quaternion.Euler(targetBody);
    }


}
