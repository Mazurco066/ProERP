using System.Text;
using System.Security.Cryptography;
using System;

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
        private string cryptKey;

        #endregion Attributes

        #region Constructors

        public User() {

            login = "";
            password = "";
            hashCode = "";
            cryptKey = "pr0m1g";
        }

        public User(string login, string password) {

            this.login = login;
            this.password = password;
            cryptKey = "pr0m1g";
            GenerateHash();
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
            GenerateHash();
        }

        public string GetMD5Hash() {

            return hashCode;
        }

        public void SetMD5Hash(string hash) {
            Decode(hash);
        }

        #endregion Getter-Setter

        #region Crypt

        private void GenerateHash() {
            byte[] data = UTF8Encoding.UTF8.GetBytes(password);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider()) {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(cryptKey));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() {
                    Key = keys,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                }) {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    hashCode = Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }


        private void Decode(string cipherString) {
            byte[] data = Convert.FromBase64String(cipherString);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider()) {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(cryptKey));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() {
                    Key = keys,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                }) {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    password = UTF8Encoding.UTF8.GetString(results);
                }
            }
        }

        #endregion Crypt

        public override string ToString() {

            return base.ToString();
        }

    }
}
