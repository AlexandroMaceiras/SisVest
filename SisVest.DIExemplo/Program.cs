using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SisVest.DomainModel.Abstract;
using SisVest.DomainModel.Concrete;
using System.Data.Entity.SqlServer;
using Ninject;

namespace SisVest.DIExemplo
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<ICursoRepository>().To <EFCursoRepository>();
            //Não necessário
            //ninjectKernel.Bind<VestContext>().ToSelf();

            //Exemplo anterior com dependência de forma normal
            //ICursoRepository cursoRepository = new EFCursoRepository(new VestContext());
            var cursoRepository = ninjectKernel.Get<ICursoRepository>();

            foreach (var curso in cursoRepository.Cursos.ToList())
            {
                Console.WriteLine(string.Format("Curso: {0} - Total Vagas: {1} ", curso.sDescricao, curso.iVagas));
            }
        }
    }
}
