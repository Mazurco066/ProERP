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
        public string customer;
        public string date;
        public string imgPath;
        public string description;
        public string payCondition;
        public string daysExecution;
        public double totalValue;
        public List<string> services { get; set; }

        #endregion

        #region Constructor

        public Estimate() { services = new List<string>(); }

        #endregion
    }
}