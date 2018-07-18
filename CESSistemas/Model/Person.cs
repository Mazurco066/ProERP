namespace Promig.Model {

    /*______________________________________________________________________
     |
     |                      CLASSE DE MODELAGEM DE DADOS
     |
     |      Classe para modelar objeto de pessoa dentro do sistema, contendo
     |      seus respectivos atributos e métodos.
     |
     */
    class Person {

        #region Attributes

        //Atributos
        public int id_person { get; set; }
        public string name { get; set; }
        public Adress adress { get; set; }
        public bool active { get; set; }

        #endregion Attributes

        #region Constructors

        public Person() {

            this.id_person = -1;
            this.name = "";
            this.adress = new Adress();
            this.active = true;
        }

        #endregion Constructors

        #region Getter-Setter

        //Boolean Control

        public void Activate() {

            this.active = true;
        }

        public void Inactivate() {

            this.active = false;
        }

        #endregion Getter-Setter

        #region Class-Methods

        public bool IsActive() {

            return this.active;
        }

        #endregion Class-Methods

    }
}
