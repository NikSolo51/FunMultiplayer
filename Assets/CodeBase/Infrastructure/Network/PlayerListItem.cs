using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CodeBase.Infrastructure.Network
{
    public class PlayerListItem : MonoBehaviourPunCallbacks
    {
        [Required]
        [SerializeField] private TMP_Text _text;
        private Player _player;

        public void Constructor(Player player)
        {
            _player = player;
            _text.text = player.NickName;
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (_player == otherPlayer)
                Destroy(gameObject);
        }

        public override void OnLeftRoom()
        {
            Destroy(gameObject);
        }
    }
}