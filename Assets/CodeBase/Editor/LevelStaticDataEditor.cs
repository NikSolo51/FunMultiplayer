using CodeBase.Services.StaticData;
using CodeBase.SoundManager;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string InitialHostPointTag = "InitialHostPoint";
        private const string InitialOtherPlayerPointTag = "InitialOtherPlayerPoint";
        private const string InitialCameraPointTag = "CameraInitialPoint";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelStaticData levelData = (LevelStaticData) target;

            if (GUILayout.Button("Collect"))
            {
                levelData.LevelKey = SceneManager.GetActiveScene().name;
                SoundManagerMarker soundManagerMarker = FindObjectOfType<SoundManagerMarker>();
                levelData.SoundManagerData = new SoundManagerData(soundManagerMarker.sounds,soundManagerMarker.clips,
                    soundManagerMarker.soundManagerType);
                
                levelData.InitialHostPosition = GameObject.FindGameObjectWithTag(InitialHostPointTag).transform.position;
                levelData.InitialOthePlayerPosition = GameObject.FindGameObjectWithTag(InitialOtherPlayerPointTag).transform.position;
                Transform initialCameraPoint= GameObject.FindGameObjectWithTag(InitialCameraPointTag).transform;
                levelData.InitialCameraPosition = initialCameraPoint.position;
                levelData.InitialCameraRotation = initialCameraPoint.rotation;
            }

            EditorUtility.SetDirty(target);
        }
    }
}