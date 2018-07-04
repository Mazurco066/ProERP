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
        private int id_person;
        private string name;
        private Adress adress;
        private bool active;

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

        //Getter/Setter
        public int GetIdPerson() {

            return id_person;
        }

        public void SetIdPerson(int id) {

            this.id_person = id;
        }

        public string GetName() {

            return name;
        }

        public void SetName(string name) {

            this.name = name;
        }

        public Adress GetAdress() {

            return this.adress;
        }

        public void SetAdress(Adress adress) {

            this.adress = adress;
        }

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
