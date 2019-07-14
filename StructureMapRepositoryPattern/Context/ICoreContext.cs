using StructureMapRepositoryPattern.Repository;
using StructureMapRepositoryPattern.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureMapRepositoryPattern.Context
{
    public interface ICoreContext
    {
        T New<T>();
        T Query<T>() where T : IQuery;
    }
}
