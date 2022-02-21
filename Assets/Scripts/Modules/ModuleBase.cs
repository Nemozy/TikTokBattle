using System.Threading.Tasks;

namespace Modules
{
    public abstract class ModuleBase
    {
        public abstract Task Connect();
        public abstract Task Stop();

        public virtual void Tick()
        {
        }
        
        public virtual Task PreloadAssets()
        {
            return Task.CompletedTask;
        }
    }
}