using System;
using System.Threading.Tasks;
using View;

namespace Pool
{
    public class Pool
    {
        private PoolContainer<BaseView> _pool;
        
        public void Release(BaseView obj)
        {
            _pool.Release(obj);
        }

        public T Get<T>(PoolObjectType type) where T : BaseView
        {
            return Get<T>(type, _pool);
        }
        
        /*public void Destroy()
        {
            Clear();
            _pool = null;
        }*/
        
        private T Get<T>(PoolObjectType type, PoolContainer<BaseView> pool) where T : BaseView
        {
            if (pool == null)
            {
                _pool = CreatePoolContainer();
            }

            return _pool.Get<T>(type);
        }
        
        private PoolContainer<BaseView> CreatePoolContainer()
        {
            if (_pool != null)
            {
                throw new Exception("Pool already created.");
            }
            var poolContainer = new PoolContainer<BaseView>();
            poolContainer.CreatePool();
            _pool = poolContainer;
            
            return poolContainer;
        }

        private async Task CreatePoolContainerAsync()
        {
            var poolContainer = new PoolContainer<BaseView>();
            await poolContainer.CreatePoolAsync();
            
            _pool = poolContainer;
        }

        private void Clear()
        {
            _pool.Clear();
        }
    }
}