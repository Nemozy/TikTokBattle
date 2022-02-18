using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace View
{
    public class UnitMaterial
    {
        private static readonly int ColorShaderPropertyId = Shader.PropertyToID("_Color");
        
        private readonly IReadOnlyCollection<Renderer> _renderers;
        private readonly List<Material> _materials = new ();
        
        public UnitMaterial(List<Renderer> renderers)
        {
            _renderers = renderers.Where(x => !(x is ParticleSystemRenderer)).ToList().AsReadOnly();foreach (var renderer in _renderers)
            {
                var materials = renderer.materials;
                foreach (var material in materials)
                {
                    _materials.Add(material);
                }
            }
        }
        
        public void SetTeamColor(Color teamColor)
        {
            SetShaderColorProperty(ColorShaderPropertyId, teamColor);
        }
        
        private void SetShaderColorProperty(int propertyId, Color propertyValue)
        {
            foreach (var renderer in _renderers)
            {
                foreach (var material in renderer.materials)
                {
                    if (material.HasProperty(propertyId))
                    {
                        material.SetColor(propertyId, propertyValue);
                    }
                }
            }
        }
    }
}