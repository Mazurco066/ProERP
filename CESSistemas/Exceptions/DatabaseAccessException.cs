using System;

namespace Promig.Exceptions {
    class DatabaseAccessException : Exception {
        public DatabaseAccessException() : base("Erro ao se conectar ao banco!") { }
    }
}
