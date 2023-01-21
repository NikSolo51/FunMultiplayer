# KillBox
Killbox is a multiplayer pet project inspired by the map of the same name from Half Life 2 Deathmatch.

Using as tech stack: __C#, Unity, Zenject, Odin, JahroConsole, Addressables__.

Multiplayer framework: __Photon__.
------------------
# My solution for combining addresables and zenject
Addresables takes care of loading assets, and photon instantiates them.

```
        public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
        {
            GameObject prefab = _asset.LoadSynchronously<GameObject>(prefabId);

            if (rotation == Quaternion.identity) rotation = prefab.transform.rotation;

            GameObject gameObject = InstantiateRegistered(prefab, position, rotation);
            return gameObject;
        }

        public void Destroy(GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
        }
```
Replacing the standard instantiate with a custom one is possible thanks to IPunPrefabPool:

```
class GameFactory : IPunPrefabPool
```
And redefining the standard instantiate to custom

```
PhotonNetwork.PrefabPool = GameFactory : IPunPrefabPool;
```
