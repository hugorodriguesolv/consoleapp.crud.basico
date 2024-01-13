USE geekjobs

GO


SET NOCOUNT ON;


-- D - DELETE

DELETE Pessoa
DELETE Departamento
DELETE Cidade
DELETE Estado


-- C - CREATE

DBCC CHECKIDENT ('dbo.Pessoa', RESEED, 1);
DBCC CHECKIDENT ('dbo.Departamento', RESEED, 1);
DBCC CHECKIDENT ('dbo.Cidade', RESEED, 1);
DBCC CHECKIDENT ('dbo.Estado', RESEED, 1);

INSERT INTO Estado (Nome) VALUES ('São Paulo')

DECLARE @IDESTADO AS INT
SET @IDESTADO = @@IDENTITY

INSERT INTO Cidade (Nome, Populacao, IdEstado) VALUES ('São Paulo', 10000000, @IDESTADO)
INSERT INTO Departamento (Nome, IdCidade) VALUES ('Suprimento', @@IDENTITY)
INSERT INTO Pessoa (Nome, IdDepartamento) VALUES ('Hugo', @@IDENTITY)

INSERT INTO Cidade (Nome, Populacao, IdEstado) VALUES ('Cotia', 10000000, @IDESTADO)
INSERT INTO Departamento (Nome, IdCidade) VALUES ('Centro de Distribuição', @@IDENTITY)
INSERT INTO Pessoa (Nome, IdDepartamento) VALUES ('Pedro Lucas', @@IDENTITY)


INSERT INTO Estado (Nome) VALUES ('Ceará')
SET @IDESTADO = @@IDENTITY

INSERT INTO Cidade (Nome, Populacao, IdEstado) VALUES ('Pedra Branca', 300000, @IDESTADO)
INSERT INTO Departamento (Nome, IdCidade) VALUES ('RH', @@IDENTITY)
INSERT INTO Pessoa (Nome, IdDepartamento) VALUES ('Caio', @@IDENTITY)

INSERT INTO Cidade (Nome, Populacao, IdEstado) VALUES ('Fortaleza', 2000000, @IDESTADO)
INSERT INTO Departamento (Nome, IdCidade) VALUES ('Vendas', @@IDENTITY)
INSERT INTO Pessoa (Nome, IdDepartamento) VALUES ('Sérgio', @@IDENTITY)


SET NOCOUNT OFF;