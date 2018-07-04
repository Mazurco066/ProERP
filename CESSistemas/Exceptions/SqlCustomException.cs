using System;

namespace Promig.Exceptions {
    class SqlCustomException : Exception {
        public SqlCustomException() : base("Erro de Conexão!") { }
    }
}
