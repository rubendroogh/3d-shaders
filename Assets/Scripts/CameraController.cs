using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private Vector2 rotation;
    private float sensitivity = .4f;
    private Text sensitivityText;

    private void Start()
    {
        this.sensitivityText = GameObject.Find("Camera speed").GetComponent<Text>();
    }

    private void Update()
    {
        this.sensitivity += Input.mouseScrollDelta.y * .01f;
        this.sensitivity = Mathf.Clamp(this.sensitivity, 0, 1);
        this.sensitivityText.text = $"Camera sensitivity: {this.sensitivity.ToString()}";

        if (Input.GetMouseButton(1))
        {
            rotation.x += Input.GetAxis("Mouse X") * 5f;
            rotation.y -= Input.GetAxis("Mouse Y") * 5f;

            rotation.x = Mathf.Repeat(rotation.x, 360);
            rotation.y = Mathf.Clamp(rotation.y, -90, 90);

            this.transform.rotation = Quaternion.Euler(rotation.y, rotation.x, 0);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += this.transform.forward * sensitivity;
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position -= this.transform.right * sensitivity;
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += this.transform.right * sensitivity;
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position -= this.transform.forward * sensitivity;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            this.transform.position += this.transform.up * sensitivity;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            this.transform.position -= this.transform.up * sensitivity;
        }
    }
}
