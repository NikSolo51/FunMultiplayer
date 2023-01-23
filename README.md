# KillBox
Killbox is a multiplayer pet project inspired by the map of the same name from Half Life 2 Deathmatch.

Using as tech stack: __C#, Unity, Addressables, JahroConsole, Odin, Zenject__.

Multiplayer framework: __[Photon](https://www.photonengine.com/)__.
# Download my game
__Itch io: [KillBox](https://nik-salazar.itch.io/killbox)__
_______________
# Photon
The [Photon](https://www.photonengine.com/) is a game engine specializing in multiplayer game development. It’s a series of products, software, technology, and networking components that bring great speed, performance, and more to online play.
_______________
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
______________
# Addressable
The [Addressable Asset System](https://docs.unity3d.com/Manual/com.unity.addressables.html) (i.e., [Addressables](https://docs.unity3d.com/Manual/com.unity.addressables.html)) is a Unity Editor and runtime asset management system that improves support for large production teams with complex live content delivery needs. The system uses asynchronous loading to support loading from any location with any collection of dependencies

![45WFRKL2_bNapUxyapBag08NhT-x236lQJAC5vxbVklbPzSV3CpkxjWgm8rRigsX8hNNl-9JsuvTrk4CXAcly4e2](https://user-images.githubusercontent.com/50959880/213870539-f5baa988-50b3-4d20-8331-e05f05a4aa6c.jpg)
_______________
# JahroConsole
[Jahro](https://assetstore.unity.com/packages/tools/utilities/jahro-console-173608) is a simple debugging plugin for Unity projects. Jahro visual interface allows you to pull down the console, select double-speed, and test the scene in real time.

![image](https://user-images.githubusercontent.com/50959880/213870501-6a006aab-9902-4bbe-b102-74a6bf988886.png)
_______________
# Odin
[Odin](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041) puts your Unity workflow on steroids, making it easy to build powerful and advanced user-friendly editors for you and your entire team.

![image](https://user-images.githubusercontent.com/50959880/213870563-14b76dc5-13f0-4f77-9bcc-a701d9ef43a1.png)
______________
# Zenject
[Zenject](https://github.com/modesttree/Zenject) is a lightweight highly performant dependency injection framework built specifically to target Unity 3D.

![image](https://user-images.githubusercontent.com/50959880/213870635-5aa95b67-5d9d-4456-835a-817c8313ce31.png)

