using System;
using System.Data;
using System.Data.SqlClient;

namespace DADOS
{
    public class AcessoDados
    {
        public SqlConnection CriarConexao()
        {
            return new SqlConnection(connectionString:
                "Data Source=DESKTOP-I563B26\\SQLEXPRESS;" +
                "Initial Catalog=registroimc;" +
                "Integrated Security=True;");
        }

        // Parâmetros que irão para o banco.
        readonly SqlParameterCollection sqlParamameterCollection = new SqlCommand().Parameters;

        public void LimparParametros()
        {
            sqlParamameterCollection.Clear();
        }

        public void AdicionarParametros(string nome, object valor)
        {
            sqlParamameterCollection.Add(new SqlParameter(nome, valor));
        }

        public object ExecutarPersistencia(CommandType cmdoType, string textoSql, bool read = true)
        {
            try
            {
                SqlConnection sqlConnection = CriarConexao();
                sqlConnection.Open();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandType = cmdoType;
                sqlCommand.CommandText = textoSql;
                sqlCommand.CommandTimeout = 3600;

                foreach (SqlParameter sqlParameter in sqlParamameterCollection)
                {
                    sqlCommand.Parameters.Add(new SqlParameter(sqlParameter.ParameterName, sqlParameter.Value));
                }

                // Se 'read' for true executa um select, se for false faz um insert/update/delete.
                if (read) return sqlCommand.ExecuteReader();
                else return sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable ExecutarConsulta(CommandType cmdoType, string textoSql)
        {
            try
            {
                SqlConnection sqlConnection = CriarConexao();
                sqlConnection.Open();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandType = cmdoType;
                sqlCommand.CommandText = textoSql;
                sqlCommand.CommandTimeout = 3600;

                foreach (SqlParameter sqlParameter in sqlParamameterCollection)
                {
                    sqlCommand.Parameters.Add(new SqlParameter(sqlParameter.ParameterName, sqlParameter.Value));
                }

                // Criar um adaptador de dados SQL.
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                // Criar um DataTable para receber os dados que vem do banco.
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}