using System;

namespace Promig.Model
{

    /*______________________________________________________________________
   |
   |                      CLASSE DE MODELAGEM DE DADOS
   |
   |      Classe para modelar objeto de orçamento dentro do sistema, contendo
   |      seus respectivos atributos e métodos.
   |
   */

    class Estimate {

        #region Attributes
        // Atributos
        private int docNo;
        private int idCustomer;
        private string customer;
        private DateTime date;
        private string imgPath;
        private string description;
        private string payCondition;
        private string daysExecution;
        private double totalValue;
        private string description1;
        private string description2;
        private string description3;
        private string description4;
        private string description5;
        private string description6;
        private string description7;
        #endregion

        #region Constructor
        // Construtor
        public Estimate() {

        }
        #endregion

        #region Getter/Setter
        // Acessores
        public int DocNo { get { return docNo; } set { docNo = value; } }
        public int IdCustomer { get { return idCustomer; } set { idCustomer = value; } }
        public string Customer { get { return customer; } set { customer = value; } }
        public DateTime Date { get { return date; } set { date = value; } }
        public string ImgPath { get { return imgPath; } set { imgPath = value; } }
        public string Description { get { return description; } set { description = value; } }
        public string PayCondition { get { return payCondition; } set { payCondition = value; } }
        public string DaysExecution { get { return daysExecution; } set { daysExecution = value; } }
        public double TotalValue { get { return totalValue; } set { totalValue = value; } }
        public string Description1 { get { return description1; } set { description1 = value; } }
        public string Description2 { get { return description2; } set { description2 = value; } }
        public string Description3 { get { return description3; } set { description3 = value; } }
        public string Description4 { get { return description4; } set { description4 = value; } }
        public string Description5 { get { return description5; } set { description5 = value; } }
        public string Description6 { get { return description6; } set { description6 = value; } }
        public string Description7 { get { return description7; } set { description7 = value; } }
        #endregion

    }
}