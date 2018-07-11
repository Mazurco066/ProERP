using Promig.Utils;

namespace Promig.Model {

    /*______________________________________________________________________
     |
     |                      CLASSE DE MODELAGEM DE DADOS
     |
     |      Classe para modelar objeto de usuário dentro do sistema, contendo
     |      seus respectivos atributos e métodos.
     |
     */
    class User {

        #region Attributes

        private string login;
        private string password;
        private string hashCode;

        #endregion

        #region Constructors

        public User() {

            login = "";
            password = "";
            hashCode = "";
        }

        public User(string login, string password) {

            this.login = login;
            this.password = password;
            this.hashCode = Crypt.Encrypt(password);
        }

        #endregion Constructors

        #region Getter-Setter

        public string GetLogin() {

            return login;
        }

        public void SetLogin(string login) {

            this.login = login;
        }

        public string GetPassword() {

            return password;
        }

        public void SetPassword(string password) {

            this.password = password;
            this.hashCode = Crypt.Encrypt(password);
        }

        public string GetEncryptedCode() {

            return hashCode;
        }

        public void SetEncryptedCode(string code) {
            this.hashCode = code;
            this.password = Crypt.Decrypt(code);
        }

        #endregion Getter-Setter
    }
}
