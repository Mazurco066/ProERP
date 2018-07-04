using System;

namespace Promig.Exceptions {
    class NegatedAcessException : Exception {
        public NegatedAcessException() : base("Acesso Negado!") { }
    }
}
