using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldManager : MonoBehaviour
    {   
        [SerializeField]
        private bool isDedicatedServer = false;
        [SerializeField]
        private bool isClient = false;
        [SerializeField]
        private bool enableGUI = true;
        void Start() {
            if (isDedicatedServer)
                NetworkManager.Singleton.StartServer();
            if (isClient)
                NetworkManager.Singleton.StartClient();
            
        }
        void OnGUI()
        {
            if (!enableGUI) return;
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
                DisconnectButton();
            }

            GUILayout.EndArea();
        }


        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
            if (GUILayout.Button("Exit")) Application.Quit();
        }
        static void DisconnectButton()
        {
            if (GUILayout.Button("Disconnect")) NetworkManager.Singleton.Shutdown();
        }
        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }
    }
}


