using DADOS;
using MODEL;
using System;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BUSINESS
{
    public class UsuarioBusiness
    {
        private readonly AcessoDados acesso = new AcessoDados();

        #region Hash da senha do usuário
        /// <summary>
        /// Faz o hash da senha do usuário.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string FazerHashDaSenha(string input)
        {
            byte[] hash = new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
        #endregion

        #region Buscar dados do usuário (BD)
        public DataTable BuscarDadosUsuario(Usuario usuario)
        {
            try
            {
                acesso.LimparParametros();
                acesso.AdicionarParametros("@Email", usuario.Email);
                acesso.AdicionarParametros("@Senha", usuario.Senha);

                // Query que será passada ao banco.
                string query = "SELECT * FROM TB_Usuario WHERE " +
                    "Email = @Email AND " +
                    "Senha = @Senha";

                // Retorna o datatable com os dados do jogador.
                return acesso.ExecutarConsulta(CommandType.Text, query);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region Cadastrar usuário (BD)
        public void CadastrarUsuario(UsuarioCadastro usuarioCadastro)
        {
            try
            {
                acesso.LimparParametros();
                acesso.AdicionarParametros("@Nome", usuarioCadastro.Nome);
                acesso.AdicionarParametros("@Email", usuarioCadastro.Email);
                acesso.AdicionarParametros("@Senha", usuarioCadastro.Senha);

                // Query que será passada ao banco.
                string query = "INSERT INTO TB_Usuario VALUES " +
                    "(@Nome, @Email, @Senha)";

                acesso.ExecutarPersistencia(CommandType.Text, query, false);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Buscar se e-mail já existe (BD)
        public DataTable BuscarSeEmailJaExiste(string email)
        {
            try
            {
                acesso.LimparParametros();
                acesso.AdicionarParametros("@Email", email);

                // Query que será passada ao banco.
                string query = "SELECT Email FROM TB_Usuario WHERE " +
                    "Email = @Email";

                return acesso.ExecutarConsulta(CommandType.Text, query);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
