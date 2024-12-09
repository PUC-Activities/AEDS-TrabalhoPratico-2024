using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEDS_TrabalhoPratico_2024
{
    internal class Curso
    {
        private string nomeCurso;
        private int codCurso, quantVagas;
        double notaDeCorte;
        private Candidato[] todosCandidatos;
        private Fila<Candidato> candidatosSelecionados, filaEspera;
        public Curso()
        {
            nomeCurso = "";
            codCurso = 0;
            codCurso = 0;
            notaDeCorte = 0;
        }

        public void InstanciaObj(int quantInscritosCurso)
        {
            todosCandidatos = new Candidato[quantInscritosCurso];

        }
        public string NomeCurso
        {
            get { return nomeCurso; }
            set { nomeCurso = value; }
        }

        public int CodCurso
        {
            get { return codCurso; }
            set { codCurso = value; }
        }

        public int QuantVagas
        {
            get { return quantVagas; }
            set { quantVagas = value; }
        }

        public double NotaDeCorte
        {
            get { return notaDeCorte; }
            set { notaDeCorte = value; }
        }

        public Candidato[] TodosCandidatos
        {
            get { return todosCandidatos; }
            set { todosCandidatos = value; }
        }
        public Fila<Candidato> CandidatosSelecionados
        {
            get { return candidatosSelecionados; }
            set { candidatosSelecionados = value; }
        }

        public Fila<Candidato> FilaEspera
        {
            get { return filaEspera; }
            set { filaEspera = value; }
        }

    }
}
