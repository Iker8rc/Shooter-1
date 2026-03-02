using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject panelLoading;
    [SerializeField]
    private GameObject panelRoom;
    [SerializeField]
    private GameObject panelMulti;
    [SerializeField]
    private TextMeshProUGUI users;
    [SerializeField]
    private int maxPlayers;
    [SerializeField]
    private TMP_InputField inputNickName;
    [SerializeField]
    private bool conectado = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Update()
    {
        Debug.Log(PhotonNetwork.NetworkClientState);
    }
    public void MultiplayerButton()
    {
        panelLoading.SetActive(true);
        PhotonNetwork.NickName = inputNickName.text;
        PhotonNetwork.ConnectUsingSettings(); //Para conectarnos al server de photon
        conectado = false;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        if (conectado == false)
        {
            PhotonNetwork.JoinRandomRoom(); //Para unirnos a una sala random 
            Debug.Log("Estoy ejecutando el master");
            conectado = true;   
            //PhotonNetwork.JoinRoom("nombre room");
        } 
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
    public void MaxPlayers(int _maxPlayers)
    {   
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Ya conectado");
            conectado = false;
            OnConnectedToMaster();
        }
        maxPlayers = _maxPlayers;
        panelLoading.SetActive(true);
        PhotonNetwork.NickName = inputNickName.text;
        PhotonNetwork.ConnectUsingSettings();
        
        Debug.Log("Llego aquí");
        conectado = false;      
    }
    public void Multiplayer()
    {
        //Elegir num de jugadores 
        panelMulti.SetActive(true);
    }
    public void SoloPlayer()
    {
        panelLoading.SetActive(true);
        SceneManager.LoadScene("SoloLevel"); 
    }
}
