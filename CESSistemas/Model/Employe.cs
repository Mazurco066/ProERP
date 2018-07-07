namespace Promig.Model {

    /*______________________________________________________________________
     |
     |                      CLASSE DE MODELAGEM DE DADOS
     |
     |      Classe para modelar objeto de funcionário dentro do sistema, contendo
     |      seus respectivos atributos e métodos.
     |
     */
    class Employe : Person {

        #region Attributes

        public int id { get; set; }
        public string cpf { get; set; }
        public string rg { get; set; }
        public string role { get; set; }
        public string admission { get; set; }
        public string demission { get; set; }
        public string job { get; set; }
        public User user { get; set; }

        #endregion Attributes

        #region Constructors

        public Employe() {

            this.id = -1;
            base.SetIdPerson(-1);
            base.SetName("model");
            this.role = "User";
            this.cpf = "";
            this.rg = "";
            this.admission = "";
            this.demission = "";
            this.job = "";
            this.user = null;
        }

        #endregion Constructors

        #region Getter/Setter

        public int GetId() {

            return id;
        }

        public void SetId(int id) {

            this.id = id;
        }

        public string GetCpf() {

            return cpf;
        }

        public void SetCpf(string cpf) {

            this.cpf = cpf;
        }

        public string GetRg() {

            return rg;
        }

        public void SetRg(string rg) {

            this.rg = rg;
        }

        public string GetJob() {

            return job;
        }

        public void SetJob(string job) {

            this.job = job;
        }

        public string GetAdmission() {

            return admission;
        }

        public void SetAdmission(string admission) {

            this.admission = admission;
        }

        public string GetDemission() {

            return demission;
        }

        public void SetDemission(string demission) {

            this.demission = demission;
        }

        public string GetRole() {

            return role;
        }

        public void SetRole(string role) {

            this.role = role;
        }

        public User GetUser() {

            return user;
        }

        public void SetUser(User user) {

            this.user = user;
        }

        #endregion Getter/Setter

        #region Class-Methods

        //Método para impressão na grid
        public string[] ToStringArray() {

            //Definindo vetor que será retornado
            string[] _return = new string[4];

            //Preenchendo dados do cliente
            _return[0] = GetId().ToString();
            _return[1] = GetName();
            _return[2] = GetAdress().GetCity();
            _return[3] = cpf;

            //Retornando informações do cliente para grid
            return _return;

        }

        #endregion Class-Methods

    }
}
