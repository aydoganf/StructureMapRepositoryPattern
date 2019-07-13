using System.Collections.Generic;

namespace StructureMapRepositoryPattern.Service
{
    public interface IDataValidator<T> where T : IData
    {
        IEnumerable<string> Validate(T data, List<T> dataSource);
    }
}
