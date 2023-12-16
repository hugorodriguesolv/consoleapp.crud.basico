USE geekjobs

GO


IF NOT EXISTS (SELECT NAME FROM SYS.tables WHERE NAME = 'Cidade')
BEGIN

	/* 
		CRUD
		C - CREATE
		R - READ
		U - UPDATE
		D - DELETE
	*/

	USE GEEKJOBS


	SET NOCOUNT ON;


	-- D - DELETE

	DELETE Pessoa
	DELETE Departamento
	DELETE Cidade
	DELETE Estado


	-- C - CREATE

	DBCC CHECKIDENT ('dbo.Pessoa', RESEED, 0);
	DBCC CHECKIDENT ('dbo.Departamento', RESEED, 0);
	DBCC CHECKIDENT ('dbo.Cidade', RESEED, 0);
	DBCC CHECKIDENT ('dbo.Estado', RESEED, 0);

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


	/*
	-- R - READ

	--SELECT @@IDENTITY
	--SELECT * FROM Estado
	--SELECT * FROM Cidade
	--SELECT * FROM Departamento
	--SELECT * FROM Pessoa


	SELECT 
		PES.Id AS 'CodigoPessoa',
		PES.Nome,
		PES.IdDepartamento,
		DEP.Nome,
		DEP.IdCidade,
		CID.Nome,
		CID.Populacao,
		CID.IdEstado,
		EST.Nome
	FROM
		Departamento AS DEP 
		INNER JOIN Pessoa AS PES ON (PES.IdDepartamento = DEP.Id)
		INNER JOIN Cidade AS CID ON (DEP.IdCidade = CID.Id)
		INNER JOIN Estado AS EST ON (CID.IdEstado = EST.Id)
	WHERE
		EST.ID = 1
	ORDER BY
		DEP.Nome ASC,
		EST.Nome DESC


	-- U - UPDATE

	SELECT 
		PES.Id AS 'CodigoPessoa',
		PES.Nome,
		PES.IdDepartamento,
		DEP.Nome,
		DEP.IdCidade,
		CID.Nome,
		CID.Populacao,
		CID.IdEstado,
		EST.Nome
	FROM
		Departamento AS DEP 
		INNER JOIN Pessoa AS PES ON (PES.IdDepartamento = DEP.Id)
		INNER JOIN Cidade AS CID ON (DEP.IdCidade = CID.Id)
		INNER JOIN Estado AS EST ON (CID.IdEstado = EST.Id)
	ORDER BY
		PES.Nome


	UPDATE 
		Pessoa 
	SET 
		Nome = 'Hugo Rodrigues',
		IdDepartamento = 2
	WHERE 
		ID = 1


	SELECT 
		PES.Id AS 'CodigoPessoa',
		PES.Nome,
		PES.IdDepartamento,
		DEP.Nome,
		DEP.IdCidade,
		CID.Nome,
		CID.Populacao,
		CID.IdEstado,
		EST.Nome
	FROM
		Departamento AS DEP 
		INNER JOIN Pessoa AS PES ON (PES.IdDepartamento = DEP.Id)
		INNER JOIN Cidade AS CID ON (DEP.IdCidade = CID.Id)
		INNER JOIN Estado AS EST ON (CID.IdEstado = EST.Id)
	ORDER BY
		PES.Nome


	SELECT Id, Nome FROM Pessoa
	*/


	SET NOCOUNT OFF;
END