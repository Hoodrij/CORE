#if ADDRESSABLES
using System.Threading.Tasks;
using UnityAsync;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Units
{
    public class AddressableAssets : Unit, IAssets
    {
        private static string LOG_TAG = "[Addressables]".Color(Color.red);
        
        public async Task<Object> Load(string path)
        {
            return await CheckExistenceAndLoad<Object>(path);
        }
        
        public async Task<T> Load<T>(string path) where T : Object
        {
            if (typeof(T).IsSubclassOf(typeof(Component)))
            { 
                GameObject gameObject = await CheckExistenceAndLoad<GameObject>(path);
                return gameObject.GetComponent<T>();
            }
        
            return await CheckExistenceAndLoad<T>(path);
        }

        private async Task<T> CheckExistenceAndLoad<T>(string path) where T : Object
        {
            AsyncOperationHandle<T> ao = Addressables.LoadAssetAsync<T>(path);
            await Wait.While(() => ao.Status == AsyncOperationStatus.None);
            if (ao.Status == AsyncOperationStatus.Failed)
            {
                $"{LOG_TAG} Path {path} does not exist".log();
                return null;
            }
            return await ao.Task;
        } 
    }
}
#endif