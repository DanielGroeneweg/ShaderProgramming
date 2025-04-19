using UnityEngine;
public class CameraControls : MonoBehaviour
{
    [SerializeField] private float speed = 10;

    private bool forward = false;
    private bool left = false;
    private bool back = false;
    private bool right = false;
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) forward = true;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) left = true;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) back = true;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) right = true;
    }
    private void FixedUpdate()
    {
        if (forward) transform.position += transform.forward * speed;
        if (left) transform.position -= transform.right * speed;
        if (back) transform.position -= transform.forward * speed;
        if (right) transform.position += transform.right * speed;

        forward = false;
        left = false;
        back = false;
        right = false;
    }
}