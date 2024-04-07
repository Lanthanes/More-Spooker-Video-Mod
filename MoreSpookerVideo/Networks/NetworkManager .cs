using Photon.Pun;
using System.Linq;

namespace MoreSpookerVideo.Networks
{
    internal class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static NetworkManager? Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                MoreSpookerVideo.Logger?.LogWarning("Already NetworkManager of MoreSpookerVideo MOD in game...");
                Destroy(gameObject);
            }
        }

        void Start()
        {
            if (PhotonNetwork.IsConnected)
            {
                MoreSpookerVideo.Logger?.LogInfo("Connected on Photon!");
            }
            else
            {
                MoreSpookerVideo.Logger?.LogWarning("Connection to Photon...");
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            MoreSpookerVideo.Logger?.LogInfo("Connected on Photon OK!");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            if (PhotonNetwork.IsMasterClient)
            {
                MoreSpookerVideo.AllItems.ForEach(item =>
                {
                    this.SendNewItemDataToClient(item);
                });
            }
        }

        public void SendNewItemDataToClient(Item item)
        {
            if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
            {
                photonView?.RPC("RPCA_MoreSpookerVideo_UpdateItem", RpcTarget.Others, item.id, item.displayName, item.id, item.Category, item.price, item.purchasable);
            }
        }

        [PunRPC]
        public void RPCA_MoreSpookerVideo_UpdateItem(byte itemId, string itemName, byte itemIdIcon, ShopItemCategory itemCategory, int itemPrice, bool itemPurchaseable)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                MoreSpookerVideo.Logger?.LogWarning("Server got this call, not supported!");
                return;
            }

            Item? iconItem = MoreSpookerVideo.AllItems.FirstOrDefault(item => item.id.Equals(itemIdIcon));
            Item? updateItem = MoreSpookerVideo.AllItems.FirstOrDefault(i => i.id.Equals(itemId));

            if (updateItem)
            {
                updateItem.displayName = itemName;
                updateItem.price = itemPrice;
                updateItem.purchasable = itemPurchaseable;
                updateItem.Category = itemCategory;

                if (iconItem)
                {
                    updateItem.icon = iconItem.icon;
                }

                MoreSpookerVideo.Logger?.LogInfo($"Item {updateItem.displayName} has unlocked! ({updateItem.Category})");
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            MoreSpookerVideo.Logger?.LogInfo("NetworkManager of MoreSpookerVideo MOD is enabled...");
        }

        public override void OnDisable()
        {
            base.OnDisable();
            MoreSpookerVideo.Logger?.LogWarning("NetworkManager of MoreSpookerVideo MOD is diasabled...");
        }
    }
}
