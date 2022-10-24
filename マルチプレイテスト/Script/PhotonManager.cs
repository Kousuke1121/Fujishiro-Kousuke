using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    enum Mode
    {
        Player,
        Spectator,
    }

    Mode mode;

    GameObject player;
    GameObject enemy;
    Player enemyPlayer;
    Vector3 PlayerPos;
    bool flag = false;


    private GameObject MenuCanvas;
    public Text nickname;
    public Text message;

    public GameObject PlayerName;
    public GameObject PlayerList;

    public bool IsFlag()
    {
        return flag;
    }

    public void SetFlag(bool f)
    {
        flag = f;
    }

    public void SetMessage(string s)
    {
        message.text = s;
    }

    public void SetPlayer(GameObject ob)
    {
        player = ob;
    }

    public void SetEnemy(GameObject ob)
    {
        enemy = ob;
    }

    public GameObject GetEnemy()
    {
        return enemy;
    }

    public Player GetEnemyPLayer()
    {
        return enemyPlayer;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        MenuCanvas = GameObject.Find("MenuCanvas");
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        if (PhotonNetwork.LocalPlayer.NickName == "")
        {
            ChengePanel(GameObject.Find("LoadPanel"), MenuCanvas.transform.Find("NamePanel").gameObject);
        }
        else
        {
            ChengePanel(GameObject.Find("LoadPanel"), MenuCanvas.transform.Find("TitelPanel").gameObject);
        }
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("ロビーに入りました。");
    }

    public void OnNickName()
    {
        InputField field = GameObject.Find("NickNameInput").GetComponent<InputField>();
        nickname.text = field.text;
        PhotonNetwork.LocalPlayer.NickName = nickname.text;
        ChengePanel(MenuCanvas.transform.Find("NamePanel").gameObject, MenuCanvas.transform.Find("TitelPanel").gameObject);
    }

    private void ChengePanel(GameObject beforeob,GameObject afterob)
    {
        beforeob.SetActive(false);
        afterob.SetActive(true);
    }

    public void OnRoomCreate()
    {
        RoomOptions opt = new RoomOptions();
        opt.MaxPlayers = 3;
        PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName+"の部屋", opt, TypedLobby.Default);
    }

    public void OnRoomSearch()
    {
        ChengePanel(GameObject.Find("TitelPanel"), MenuCanvas.transform.Find("RoomSearchPanel").gameObject);
    }

    public void OnRename()
    {
        ChengePanel(GameObject.Find("TitelPanel"), MenuCanvas.transform.Find("NamePanel").gameObject);
    }

    public void OnBack()
    {
        ChengePanel(GameObject.Find("RoomSearchPanel"), MenuCanvas.transform.Find("TitelPanel").gameObject);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        GameObject roomPanel = GameObject.Find("RoomPanel");
        roomPanel.SetActive(false);
        Text Message = GameObject.Find("Message").GetComponent<Text>();
        Message.enabled = true;
        StartCoroutine(RoomSearchFailed(roomPanel, Message));
    }

    IEnumerator RoomSearchFailed(GameObject ob,Text text)
    {
        yield return new WaitForSeconds(1.5f);
        text.enabled = false;
        ob.SetActive(true);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer + "が入室しました。");

        NameList(newPlayer.NickName);
    }
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount <= 2)
        {
            flag = true;
            mode = Mode.Player;
            MenuCanvas.GetComponent<Canvas>().enabled = false;
            player = PhotonNetwork.Instantiate("unitychan", new Vector3(0, 100, 0), Quaternion.identity, 0);
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                PlayerPos = new Vector3(0, 0, -3);
                Debug.Log("マスターです。");
            }
            else
            {
                PlayerPos = new Vector3(0, 0, 3);
                Debug.Log("マスターじゃないです。");
            }
            player.transform.position = PlayerPos;

            

            GameObject.Find("GameCanvas").GetComponent<Canvas>().enabled = true;
            Camera.main.depth = 10;
            PlayerController PlayerController = player.GetComponent<PlayerController>();
            PlayerController.SetFlag(true);
            //TargetCameraScript Target = Camera.main.GetComponent<TargetCameraScript>();
            //Target.SetFlag(true);
            //Target.target = player;
            GameObject camera = Camera.main.gameObject;
            camera.transform.parent = player.transform.Find("pivot").transform;
            camera.transform.localPosition = new Vector3(0f, 0.1f, -5f);
            TpsCameraScript TPSCamera = Camera.main.GetComponent<TpsCameraScript>();
            TPSCamera.pivot = player.transform.Find("pivot").transform;
            TPSCamera.enabled = true;
        }
        else
        {
            flag = true;
            mode = Mode.Spectator;
            MenuCanvas.GetComponent<Canvas>().enabled = false;
            player = PhotonNetwork.Instantiate("Player", new Vector3(0, 10, -50), Quaternion.identity, 0);
            GameObject.Find("GameCanvas").GetComponent<Canvas>().enabled = true;
            Camera.main.depth = 10;
            FpsCameraScript FPSCamera = player.GetComponent<FpsCameraScript>();
            FPSCamera.SetFlag(true);
            GameObject camera = Camera.main.gameObject;
            camera.GetComponent<TargetCameraScript>().enabled = false;
            camera.transform.parent = player.transform;
            camera.transform.localPosition = new Vector3(0, 0, 0);
        }

        foreach (var p in PhotonNetwork.PlayerList)
        {
            Debug.Log(p.NickName);
            NameList(p.NickName);
            if (PhotonNetwork.LocalPlayer.UserId != p.UserId && enemyPlayer == null)
            {
                enemyPlayer = p;
            }
        }
    }


    private void Update()
    {
        if (!flag) { return; }
        if (Input.GetKey(KeyCode.Escape))
        {
            if (mode == Mode.Spectator)
            {
                Camera.main.transform.parent = null;
            }
            PhotonNetwork.Disconnect();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        for (int i = 0; i < PlayerList.transform.childCount; i++)
        {
            GameObject t = PlayerList.transform.GetChild(i).gameObject;
            if (t.GetComponent<Text>().text == otherPlayer.NickName)
            {
                Destroy(t);
                break;
            }
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        for (int i = 0; i < PlayerList.transform.childCount; i++)
        {
            GameObject t = PlayerList.transform.GetChild(i).gameObject;
            Destroy(t);
        }

        GameObject.Find("GameCanvas").GetComponent<Canvas>().enabled = false;
        
        Camera.main.depth = 0;
        foreach(Transform child in MenuCanvas.transform)
        {
            if (child.name == "LoadPanel")
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
        MenuCanvas.GetComponent<Canvas>().enabled = true;
        player.GetComponent<FpsCameraScript>().SetFlag(false);
        PhotonNetwork.ConnectUsingSettings();
    }
    private void NameList(string nickname)
    {
        GameObject playername = (GameObject)Instantiate(PlayerName);
        playername.transform.SetParent(PlayerList.transform, false);
        Text nametext = playername.GetComponent<Text>();
        nametext.text = nickname;
        nametext.color = new Color(0, 0, 255);
    }
    public void CheckEnemy()
    {
        foreach(GameObject ob in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(ob != player)
            {
                SetEnemy(ob);
                break;
            }
        }
    }
}
