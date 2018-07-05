--Criando banco de dados para promig
CREATE DATABASE promig_erp_database
GO
use promig_erp_database
GO
--Criando as tabelas do banco
CREATE TABLE enderecos(
	id_endereco int PRIMARY KEY IDENTITY,
	rua varchar(255) not null,
	numero int not null,
	bairro varchar(255) not null,
	cidade varchar(255) not null,
	uf varchar(2) not null,
	cep varchar(8) not null
);
GO
CREATE TABLE pessoas( 
	id_pessoa INT PRIMARY KEY IDENTITY,
	id_endereco INT FOREIGN KEY REFERENCES enderecos(id_endereco),
	nome_pessoa varchar(255) not null,
	status int not null
);
GO
CREATE TABLE funcionarios(
	id_funcionario INT PRIMARY KEY IDENTITY,
	id_pessoa INT FOREIGN KEY REFERENCES pessoas(id_pessoa),
	permissao varchar(255) not null,
	cpf varchar(11) unique not null,
	data_admissao varchar(10),
	funcao varchar(255)
);
GO
CREATE TABLE clientes(
	id_cliente INT PRIMARY KEY IDENTITY,
    id_pessoa INT FOREIGN KEY REFERENCES pessoas(id_pessoa),
    fisico int not null,
    cpf_cnpj varchar(14),
    telefone_residencial varchar(20),
    telefone_celular varchar(20),
    contato varchar(255),
    inscricao_estadual varchar(30)
);
GO
CREATE TABLE fornecedores(
	id_fornecedor INT PRIMARY KEY IDENTITY,
    id_pessoa INT FOREIGN KEY REFERENCES pessoas(id_pessoa),
	razao_social varchar(255),
    cnpj varchar(14),
    tel_residencial varchar(20),
    tel_celular varchar(20)
);
GO
CREATE TABLE usuarios( 
	login varchar(255) unique not null,
	password varchar(255) not null,
	id_funcionario INT FOREIGN KEY REFERENCES funcionarios(id_funcionario)
);
GO
CREATE TABLE LOG(
	id_log INT PRIMARY KEY IDENTITY,
	id_funcionario INT FOREIGN KEY REFERENCES funcionarios(id_funcionario),
	log_date varchar(255),
	user_action varchar(255)
);
GO
--Inserindo dados iniciais
/* inserindo dados para teste FUNCIONARIO 01 */
insert into enderecos(rua, numero, bairro, cidade, uf, cep)
values('Rua José Cristino de Oliveira Camois', 562, 'Jd. Planalto', 'Mogi Guaçu', 'SP', '13843054')
GO
insert into pessoas(id_endereco, nome_pessoa, status)
values(1, 'Gabriel Mazurco Ribeiro', 1)
GO
insert into funcionarios(permissao, cpf, data_admissao, id_pessoa, funcao)
values('Admin','43662474883', '14/03/2018', 1, 'Pika das Galaxias')
GO
insert into usuarios(login, password, id_funcionario)
values('admin', 'Z+W5Cf4n8F0=', 1)
GO
/* Para ver todos os dados de uma tabela */
select * from pessoas;
GO
select * from enderecos;
Go
select * from funcionarios;
GO
select * from clientes;
GO
select * from fornecedores;
GO
select * from usuarios;
GO
select * from log;
GO
/* Se precisar... comando para deletar banco de dados */
drop database promig_erp_database;
