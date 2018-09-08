using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promig.Model {
    class Credit {

        #region Attributes
        private int id_credit;
        private int id_customer;
        private string description;
        private string receipt_date;
        private string due_date;
        private double total_amount;
        private double start_amount;
        #endregion

        #region Constructor
        public Credit() {

        }
        #endregion

        #region Getter/Setter
        public int IdCredit { get { return id_credit; } set { id_credit = value; } }
        public int IdCustomer { get { return id_customer; } set { id_customer = value; } }
        public string Description { get { return description; } set { description = value; } }
        public string ReceiptDate { get { return receipt_date; } set { receipt_date = value; } }
        public string DueDate { get { return due_date; } set { due_date = value; } }
        public double TotalAmount { get { return total_amount; } set { total_amount = value; } }
        public double StartAmount { get { return start_amount; } set { start_amount = value; } }
        #endregion

    }
}
