using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisVest.DomainModel.Entities;

namespace SisVest.Test.Entities
{
    [TestClass]
    public class CandidatoTest
    {
        public Candidato candidato1, candidato2;

        [TestInitialize]
        public void InicializarTest()
        {
            candidato1 = new Candidato()
            {
                iCandidatoId = 1,
                bAprovado = false,
                sCpf = "12345678923",
                dtNascimento = new DateTime(1990,1,1),
                sEmail = "candidato1@devmedia.com.br",
                sNome = "Candidato1"
            };
        }

        [TestMethod]
        public void Garantir_Que_2_Candidatos_Sao_Iguais_Quando_Tem_Mesmo_Id()
        {
            candidato2 = new Candidato()
            {
                iCandidatoId = 1,
                bAprovado = true,
                sCpf = "323256788923",
                dtNascimento = new DateTime(1992, 1, 1),
                sEmail = "candidato2@devmedia.com.br",
                sNome = "Candidato2"
            };

            Assert.AreEqual(candidato1.iCandidatoId, candidato2.iCandidatoId);
            Assert.AreEqual(candidato1, candidato2);
        }

        [TestMethod]
        public void Garantir_Que_2_Candidatos_Sao_Iguais_Quando_Tem_Mesmo_Cpf()
        {
            candidato2 = new Candidato()
            {
                iCandidatoId = 2,
                bAprovado = true,
                sCpf = "12345678923",
                dtNascimento = new DateTime(1992, 1, 1),
                sEmail = "candidato2@devmedia.com.br",
                sNome = "Candidato2"
            };

            Assert.AreEqual(candidato1.sCpf, candidato2.sCpf);
            Assert.AreEqual(candidato1, candidato2);
        }

        [TestMethod]
        public void Garantir_Que_2_Candidatos_Sao_Iguais_Quando_Tem_Mesmo_Email()
        {
            candidato2 = new Candidato()
            {
                iCandidatoId = 2,
                bAprovado = true,
                sCpf = "222256788923",
                dtNascimento = new DateTime(1992, 1, 1),
                sEmail = "candidato1@devmedia.com.br",
                sNome = "Candidato2"
            };

            Assert.AreEqual(candidato1.sEmail, candidato2.sEmail);
            Assert.AreEqual(candidato1, candidato2);
        }
    }
}
