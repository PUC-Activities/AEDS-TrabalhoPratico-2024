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

        public void InstanciaCandidatosSelecionados(Candidato[] todosCandidatos)
        {
            for (int i = 0; i < QuantVagas; i++)
            {
                // Cria uma cópia de cada candidato
                Candidato candidato = new Candidato(todosCandidatos[i]);
                CandidatosSelecionados[i] = candidato;
            }
            
        }
        public void InstanciaFilaEspera(Candidato[] todosCandidatos)
        {
            int candidatosFilaEspera = todosCandidatos.Length - QuantVagas;

            Fila<Candidato> filaEspera = new Fila<Candidato>(candidatosFilaEspera);

            for (int i = QuantVagas; i < todosCandidatos.Length; i++)
            {
                filaEspera.Inserir(todosCandidatos[i]);
            }

            this.FilaEspera = filaEspera;
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
