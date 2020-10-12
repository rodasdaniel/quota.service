using Application.Quota.Dtos;
using System.Threading.Tasks;

namespace Infrastructure.Quota.Agents.Client
{
    public interface IClientProvider
    {
        Task<InfoClientDto> GetInfoClient(long idClient);
    }
}
