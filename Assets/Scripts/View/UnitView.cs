using System.Linq;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace View
{
    public class UnitView
    {
        public int Id { get;}
        
        private readonly Transform _transform;
        private readonly UnitMaterial _unitMaterial;

        public UnitView(Transform transform, int id)
        {
            _transform = transform;
            _transform.gameObject.SetActive(true);
            Id = id;
            var renderers = _transform.GetComponentsInChildren<Renderer>(true).ToList();
            _unitMaterial = new UnitMaterial(renderers);
        }

        public void SetTeamColor(Color teamColor)
        {
            _unitMaterial.SetTeamColor(teamColor);
        }
        
        public void SetPosition(int x, int y)
        {
            _transform.localPosition = new Vector3(x, _transform.localPosition.y, y);          
        }

        public void PlayDie()
        {
            _transform.gameObject.SetActive(false);
        }
        
        public void Destroy()
        {
            UnityObject.Destroy(_transform.gameObject);
        }
    }
}