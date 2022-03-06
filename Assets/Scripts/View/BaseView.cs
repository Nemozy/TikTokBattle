using Pool;
using UnityEngine;

namespace View
{
    public abstract class BaseView : MonoBehaviour
    {
        public PoolObjectType ViewType { get; private set; }
        
        public virtual void Connect(PoolObjectType type)
        {
            ViewType = type;
        }
    }
}