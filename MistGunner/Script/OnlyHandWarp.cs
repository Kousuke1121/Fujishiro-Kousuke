using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnlyHandWarp : MonoBehaviour
{
    public struct Hand
    {
        public int hit_Num;

        public GameObject handObject;
        public GameObject line;
        public LineRenderer lr;

        public RaycastHit hit;
        public Vector3 pos;
    }

    public Hand hand;

    [SerializeField] GameObject OVRcamera; //OVRcameraRig
    [SerializeField] public GameObject Lefthand;   //����
    [SerializeField] public GameObject Righthand;  //�E��

    [SerializeField]
    public Transform[] warpPoint;//�Ȑ��̒��ԓ_

    [SerializeField] float WarpbeforeSec = 0.1f;  //���[�v�O�̑ҋ@����
    [SerializeField] float WarpAfterSec = 0.1f;   //���[�v��̑ҋ@����
    [System.NonSerialized] public bool controllerCheck = true;
    bool objectCheck = false;
    bool warpFlag = false;

    [SerializeField] Material warpTrueColor;    //���[�v�\�Ȏ���Material
    [SerializeField] Material warpFalseColor;   //���[�v�s�\�Ȏ���Material


    [SerializeField] float angle = 30f;



    Vector3 hitPoint;//Line���������Ă�����W
    Vector3 WarpPos;//���[�v������W
    GameObject HitGameObject;
    [SerializeField] public string TagName = "";   //���[�v�o����Tag

    [SerializeField] public GameObject hpObject;
    bool hpObjectCheck = false;

    [SerializeField]
    float MaxScale;
    [SerializeField]
    float MinScale;

    float Scale;

    [SerializeField]
    float scalediv = 10f;

    [SerializeField] float Min_X;
    [SerializeField] float Max_X;
    [SerializeField] float Min_Y;
    [SerializeField] float Min_Z;
    [SerializeField] float Max_Z;

    int mask = 9 << 10;

    [SerializeField] public GameObject warpParticle;

    float present_Location;
    [SerializeField] float speed = 1f;
    private float warpDistance;
    [System.NonSerialized] public bool warpCheck = false;

    [SerializeField] float warpDis = 0.01f;

    void Start()
    {
        Line();
    }

    void Update()
    {
        DebagPos();
        Controller();
        Linecast();
        ChangeDirection();
        //LineScaleChange();

        if(warpCheck == true)
        {
            warpFlag = false;
            WarpAction();
        }
    }

    public void NewHand()
    {
        hand = new Hand();
        hand.hit_Num = 6;
    }

    /// <summary>
    /// �R���g���[���[���o�͂��i��֐�
    /// </summary>
    void Controller()
    {
        if (controllerCheck)
        {
            if (warpFlag)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
                {
                    On_WarpButton();
                }
            }
        }
        else
        {
            if (warpFlag)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                {
                    On_WarpButton();
                }
            }
        }
    }

    /// <summary>
    /// ���[�v�̃{�^���������ꂽ�Ƃ����ɌĂ΂��֐�
    /// </summary>
    void On_WarpButton()
    {
        WarpPos = hitPoint;
        hpObjectCheck = true;
        hand.line.SetActive(false);
        warpDistance = Vector3.Distance(OVRcamera.transform.position, hitPoint);
        StartCoroutine(Delay(WarpbeforeSec, () =>
        {
            Warp();
        }));
    }

    /// <summary>
    /// �v���C���[����]������֐�
    /// </summary>
    void ChangeDirection()
    {
        if (controllerCheck)
        {
            if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickLeft))
            {
                OVRcamera.transform.Rotate(0, -angle, 0);
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickRight))
            {
                OVRcamera.transform.Rotate(0, angle, 0);
            }
        }
        else
        {
            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft))
            {
                OVRcamera.transform.Rotate(0, -angle, 0);
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight))
            {
                OVRcamera.transform.Rotate(0, angle, 0);
            }
        }

    }

    /// <summary>
    /// ���C���̒�����ύX����֐�
    /// </summary>
    void LineScaleChange()
    {
        if (controllerCheck)
        {
            Vector2 stickL = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
            Scale = Mathf.Clamp(Scale += stickL.y / scalediv, MinScale, MaxScale);
            warpPoint[1].localScale = new Vector3(Scale, Scale, Scale);
        }
        else
        {
            Vector2 stickR = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
            Scale = Mathf.Clamp(Scale += stickR.y / scalediv, MinScale, MaxScale);
            warpPoint[1].localScale = new Vector3(Scale, Scale, Scale);
        }
    }

    /// <summary>
    /// LineRenderer�ݒ�֐�
    /// </summary>
    void Line()
    {
        hand.line = new GameObject("Line");

        hand.line.transform.parent = hand.handObject.transform;
        hand.lr = hand.line.AddComponent<LineRenderer>();
        hand.lr.receiveShadows = false;
        hand.lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        hand.lr.loop = false;
        hand.lr.positionCount = 20;

        hand.lr.startWidth = 0.1f;
        hand.lr.endWidth = 0.1f;
        hand.lr.startColor = Color.green;
        hand.lr.endColor = Color.green;
        hand.lr.material = warpFalseColor;
    }

    /// <summary>
    /// �t�@���N�V�����L�[�������ċȐ���ύX
    /// </summary>
    void DebagPos()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            hand.hit_Num = 1;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            hand.hit_Num = 2;
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            hand.hit_Num = 3;
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            hand.hit_Num = 4;
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            hand.hit_Num = 5;
        }

    }
    
    /// <summary>
    /// ���������ꏊ�ŋȐ���Ԃ�
    /// </summary>
    void Linecast()
    {
        if (Physics.Linecast(warpPoint[0].position, warpPoint[1].position, out hand.hit,mask))
        {
            hand.hit_Num = 1;
            Ray(warpPoint[0].position);
            objectCheck = true;
        }
        else if (Physics.Linecast(warpPoint[1].position, warpPoint[6].position, out hand.hit, mask))
        {
            hand.hit_Num = 2;
            Ray(warpPoint[0].position);
            objectCheck = true;
        }
        else if (Physics.Linecast(warpPoint[6].position, warpPoint[7].position, out hand.hit, mask))
        {
            hand.hit_Num = 3;
            Ray(warpPoint[0].position);
            objectCheck = true;
        }
        else if (Physics.Linecast(warpPoint[7].position, warpPoint[8].position, out hand.hit, mask))
        {
            hand.hit_Num = 4;
            Ray(warpPoint[0].position);
            objectCheck = true;
        }
        else if (Physics.Linecast(warpPoint[8].position, warpPoint[5].position, out hand.hit, mask))
        {
            hand.hit_Num = 5;
            Ray(warpPoint[0].position);
            objectCheck = true;
        }
        else
        {
            hand.hit_Num = 6;
            objectCheck = false;
            warpFlag = false;
            warpParticle.SetActive(false);
            hpObject.SetActive(false);
            hand.lr.material = warpFalseColor;
            //Debug.Log(objectCheck);
        }
        //Debug.Log(hand.hit_Num);
        DrawLine(hand.lr, warpPoint, hand.pos);
    }

    /// <summary>
    /// �Ȑ���`�悷��֐�
    /// </summary>
    void DrawLine(LineRenderer line, Transform[] point, Vector3 hit)
    {
        float t = 0.0f;

        for (int i = 0; i < 20; i++)
        {
            t += 0.05f;
            switch (hand.hit_Num)
            {
                case 1:
                    Vector3 hit_1 = Vector3.Lerp(point[0].position, hitPoint, t);
                    line.SetPosition(i, hit_1);
                    //Debug.Log("1");
                    break;
                case 2:
                    Vector3 hit_2 = GetPoint_2(point[0].position, point[1].position, hitPoint, t);
                    line.SetPosition(i, hit_2);
                    //Debug.Log("2");
                    break;
                case 3:
                    Vector3 hit_3 = GetPoint_3(point[0].position, point[1].position, point[2].position, hitPoint, t);
                    line.SetPosition(i, hit_3);
                    //Debug.Log("3");
                    break;
                case 4:
                    Vector3 hit_4 = GetPoint_4(point[0].position, point[1].position, point[2].position, point[7].position, hitPoint, t);
                    line.SetPosition(i, hit_4);
                    //Debug.Log("4");
                    break;
                case 5:
                    Vector3 hit_5 = GetPoint_5(point[0].position, point[1].position, point[2].position, point[3].position, point[4].position, hitPoint, t);
                    line.SetPosition(i, hit_5);
                    //Debug.Log("5");
                    break;
                case 6:
                    Vector3 hit_6 = GetPoint_5(point[0].position, point[1].position, point[2].position, point[3].position, point[4].position, point[5].position, t);
                    line.SetPosition(i, hit_6);
                    break;
                default:
                    //Debug.Log("�E�E�E�E");
                    break;

            }


        }
    }

    /// <summary>
    /// RaycastHit������֐�
    /// </summary>
    /// <param name="_tmp3"></param>
    void Ray(Vector3 _tmp3)
    {
        hitPoint = hand.hit.point;
        HitGameObject = hand.hit.collider.gameObject;

        objectCheck =  Angle(hand.hit);

        if(HitGameObject.tag != TagName)
        {
            objectCheck = false;
        }

        if (objectCheck)
        {
            hand.lr.material = warpTrueColor;
            warpParticle.SetActive(true);
            hpObject.SetActive(true);
            if (!hpObjectCheck)
            {
                hpObject.transform.position = hitPoint;
                warpParticle.transform.position = hitPoint;
                if(hand.hit.normal != null || hand.hit.normal != Vector3.zero)
                {
                    hpObject.transform.rotation = Quaternion.LookRotation(hand.hit.normal, Vector3.up);
                    warpParticle.transform.rotation = Quaternion.LookRotation(hand.hit.normal, Vector3.up);
                }
            }
            warpFlag = true;
        }
        else
        {
            //Debug.Log("else");
            hand.lr.material = warpFalseColor;
            warpParticle.SetActive(false);
            hpObject.SetActive(false);
            warpFlag = false;

        }

        if (hpObjectCheck)
        {
            hpObject.transform.position = WarpPos;
            warpParticle.transform.position = WarpPos;
        }
    }


    void Warp()
    {
        if (hand.line.activeSelf == false)
        {
            warpCheck = true;
            warpFlag = false;
        }
    }

    /// <summary>
    /// ���[�v������֐�
    /// </summary>
    void WarpAction()
    {

        //Debug.Log("�X�^�[�g�n�_�F" + OVRcamera.transform.position + "���[�v�n�_�F" + WarpPos);
        float distance = Vector3.Distance(OVRcamera.transform.position, WarpPos);
        present_Location = (5 * speed) / warpDistance;
        present_Location += 0.1f;
        OVRcamera.transform.position = Vector3.Lerp(OVRcamera.transform.position, WarpPos, present_Location);
        if(distance <= warpDis && distance >= 0)
        {            
            StartCoroutine(Delay(WarpAfterSec, () =>
            {
                LineActive();
            }));
        }
    }

    /// <summary>
    /// ���[�v��Ƀ��C����`�悷��֐�
    /// </summary>
    void LineActive()
    {
        hand.line.SetActive(true);
        warpCheck = false;
        hpObjectCheck = false;
    }

    /// <summary>
    /// ���[�v��̊p�x������֐�
    /// </summary>
    /// <param name="hit"></param>
    /// <returns></returns>
    private bool Angle(RaycastHit hit)
    {
        if(hit.normal.x > Min_X && hit.normal.x <Max_X && hit.normal.y > Min_Y && hit.normal.z >= Min_Z && hit.normal.z < Max_Z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// ���b��Ɏ��s����֐�
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator Delay(float seconds, UnityAction action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
    /// <summary>
    /// �񎟃x�W�F�Ȑ�
    /// </summary>
    Vector3 GetPoint_2(Vector3 p0, Vector3 p1,Vector3 p2,float t)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);

        Vector3 c = Vector3.Lerp(a, b, t);

        return c;
    }

    /// <summary>
    /// �O���x�W�F�Ȑ�
    /// </summary>
    Vector3 GetPoint_3(Vector3 p0,Vector3 p1,Vector3 p2,Vector3 p3,float t)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        Vector3 f = Vector3.Lerp(d, e, t);

        return f;
    }

    /// <summary>
    /// �l���x�W�F�Ȑ�
    /// </summary>
    Vector3 GetPoint_4(Vector3 p0,Vector3 p1,Vector3 p2,Vector3 p3,Vector3 p4,float t)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);
        Vector3 d = Vector3.Lerp(p3, p4, t);

        Vector3 e = Vector3.Lerp(a, b, t);
        Vector3 f = Vector3.Lerp(b, c, t);
        Vector3 g = Vector3.Lerp(c, d, t);

        Vector3 h = Vector3.Lerp(e, f, t);
        Vector3 i = Vector3.Lerp(f, g, t);

        Vector3 j = Vector3.Lerp(h, i, t);

        return j;
    }

    /// <summary>
    /// �܎��x�W�F�Ȑ�
    /// </summary>
    Vector3 GetPoint_5(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Vector3 p5,float t)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);
        Vector3 d = Vector3.Lerp(p3, p4, t);
        Vector3 e = Vector3.Lerp(p4, p5, t);

        Vector3 f = Vector3.Lerp(a, b, t);
        Vector3 g = Vector3.Lerp(b, c, t);
        Vector3 h = Vector3.Lerp(c, d, t);
        Vector3 i = Vector3.Lerp(d, e, t);

        Vector3 j = Vector3.Lerp(f, g, t);
        Vector3 k = Vector3.Lerp(g, h, t);
        Vector3 l = Vector3.Lerp(h, i, t);

        Vector3 m = Vector3.Lerp(j, k, t);
        Vector3 n = Vector3.Lerp(k, l, t);

        Vector3 o = Vector3.Lerp(m, n, t);

        return o;
    }
}
