using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promig.Model {


    /*______________________________________________________________________
    |
    |                      CLASSE DE MODELAGEM DE DADOS
    |
    |       Classe para modelar objeto de pedido de venda dentro do sistema, contendo
    |       seus respectivos atributos e métodos.
    |
    */


    class SaleOrder {

        #region Attributes

        private int no_saleOrder;
        private int no_estimate;
        private string date_realization;
        private string situation;
        private double discount;
        private double totalDiscount;
        private Estimate estimate;

        #endregion

        #region Constructor

        public SaleOrder(){

        }

        #endregion

        #region Getter/Setter

        public int No_saleOrder{
          get{ return no_saleOrder; }
          set{ no_saleOrder = value; }
        }
        public int No_estimate{
          get{ return no_estimate; }
          set{ no_estimate = value; }
        }
        public string Date_realization{
          get{ return date_realization; }
          set{ date_realization = value; }
        }
        public string Situation{
          get{ return situation; }
          set{ situation = value; }
        }
        public double Discount{
          get{ return discount; }
          set{ discount = value; }
        }
        public double TotalDiscount{
          get{ return totalDiscount; }
          set{ totalDiscount = value; }
        }
        public Estimate Estimate{
          get{ return estimate; }
          set{ estimate = value; }
        }

        #endregion
    }
}
