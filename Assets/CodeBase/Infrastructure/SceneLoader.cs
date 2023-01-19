using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class SceneLoader
    {
        private ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string name, Action onLoaded = null)
        {
            _coroutineRunner.StartCoroutine(LoadScene(name,onLoaded));
        }
        
        public IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                yield break;
            }

            //AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);
            PhotonNetwork.LoadLevel(nextScene);
            onLoaded?.Invoke();
        }
    }
}