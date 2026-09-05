create table clientes (
id int primary key auto_increment,
nome varchar(100) not null,
email varchar(100) not null,
cpf varchar(11),
ativo boolean );
