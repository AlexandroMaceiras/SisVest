using SisVest.WebUI.Models;

namespace SisVest.WebUI.Infraestrutura.Provider.Abstract
{
    public interface IAutenticacaoProvider
    {
        bool Autenticar(AutenticacaoModel autenticacaoModel, out string msgErro, string grupo);

        void Desautenticar();

        bool Autenticado { get; }

        AutenticacaoModel UsuarioAutenticado { get; }

    }
}