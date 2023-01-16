using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace CodeBase.Infrastructure.Network
{
    public class RoomListItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private RoomInfo _info;
        private NetworkLauncher _networkLauncher;
        
        public void Constructor(RoomInfo info,NetworkLauncher networkLauncher)
        {
            _info = info;
            _text.text = _info.Name;
            _networkLauncher = networkLauncher;
        }

        public void OnClick()
        {
            _networkLauncher.JoinRoom(_info);
        }
    }
}