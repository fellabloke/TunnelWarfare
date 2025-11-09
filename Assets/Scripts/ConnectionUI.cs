using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class ConnectionUI : MonoBehaviour
{
    [SerializeField] private Button _clientButton;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _serverButton;
    void Start()
    {
        if (_clientButton)
        {
            _clientButton.onClick.AddListener(() =>
            {
                Debug.Log("Starting Client");
                NetworkManager.Singleton.StartClient();
                gameObject.SetActive(false);
            });
        }
        if (_hostButton)
        {
            _hostButton.onClick.AddListener(() =>
            {
                Debug.Log("Starting Host");
                NetworkManager.Singleton.StartHost();
                gameObject.SetActive(false);
            });
        }
        if(_serverButton)
        {
            _serverButton.onClick.AddListener(() =>
            {
                Debug.Log("Starting Server");
                NetworkManager.Singleton.StartServer();
                gameObject.SetActive(false);
            });
        }       
    }
}
