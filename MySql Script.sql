/* Criando banco de dados */
create database if not exists promig;
use promig;

/* Criando as tabelas */
create table if not exists enderecos(
	id_endereco int not null auto_increment,
	rua varchar(255) not null,
	numero int not null,
	bairro varchar(255) not null,
	cidade varchar(255) not null,
	uf varchar(2) not null,
	cep varchar(8) not null,
	CONSTRAINT PRK_ID_ENDERECO PRIMARY KEY (id_endereco) 
);

create table if not exists pessoas( 
	id_pessoa int not null auto_increment,
	id_endereco int not null,
	nome_pessoa varchar(255) not null,
	status boolean not null,
	CONSTRAINT PRK_ID_PESSOA PRIMARY KEY (id_pessoa),
	CONSTRAINT FRK_ID_PESSOA_ENDERECO FOREIGN KEY (id_endereco) REFERENCES enderecos(id_endereco)
);

create table if not exists funcionarios(
	id_funcionario int not null auto_increment,
	id_pessoa int not null,
	permissao varchar(255) not null,
	cpf varchar(11) unique not null,
	data_admissao varchar(10),
	funcao varchar(255),
	CONSTRAINT PRK_ID_FUNCIONARIO PRIMARY KEY (id_funcionario),
	CONSTRAINT FRK_ID_FUNCIONARIO FOREIGN KEY (id_pessoa) REFERENCES pessoas(id_pessoa)
);

create table if not exists clientes(
	id_cliente int not null auto_increment,
    id_pessoa int not null,
    fisico boolean not null,
    cpf_cnpj varchar(14),
    telefone_residencial varchar(20),
    telefone_celular varchar(20),
    contato varchar(255),
    inscricao_estadual varchar(30),
    CONSTRAINT PRK_ID_CLIENTE PRIMARY KEY (id_cliente),
    CONSTRAINT FRK_ID_CLIENTE FOREIGN KEY (id_pessoa) REFERENCES pessoas(id_pessoa)
);

create table if not exists fornecedores(
	id_fornecedor int not null auto_increment,
    id_pessoa int not null,
	razao_social varchar(255),
    cnpj varchar(14),
    tel_residencial varchar(20),
    tel_celular varchar(20),
    CONSTRAINT PRK_ID_FORNECEDOR PRIMARY KEY (id_fornecedor),
    CONSTRAINT FRK_ID_FORNECEDOR FOREIGN KEY (id_pessoa) REFERENCES pessoas(id_pessoa)
);

create table if not exists usuarios( 
	login varchar(255) not null,
	password varchar(255) not null,
	id_funcionario int not null,
	CONSTRAINT UNK_LOGIN_USUARIO UNIQUE KEY (login),
	CONSTRAINT FRK_ID_USUARIO FOREIGN KEY (id_funcionario) REFERENCES funcionarios(id_funcionario)
);

create table if not exists debitos(
	id_debito int not null auto_increment,
	id_fornecedor int not null, 
    descricao text,
    data_pagamento varchar(12),
    data_vencimento varchar(12),
    valor_total double,
    valor_inicial double,
    CONSTRAINT PRK_ID_DEBITO PRIMARY KEY (id_debito),
	CONSTRAINT FRK_ID_DEBITO FOREIGN KEY (id_fornecedor) REFERENCES fornecedores(id_fornecedor)
);

create table if not exists creditos(
	id_credito int not null auto_increment,
	id_cliente int not null, 
    descricao text ,
    data_recebimento varchar(12),
    data_vencimento varchar(12),
    valor_total double,
    valor_inicial double,
    CONSTRAINT PRK_ID_CREDITO PRIMARY KEY (id_credito),
	CONSTRAINT FRK_ID_CREDITO FOREIGN KEY (id_cliente) REFERENCES clientes(id_cliente)
);

create table if not exists orcamentos(
    no_documento int not null auto_increment,
    id_cliente int not null,
    data_orcamento varchar(12),
    caminho_imagem varchar(255),
    descricao varchar(255),
    condicao_pagto varchar(10),
    execucao_dias varchar(10),
    valor_total double not null,
    constraint PRK_DOCNO primary key (no_documento),
    constraint FRK_CLIENTE foreign key (id_cliente) references pessoas (id_pessoa)
);

create table if not exists servicos(
	no_orcamento int not null,
	descricao varchar(255)
);

create table if not exists log(
	id_log int not null AUTO_INCREMENT,
	id_funcionario int not null,
	log_date varchar(255),
	user_action varchar(255),
	CONSTRAINT PRK_ID_LOG PRIMARY KEY (id_log),
	CONSTRAINT FRK_ID_FUNCIONARIO_LOG FOREIGN KEY (id_funcionario) REFERENCES funcionarios(id_funcionario)
);

/* inserindo dados para teste FUNCIONARIO 01 - Dados iniciais do sistema */
insert into enderecos(rua, numero, bairro, cidade, uf, cep)
values('Rua José Cristino de Oliveira Camois', 562, 'Jd. Planalto', 'Mogi Guaçu', 'SP', '13843054');
insert into pessoas(id_endereco, nome_pessoa, status)
values(1, 'Gabriel Mazurco Ribeiro', true);
update pessoas set id_endereco = 1 where id_pessoa = 1;
insert into funcionarios(permissao, cpf, data_admissao, id_pessoa, funcao)
values('Admin','43662474883', '14/03/2018', 1, 'Pika das Galaxias');
insert into usuarios(login, password, id_funcionario)
values('admin', 'eKkqAeJvo3t60TtipgWMjg==', 1);

/* Para ver todos os dados de uma tabela */
select * from pessoas;
select * from enderecos;
select * from funcionarios;
select * from clientes;
select * from fornecedores;
select * from usuarios;
select * from debitos;
select * from creditos;
select * from orcamentos;
select * from servicos;
select * from log;

/* Se precisar... comando para deletar banco de dados */
drop database if exists promig;

/* Criação de procedures */
delimiter $$
create procedure AddOrcamento (
	id_cliente int,
    data_orcamento varchar(12),
    caminho_imagem varchar(255),
    descricao varchar(255),
    condicao_pagto varchar(10),
    execucao_dias varchar(10),
    valor_total double,
    OUT lis int
) begin
	insert into orcamentos (id_cliente, data_orcamento, caminho_imagem, descricao, condicao_pagto, execucao_dias, valor_total) 
    values (id_cliente, data_orcamento, caminho_imagem, descricao, condicao_pagto, execucao_dias, valor_total);
    set lis = last_insert_id();
    insert into servicos (no_orcamento, descricao)
    values (lis, 'Teste');
end $$
