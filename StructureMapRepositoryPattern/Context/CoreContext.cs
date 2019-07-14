using StructureMap;
using StructureMapRepositoryPattern.Repository;
using StructureMapRepositoryPattern.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureMapRepositoryPattern.Context
{
    public class CoreContext : ICoreContext
    {
        private readonly IContainer container;
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
