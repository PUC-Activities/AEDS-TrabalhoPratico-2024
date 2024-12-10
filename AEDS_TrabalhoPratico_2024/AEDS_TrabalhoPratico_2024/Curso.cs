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
        private Fila<Candidato> filaEspera;
        private List<Candidato> candidatosSelecionados;
        public Curso()
        {
            nomeCurso = "";
            codCurso = 0;
            codCurso = 0;
            notaDeCorte = 0;
        }

        public void InstanciaTodosCandidatos(List<Candidato> temp)
        {
            TodosCandidatos = new Candidato[temp.Count];
            for (int i = 0; i < temp.Count; i++)
            {
                Candidato candidato = new Candidato(temp[i]);
                TodosCandidatos[i] = candidato;
            }

        }

        public void InstanciaCandidatosSelecionados()
        {
            candidatosSelecionados = new List<Candidato>();

        }
        public void InstanciaFilaEspera()
        {
            int tamanhoFila = Math.Max(TodosCandidatos.Length - QuantVagas, 1);
            FilaEspera = new Fila<Candidato>(tamanhoFila);

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
        public List<Candidato> CandidatosSelecionados
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

