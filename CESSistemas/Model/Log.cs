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
        public int id { get; set; }
        public string date { get; set; }
        public string action { get; set; }
        public Employe employe { get; set; }

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

    }

}
