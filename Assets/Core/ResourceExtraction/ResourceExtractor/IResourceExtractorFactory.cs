using Common;

namespace Core.ResourceExtraction.ResourceExtractor
{
    public interface IResourceExtractorFactory
    {
        Result<IResourceExtractor> Create();
    }
}