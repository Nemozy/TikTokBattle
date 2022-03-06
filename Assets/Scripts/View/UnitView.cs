using System.Linq;
using Pool;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace View
{
    public class UnitView : BaseView
    {
        public int Id { get; private set; }
        
        private UnitMaterial _unitMaterial;

        public void Connect(int id, PoolObjectType unitType)
        {
            base.Connect(unitType);
            
            Id = id;
            var renderers = transform.GetComponentsInChildren<Renderer>(true).ToList();
            _unitMaterial = new UnitMaterial(renderers);
        }

        public void SetTeamColor(Color teamColor)
        {
            _unitMaterial.SetTeamColor(teamColor);
        }
        
        public void SetPosition(int x, int y)
        {
            transform.localPosition = new Vector3(x, transform.localPosition.y, y);          
        }

        public void PlayDie()
        {
            Release();
        }

        public void Release()
        {
            GamePool.Pool.Release(this);
        }
    }
}