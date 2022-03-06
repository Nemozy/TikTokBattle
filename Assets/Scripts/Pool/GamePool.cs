using UnityEngine;

namespace Pool
{
    public static class GamePool
    {
        public static Pool Pool { get; private set; }
        private static bool _initialized;
        
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            if (_initialized)
            {
                return;
            }
            _initialized = true;
            Pool = new Pool();
        }
    }
}