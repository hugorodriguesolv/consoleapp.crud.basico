until /opt/mssql-tools/bin/sqlcmd -S sql-server -U sa -P AulaGeekJobs1 -Q "SELECT 1" &> /dev/null; do
	echo "Aguadando o servidor SQL Server ficar pronto..."
	sleep 5
done

echo "Iniciando a criação da base de dados geekjobs"
/opt/mssql-tools/bin/sqlcmd -S sql-server -U sa -P AulaGeekJobs1 -d master -i /tmp/scripts/script-criacao-database.sql
echo "Finalizando a criação da base de dados geekjobs"

echo "Iniciando a carda de dados na base de dados geekjobs"
/opt/mssql-tools/bin/sqlcmd -S sql-server -U sa -P AulaGeekJobs1 -d geekjobs -i /tmp/scripts/carga-dados.sql
echo "Finalizando a carda de dados na base de dados geekjobs"