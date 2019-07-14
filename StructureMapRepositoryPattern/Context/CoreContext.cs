using StructureMap;
using StructureMapRepositoryPattern.Service;

namespace StructureMapRepositoryPattern.Context
{
    public class CoreContext : ICoreContext
    {
        #region IoC
        private readonly IContainer container;
        #endregion

        public CoreContext(IContainer container)
        {
            this.container = container;
        }

        public T New<T>()
        {
            return container.GetInstance<T>();
        }

        public T Query<T>() where T : IQuery
        {
            return container.GetInstance<T>();
        }
    }
}
