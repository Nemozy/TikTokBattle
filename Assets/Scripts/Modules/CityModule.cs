using System.Threading.Tasks;

namespace Modules
{
    public class CityModule : ModuleBase
    {
        /*private async Task<CityView> CreateView()
        {
            
        }*/

        public override /*async*/ Task Connect()
        {
            return Task.CompletedTask;
        }
        
        public override /*async*/ Task Stop()
        {
            return Task.CompletedTask;
        }
    }
}