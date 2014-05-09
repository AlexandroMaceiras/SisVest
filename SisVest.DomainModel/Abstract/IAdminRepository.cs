using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SisVest.DomainModel.Entities;

namespace SisVest.DomainModel.Abstract
{
    public interface IAdminRepository
    {
        IQueryable<Admin> Admins { get; }
        void Excluir(int idCandidato);
        void Inserir(Admin admin);
        void Alterar(Admin admin);
        Admin Retornar(int id);
    }
}
