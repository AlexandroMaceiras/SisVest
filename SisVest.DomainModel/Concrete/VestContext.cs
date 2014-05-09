using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using SisVest.DomainModel.Entities;

namespace SisVest.DomainModel.Concrete
{
    public class VestContext : DbContext
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Candidato> Candidatos { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Vestibular> Vestibulares { get; set; }
    }
}
