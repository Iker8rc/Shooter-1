using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject panelLoading;
    [SerializeField]
    private GameObject panelRoom;
    [SerializeField]
    private TextMeshProUGUI users;
    [SerializeField]
    private int maxPlayers;
    [SerializeField]
    private TMP_InputField inputNickName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void MultiplayerButton()
    {
        panelLoading.SetActive(true);
        PhotonNetwork.NickName = inputNickName.text;
        PhotonNetwork.ConnectUsingSettings(); //Para conectarnos al server de photon
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom(); //Para unirnos a una sala random 

        //PhotonNetwork.JoinRoom("nombre room");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        
    }
    public override void OnJoinRandomFailed(short returnCode, string message) //Cuando da fallo al unirnos a una sala random
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayers }); //Para crear sala
    }
    public override void OnJoinedRoom()
    {
        //Cuando estas dentro sala
        panelLoading.SetActive(false);

        panelRoom.SetActive(true);

        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            users.text += player.Value.NickName + "\n\n";
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        users.text += newPlayer.NickName + "\n\n";
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        users.text = "";
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            users.text += player.Value.NickName + "\n\n";
        }
    }
    public void ButtonReady()
    {
        Hashtable ready = new Hashtable
        {
            {"Ready", true }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(ready);
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Ready") == true)
        {
            CheckAllUsersReady();
        }
    }
    private void CheckAllUsersReady()
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount != maxPlayers)
        { 
            return; 
        }

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            if(player.Value.CustomProperties.ContainsKey("Ready") == false)
            {
                return;
            }
        }
        PhotonNetwork.LoadLevel("MultiplayerLevel");
    }
}
