namespace Promig.Connections
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CLIENTES
    {
        [Key]
        public int id_cliente { get; set; }

        [StringLength(50)]
        public string nome { get; set; }

        [StringLength(50)]
        public string endereco { get; set; }

        [StringLength(50)]
        public string bairro { get; set; }

        public int? numero { get; set; }

        [StringLength(10)]
        public string cep { get; set; }

        [StringLength(50)]
        public string cidade { get; set; }

        [StringLength(2)]
        public string estado { get; set; }

        [StringLength(20)]
        public string tipo { get; set; }

        [StringLength(20)]
        public string cpf_cnpj { get; set; }

        [StringLength(20)]
        public string telefone { get; set; }

        [StringLength(20)]
        public string celular { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(50)]
        public string inscricao_estadual { get; set; }
    }
}
