using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promig.Model {
    class Debit {

        #region Attributes
        private int id_debit;
        private int id_supplier;
        private string description;
        private string payment_date;
        private string due_date;
        private double total_amount;
        private double start_amount;
        #endregion

        #region Constructor
        public Debit() {
          
        }
        #endregion

        #region Getter/Setter
        public int IdDebit{ get { return id_debit; } set{ id_debit = value;} }
        public int IdSupplier { get { return id_supplier; } set { id_supplier = value; } }
        public string Description { get { return description; } set { description = value; } }
        public string PaymentDate { get { return payment_date; } set { payment_date = value; } }
        public string DueDate { get { return due_date; } set { due_date = value; } }
        public double TotalAmount { get { return total_amount; } set { total_amount = value; } }
        public double StartAmount { get { return start_amount; } set { start_amount = value; } }
        #endregion


    }
}
