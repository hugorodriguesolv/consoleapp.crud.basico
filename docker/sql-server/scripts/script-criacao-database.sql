create database geekjobs

go

use geekjobs

go 

create table dbo.Estado (
	Id int identity primary key not null,
	Nome varchar(100) not null
)

go

create table dbo.Cidade (
	Id int identity primary key not null,
	Nome varchar(100) not null,
	Populacao int not null,
	IdEstado int not null references dbo.Estado(id)
)

go

create table dbo.Departamento (
	Id int identity primary key not null,
	Nome varchar(50) not null,
	IdCidade int not null references dbo.Cidade(id)
)

go

create table dbo.Pessoa (
	Id int identity primary key not null,
	Nome varchar(50) not null,
	IdDepartamento int not null references dbo.Departamento(id)
)

