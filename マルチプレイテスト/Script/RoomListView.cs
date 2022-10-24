using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class RoomListView : MonoBehaviourPunCallbacks
{
    public GameObject RoomButon;

    public GameObject Content;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {        
        foreach(RoomInfo r in roomList)
        {
            if (r.PlayerCount > 0)
            {
                RoomButtonCreate(r);
            }
            else
            {
                RoomButtonDelete(r);
            }
        }
    }

    //ルームボタンの作成
    public void RoomButtonCreate(RoomInfo r)
    {
        //既に存在していたのなら情報を更新
        if (Content.transform.Find(r.Name))
        {
            RoomInfoUpdate(Content.transform.Find(r.Name).gameObject, r);
        }
        //新しく作られたルームならばボタンを作成
        else
        {
            GameObject roomButton = (GameObject)Instantiate(RoomButon);
            roomButton.transform.SetParent(Content.transform, false);
            RoomInfoUpdate(roomButton, r);
            //生成したボタンの名前を作成するルームの名前にする
            roomButton.name = r.Name;
            roomButton.GetComponent<Button>().onClick.AddListener(JoinRoom);
        }
    }

    //ルームボタンの削除
    public void RoomButtonDelete(RoomInfo r)
    {
        if (Content.transform.Find(r.Name))
        {
            GameObject.Destroy(Content.transform.Find(r.Name).gameObject);
        }
    }

    //ルームボタンのInfoの更新
    public void RoomInfoUpdate(GameObject button,RoomInfo info)
    {
        foreach(Text t in button.GetComponentsInChildren<Text>())
        {
            if(t.name == "RoomName")
            {
                t.text = info.Name;
            }else if(t.name == "MaxPlayer")
            {
                t.text = info.MaxPlayers.ToString();
            }else if(t.name == "RoomInPlayerCount")
            {
                t.text = info.PlayerCount.ToString();
            }
        }
    }

    public void JoinRoom()
    {
        EventSystem eventSystem = EventSystem.current;

        GameObject button = eventSystem.currentSelectedGameObject;

        string roomName = button.transform.Find("RoomName").GetComponent<Text>().text;

        PhotonNetwork.JoinRoom(roomName);
    }
}
