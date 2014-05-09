using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SisVest.DomainModel.Abstract;
using System.Data.Entity;

namespace SisVest.DomainModel.Concrete
{
    public class EFAdminRepository : IAdminRepository
    {
        private VestContext vestContext;

        public EFAdminRepository(VestContext context)
        {
            vestContext = context;
        }

        public IQueryable<Entities.Admin> Admins
        {
            get { return vestContext.Admins.AsQueryable(); }
        }

        public void Excluir(int idAdmin)
        {
            var result = from a in Admins
                         where a.iAdminID.Equals(idAdmin)
                         select a;

            if (result.Count() == 0)
                throw new InvalidOperationException("Administrador não localizado no repositório!");

            vestContext.Admins.Remove(result.FirstOrDefault());
            vestContext.SaveChanges();
        }

        public void Inserir(Entities.Admin admin)
        {
            var result = from a in Admins
                         where a.sLogin.ToUpper().Equals(admin.sLogin) ||
                         a.sEmail.ToUpper().Equals(admin.sEmail)
                         select a;

            if (result.Count() > 0)
                throw new InvalidOperationException("Já existe um Administrador cadastrado com esse login ou email!");

            vestContext.Admins.Add(admin);
            vestContext.SaveChanges();
        }

        public void Alterar(Entities.Admin admin)
        {
            var result = from a in Admins
                         where (a.sLogin.ToUpper().Equals(admin.sLogin) ||
                         a.sEmail.ToUpper().Equals(admin.sEmail))
                         && !a.iAdminID.Equals(admin.iAdminID)
                         select a;

            if (result.Count() > 0)
                throw new InvalidOperationException("Já existe um Administrador cadastrado com esse login ou email!");

            // Isto é para quando alterar o valor de algum campo e a entidade chegar aqui populada ele 
            //poder saber o que fazer com ela, ou seja modificar pra depois poder salvar.
            vestContext.Entry(admin).State = EntityState.Modified;
            vestContext.SaveChanges();
        }

        public Entities.Admin Retornar(int id)
        {
            return Admins.Where(a => a.iAdminID.Equals(id)).FirstOrDefault();
        }
    }
}
