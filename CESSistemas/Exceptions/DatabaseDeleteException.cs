using System;

namespace Promig.Exceptions {
    class DatabaseDeleteException : Exception {
        public DatabaseDeleteException() : base("Erro ao delatar dados no banco! Tente novamente mais tarde!") { }
    }
}
