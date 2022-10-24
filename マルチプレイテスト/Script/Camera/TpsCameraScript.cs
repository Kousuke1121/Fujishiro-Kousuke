using UnityEngine;

public class TpsCameraScript : MonoBehaviour
{

    public Transform pivot = null;

    void Start()
    {

        if(pivot == null)
        {
            pivot = transform.parent;
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    [Range(-0.999f, -0.5f)]
    public float minYAngle = -0.5f;
    [Range(0.5f, 0.999f)]
    public float maxYAngle = -0.5f; 

    void Update()
    {
        //�}�E�X��X,Y�����ǂ�قǈړ����������擾���܂�
        float X_Rotation = Input.GetAxis("Mouse X");
        float Y_Rotation = Input.GetAxis("Mouse Y");
        //Y�����X�V���܂��i�L�����N�^�[����]�j�擾����X���̕ύX���L�����N�^�[��Y���ɔ��f���܂�
        pivot.transform.Rotate(0, X_Rotation, 0);

        //����Y���̐ݒ�ł��B
        float nowAngle = pivot.transform.localRotation.x;
        //�ő�l�A�܂��͍ŏ��l�𒴂����ꍇ�A�J����������ȏ㓮���Ȃ��p�ɂ��Ă��܂��B
        //�J���������]���Ȃ��悤�ɂ���̂�h���܂��B
        if (-Y_Rotation != 0)
        {
            if (0 < Y_Rotation)
            {
                if (minYAngle <= nowAngle)
                {
                    pivot.transform.Rotate(Y_Rotation, 0, 0);
                }
                else
                {
                    pivot.transform.Rotate(minYAngle, 0, 0);
                }
            }
            else
            {
                if (nowAngle <= maxYAngle)
                {
                    pivot.transform.Rotate(Y_Rotation, 0, 0);
                }
                else
                {
                    pivot.transform.Rotate(maxYAngle, 0, 0);
                }
            }
        }
        //���삵�Ă���ƁAZ�������񂾂񓮂��Ă����̂ŁA0�ɐݒ肵�Ă��������B
        pivot.eulerAngles = new Vector3(pivot.eulerAngles.x, pivot.eulerAngles.y, 0f);

        if (Input.GetKeyDown(KeyCode.F1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
