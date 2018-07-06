using Promig.Utils;

namespace Promig.Model {

    /*______________________________________________________________________
     |
     |                      CLASSE DE MODELAGEM DE DADOS
     |
     |      Classe para modelar objeto de log dentro do sistema, contendo
     |      seus respectivos atributos e métodos.
     |
     */
    class Log {

        //Attributos
        private int id;
        private string date;
        private string action;
        private Employe employe;

        //Construtores
        public Log() {

            this.id = -1;
            this.date = DateBr.GetDateBr();
            this.action = "";
            this.employe = null;
        }

        public Log(int id, string date, string action, Employe employe) {

            this.id = id;
            this.date = date;
            this.action = action;
            this.employe = employe;
        }

        //Getter/Setter
        public int GetId() {

            return id;
        }

        public void SetId(int id) {

            this.id = id;
        }

        public string GetDate() {

            return date;
        }

        public void SetDate(string date) {

            this.date = date;
        }

        public string GetAction() {

            return action;
        }

        public void SetAction(string action) {

            this.action = action;
        }

        public Employe GetEmploye() {

            return employe;
        }

        public void SetEmploye(Employe employe) {

            this.employe = employe;
        }

    }

}
