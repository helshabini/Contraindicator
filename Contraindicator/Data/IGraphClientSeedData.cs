using System.Threading.Tasks;

namespace Contraindicator.Data
{
    public interface IGraphClientSeedData
    {
        Task EnsureSeedDataAsync();
    }
}