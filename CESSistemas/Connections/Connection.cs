namespace Promig.Connections {
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Promig.Migrations;

    public partial class Connection : DbContext {
        public Connection()
            : base("name=Connection") {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Connection, Configuration>());
        }

        public virtual DbSet<CLIENTES> CLIENTES { get; set; }
        public virtual DbSet<CONTAS_PAGAR> CONTAS_PAGAR { get; set; }
        public virtual DbSet<CONTAS_RECEBER> CONTAS_RECEBER { get; set; }
        public virtual DbSet<FORNECEDORES> FORNECEDORES { get; set; }
        public virtual DbSet<FUNCIONARIOS> FUNCIONARIOS { get; set; }
        public virtual DbSet<USUARIOS> USUARIOS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.endereco)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.bairro)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.cep)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.cidade)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.tipo)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.cpf_cnpj)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.telefone)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.celular)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.inscricao_estadual)
                .IsUnicode(false);

            modelBuilder.Entity<CONTAS_PAGAR>()
                .Property(e => e.descricao)
                .IsUnicode(false);

            modelBuilder.Entity<CONTAS_RECEBER>()
                .Property(e => e.descricao)
                .IsUnicode(false);

            modelBuilder.Entity<FORNECEDORES>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<FORNECEDORES>()
                .Property(e => e.endereco)
                .IsUnicode(false);

            modelBuilder.Entity<FORNECEDORES>()
                .Property(e => e.bairro)
                .IsUnicode(false);

            modelBuilder.Entity<FORNECEDORES>()
                .Property(e => e.cidade)
                .IsUnicode(false);

            modelBuilder.Entity<FORNECEDORES>()
                .Property(e => e.cep)
                .IsUnicode(false);

            modelBuilder.Entity<FORNECEDORES>()
                .Property(e => e.telefone)
                .IsUnicode(false);

            modelBuilder.Entity<FORNECEDORES>()
                .Property(e => e.tipo)
                .IsUnicode(false);

            modelBuilder.Entity<FORNECEDORES>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<FUNCIONARIOS>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<FUNCIONARIOS>()
                .Property(e => e.endereco)
                .IsUnicode(false);

            modelBuilder.Entity<FUNCIONARIOS>()
                .Property(e => e.bairro)
                .IsUnicode(false);

            modelBuilder.Entity<FUNCIONARIOS>()
                .Property(e => e.cidade)
                .IsUnicode(false);

            modelBuilder.Entity<FUNCIONARIOS>()
                .Property(e => e.cep)
                .IsUnicode(false);

            modelBuilder.Entity<FUNCIONARIOS>()
                .Property(e => e.telefone)
                .IsUnicode(false);

            modelBuilder.Entity<FUNCIONARIOS>()
                .Property(e => e.celular)
                .IsUnicode(false);

            modelBuilder.Entity<FUNCIONARIOS>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<FUNCIONARIOS>()
                .Property(e => e.sexo)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.nome_usuario)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.senha)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.endereco)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.bairro)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.cidade)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.cpf)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.tipo)
                .IsUnicode(false);
        }
    }
}
