using System.Threading.Tasks;

namespace Infrastructure.CQRS.Serializers
{
    public interface IEventSerializer
    {
        Task<string> Serialize(object e);
        Task<T> Deserialize<T>(string value);
    }
}