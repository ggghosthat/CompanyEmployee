using System.Collections.Generic;
namespace Entities.LinkModels;
public class LinkCollectionWrapper<T> : LinkResourceBase
{
    public IList<T> Value {get; set;} = new List<T>();

    public LinkCollectionWrapper()
    {}

    public LinkCollectionWrapper(IList<T> value)
    {
        Value = value;
    }
}
