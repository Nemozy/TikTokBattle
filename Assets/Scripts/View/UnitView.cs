using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace View
{
    public class UnitView
    {
        public int Id { get;}
        private readonly Transform _transform;

        public UnitView(Transform transform, int id)
        {
            _transform = transform;
            Id = id;
        }
        
        public void SetPosition(int x, int y)
        {
            _transform.localPosition = new Vector3(x, _transform.localPosition.y, y);          
        }

        public void Destroy()
        {
            UnityObject.Destroy(_transform.gameObject);
        }
    }
}