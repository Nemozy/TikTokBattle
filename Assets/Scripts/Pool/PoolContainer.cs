using System.Threading.Tasks;
using StringEnum;
using UnityEngine;
using UnityEngine.AddressableAssets;
using View;
using Object = UnityEngine.Object;

namespace Pool
{
    public class PoolContainer<T> where T : BaseView
    {
        private DictionaryViewPool<T> _dictionaryPool;
        private Transform _poolPrefab;
        
        public T1 Get<T1> (PoolObjectType type) where T1 : BaseView
        {
            var obj = _dictionaryPool.Get(type);
            
            return obj as T1;
        }

        public void Release(T obj)
        {
            _dictionaryPool.Release(obj.ViewType, obj);
        }
        
        public void CreatePool()
        {
            var assetHandle = Addressables.LoadAssetAsync<GameObject>( StringEnumUtil.GetStringValue(LibraryCatalogNames.POOL_PREFAB));
            var go = assetHandle.WaitForCompletion();
            _poolPrefab = Object.Instantiate(go).transform;
            _dictionaryPool = CreateDictionaryViewPool();
        }
        
        public async Task CreatePoolAsync()
        {
            var assetHandle = Addressables.LoadAssetAsync<GameObject>(StringEnumUtil.GetStringValue(LibraryCatalogNames.POOL_PREFAB));
            await assetHandle.Task;

            if (assetHandle.OperationException != null)
            {
                throw assetHandle.OperationException;
            }
            var go = assetHandle.Result;
            _poolPrefab = Object.Instantiate(go).transform;
            _dictionaryPool = CreateDictionaryViewPool();
        }
        
        public void Dispose()
        {
            if (_poolPrefab != null && _dictionaryPool != null)
            {
                Clear();
                Addressables.Release(_poolPrefab);
            }
        }
        
        public void Clear()
        {
            _dictionaryPool.Clear();
        }

        private DictionaryViewPool<T> CreateDictionaryViewPool()
        {
            var objectPool = new DictionaryViewPool<T>(
                CreateObjectFunc,
                ActionOnGetObjectFunc,
                ActionOnReleaseObjectFunc,
                ActionOnDestroyObjectFunc);

            return objectPool;
        }

        private T CreateObjectFunc(PoolObjectType type)
        {
            var assetHandle = Addressables.LoadAssetAsync<GameObject>(StringEnumUtil.GetStringValue(type.ObjectType));
            var prefab = assetHandle.WaitForCompletion();
            var newObject = Object.Instantiate(prefab, _poolPrefab);
            var result = newObject.GetComponent<T>();
            result.Connect(type);
            newObject.SetActive(false);
            return result;
        }

        private void ActionOnGetObjectFunc(T obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void ActionOnReleaseObjectFunc(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_poolPrefab);
        }

        private void ActionOnDestroyObjectFunc(T obj)
        {
            Object.Destroy(obj.gameObject); 
        }
    }
}