using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Entities;

namespace SisVest.Test.Entities
{
    [TestClass]
    public class VestibularTest
    {
        public Vestibular vestibular1, vestibular2;

        [TestInitialize]
        public void InicializarTest()
        {
            vestibular1 = new Vestibular()
            {
                iVestibularId = 1,
                sDescricao = "Vestibular 2013",
                dtInicioInscricao = new DateTime(2013, 3, 22),
                dtFimInscricao = new DateTime(2013, 4, 22),
                dtProva = new DateTime(2013, 4, 29)
            };
        }

        [TestMethod]
        public void Garantir_Que_2_Vestibulares_Sao_Iguais_Quando_Tem_Mesmo_Id()
        {
            vestibular2 = new Vestibular()
            {
                iVestibularId = 1,
                sDescricao = "Vestibular 2014",
                dtInicioInscricao = new DateTime(2014, 3, 22),
                dtFimInscricao = new DateTime(2014, 4, 22),
                dtProva = new DateTime(2014, 4, 29)
            };

            Assert.AreEqual(vestibular1.iVestibularId, vestibular2.iVestibularId);
            Assert.AreEqual(vestibular1, vestibular2);
        }

        [TestMethod]
        public void Garantir_Que_2_Vestibulares_Sao_Iguais_Quando_Tem_Mesma_Descricao()
        {
            vestibular2 = new Vestibular()
            {
                iVestibularId = 1,
                sDescricao = "Vestibular 2013",
                dtInicioInscricao = new DateTime(2014, 3, 22),
                dtFimInscricao = new DateTime(2014, 4, 22),
                dtProva = new DateTime(2014, 4, 29)
            };

            Assert.AreEqual(vestibular1.sDescricao, vestibular2.sDescricao);
            Assert.AreEqual(vestibular1, vestibular2);
        }

    }
}
