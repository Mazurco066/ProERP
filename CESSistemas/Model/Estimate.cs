using System;
using System.Collections.Generic;

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

        private int docNo;
        private int idCustomer;
        private string nameCustomer;
        private string date;
        private string imgPath;
        private string description;
        private string payCondition;
        private string daysExecution;
        private double totalValue;
        private List<ItemEstimate> items;

        #endregion

        #region Constructor

        public Estimate() { items = new List<ItemEstimate>(); }

        #endregion

        #region Getter-Setter

        public int DocNo {
            get { return docNo; }
            set { docNo = value; }
        }

        public int IdCustomer {
            get { return idCustomer; }
            set { idCustomer = value; }
        }

        public string NameCustomer {
            get { return nameCustomer;  }
            set { nameCustomer = value; }
        }

        public string Date {
            get { return date; }
            set { date = value; }
        }

        public string ImgPath {
            get { return imgPath; }
            set { imgPath = value; }
        }

        public string Description {
            get { return description; }
            set { description = value; }
        }

        public string PayCondition {
            get { return payCondition; }
            set { payCondition = value; }
        }

        public string DaysExecution {
            get { return daysExecution; }
            set { daysExecution = value; }
        }

        public double TotalValue {
            get {
                totalValue = 0;
                foreach (ItemEstimate item in items) totalValue += item.SubTotal;
                return totalValue;
            }
            set { totalValue = value; }
        }

        public List<ItemEstimate> Items {
            get { return items; }
            set { items = value; }
        }

        #endregion
    }
}