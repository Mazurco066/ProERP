namespace Promig.Connections
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FUNCIONARIOS
    {
        [Key]
        public int codigo { get; set; }

        [Required]
        [StringLength(50)]
        public string nome { get; set; }

        [Required]
        [StringLength(50)]
        public string endereco { get; set; }

        [Required]
        [StringLength(30)]
        public string bairro { get; set; }

        public int? numero { get; set; }

        [Required]
        [StringLength(30)]
        public string cidade { get; set; }

        [Required]
        [StringLength(20)]
        public string cep { get; set; }

        [Required]
        [StringLength(20)]
        public string telefone { get; set; }

        [Required]
        [StringLength(20)]
        public string celular { get; set; }

        [Required]
        [StringLength(20)]
        public string cargo { get; set; }

        [Required]
        [StringLength(2)]
        public string estado { get; set; }

        [Required]
        [StringLength(15)]
        public string sexo { get; set; }

        public double? salario { get; set; }
    }
}
