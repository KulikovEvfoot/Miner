using System.Threading.Tasks;

namespace Core.ResourceExtraction.ResourceExtractor
{
    public interface IResourceExtractorFactory
    { 
        Task<IResourceExtractor> Create();
    }
}