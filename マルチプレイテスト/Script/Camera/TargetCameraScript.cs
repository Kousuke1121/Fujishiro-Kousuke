using UnityEngine;

public class TargetCameraScript : MonoBehaviour
{
    public GameObject target;
    public float distance;
    public float distanceY;

    [SerializeField] private float sensitivity_X = 1f;
    [SerializeField] private float sensitivity_Y = 1f;

    bool flag = false;

    public void SetFlag(bool f)
    {
        flag = f;
    }

    void Update()
    {
        if (!flag) { return; }
        TargetCamera();
        //MouseCamera();
        if (Input.GetMouseButtonDown(2))
        {
        }
    }

    private void MouseCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 newRotation = transform.localEulerAngles;
        newRotation.y += mouseX * sensitivity_X;
        newRotation.x += mouseY * -sensitivity_Y;
        transform.localEulerAngles = newRotation;
    }

    private void TargetCamera()
    {
        if (target == null) { return; }
        Vector3 p = target.transform.position;
        transform.position = p + target.transform.forward * distance * -1 + Vector3.up * distanceY;
        p.y = 1f;
        transform.LookAt(p);
    }
}
