using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Entities;

namespace SisVest.Test.Entities
{

    [TestClass]
    public class CursoTest
    {
        public Curso curso1, curso2;

        [TestInitialize]
        public void InicializarTest()
        {
            curso1 = new Curso()
            {
                iCursoID = 1,
                iVagas = 50,
                sDescricao = "computação"
            };
        }

        [TestMethod]
        public void Garantir_Que_2_Cursos_Sao_Iguais_Quando_Tem_Mesmo_Id()
        {
            curso2 = new Curso()
            {
                iCursoID = 1,
                iVagas = 55,
                sDescricao = "engenharia"
            };

            Assert.AreEqual(curso1.iCursoID, curso2.iCursoID);
            Assert.AreEqual(curso1, curso2);
        }

        //[TestMethod]
        //public void Garantir_Que_2_Cursos_Sao_Iguais_Quando_Tem_Mesma_Quantidade_De_Vagas()
        //{
        //    curso2 = new Curso()
        //    {
        //        iCursoID = 2,
        //        iVagas = 50,
        //        sDescricao = "engenharia"
        //    };

        //    Assert.AreEqual(curso1.iVagas, curso2.iVagas);
        //    Assert.AreEqual(curso1, curso2);
        //}

        [TestMethod]
        public void Garantir_Que_2_Cursos_Sao_Iguais_Quando_Tem_Mesma_Descricao()
        {
            curso2 = new Curso()
            {
                iCursoID = 2,
                iVagas = 55,
                sDescricao = "computação"
            };

            Assert.AreEqual(curso1.sDescricao, curso2.sDescricao);
            Assert.AreEqual(curso1, curso2);
        }
    }
}
