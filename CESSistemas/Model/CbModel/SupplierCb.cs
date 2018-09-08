using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promig.Model.CbModel {
    class SupplierCb {

        public int id { get; set; }
        public string name { get; set; }

        public SupplierCb() { }

        public SupplierCb(int id, string name) {
            this.id = id;
            this.name = name;
        }

    }
}
