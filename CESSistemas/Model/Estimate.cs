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
        
        public int docNo;
        public int idCustomer;
        public string date;
        public string imgPath;
        public string description;
        public string payCondition;
        public string daysExecution;
        public double totalValue;
        public List<Service> services { get; set; }
        private List<ItemEstimate> items;

        #endregion

        #region Constructor

        public Estimate() { services = new List<Service>(); }

        #endregion

        #region Getter-Setter

        public List<ItemEstimate> Items {
            get { return items; }
            set { items = value; }
        }

        #endregion
    }
}