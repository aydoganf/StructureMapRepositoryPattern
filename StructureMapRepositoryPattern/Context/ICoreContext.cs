using StructureMapRepositoryPattern.Service;

namespace StructureMapRepositoryPattern.Context
{
    public interface ICoreContext
    {
        T New<T>();
        T Query<T>() where T : IQuery;
    }
}
