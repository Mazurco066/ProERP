using System;

namespace Promig.Exceptions
{
    class DatabaseEditException : Exception {
        public DatabaseEditException() : base("Erro ao alterar dados no banco! Tente novamente mais tarde!") { }
    }
}
