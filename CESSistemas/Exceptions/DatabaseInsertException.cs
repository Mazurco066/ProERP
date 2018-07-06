using System;

namespace Promig.Exceptions {
    class DatabaseInsertException : Exception {
        public DatabaseInsertException() : base("Erro ao inserir dados no banco! Tente novamente mais tarde!") { }
    }
}
