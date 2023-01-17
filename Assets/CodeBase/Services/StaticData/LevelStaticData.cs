using CodeBase.Services.Audio.SoundManager;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public bool InitGameWorld = true;
        public string LevelKey;
      
        public SoundManagerData SoundManagerData;

        public Vector3 InitialHostPosition;
        public Vector3 InitialOthePlayerPosition;
    }
}