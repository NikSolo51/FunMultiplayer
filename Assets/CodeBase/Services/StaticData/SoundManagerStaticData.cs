using CodeBase.SoundManager;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Services.StaticData
{
    [CreateAssetMenu(fileName = "SoundManagerData", menuName = "StaticData/SoundManager", order = 0)]
    public class SoundManagerStaticData : ScriptableObject
    {
        public int id; 
        public SoundManagerType SoundManagerType;
        public AssetReferenceGameObject SoundManager;
    }
}