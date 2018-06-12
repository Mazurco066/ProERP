namespace Promig.Connection
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class USUARIOS
    {
        [Key]
        public int codigo { get; set; }

        [StringLength(15)]
        public string nome_usuario { get; set; }

        [StringLength(50)]
        public string senha { get; set; }

        [StringLength(50)]
        public string endereco { get; set; }

        [StringLength(50)]
        public string bairro { get; set; }

        [StringLength(30)]
        public string cidade { get; set; }

        [StringLength(2)]
        public string estado { get; set; }

        [Required]
        [StringLength(15)]
        public string cpf { get; set; }

        [Required]
        [StringLength(30)]
        public string email { get; set; }

        [StringLength(10)]
        public string tipo { get; set; }
    }
}
