using DADOS;
using MODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BUSINESS
{
    public class RegistroBusiness
    {
        private readonly AcessoDados acesso = new AcessoDados();        

        #region Buscar registros de um usuário (BD)
        public DataTable BuscarRegistrosDeUmUsuario(int id)
        {
            try
            {
                acesso.LimparParametros();
                acesso.AdicionarParametros("@CodUsuario", id);

                // Query que será passada ao banco.
                string query = "SELECT Peso, Altura, DataRegistro FROM TB_Registro WHERE " +
                    "CodUsuario = @CodUsuario ORDER BY DataRegistro";

                return acesso.ExecutarConsulta(CommandType.Text, query);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Inserir um registro de IMC (BD)
        public void RegistarImc(Registro registro)
        {
            try
            {
                acesso.LimparParametros();
                acesso.AdicionarParametros("@Peso", registro.Peso);
                acesso.AdicionarParametros("@Altura", registro.Altura);
                acesso.AdicionarParametros("@DataRegistro", registro.DataRegistro);
                acesso.AdicionarParametros("@CodUsuario", registro.CodUsuario);

                // Query que será passada ao banco.
                string query = "INSERT INTO TB_Registro VALUES " +
                    "(@Peso, @Altura, @DataRegistro, @CodUsuario)";

                acesso.ExecutarPersistencia(CommandType.Text, query, false);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
