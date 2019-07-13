using System;

namespace StructureMapRepositoryPattern.Utility
{
    public class DataAlreadyExistException : Exception
    {
        string errorMessage;
        public DataAlreadyExistException(string message)
        {
            errorMessage = message;
        }

        public override string Message => errorMessage;
    }
}
