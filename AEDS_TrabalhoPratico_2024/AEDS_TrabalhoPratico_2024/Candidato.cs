using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEDS_TrabalhoPratico_2024
{
    internal class Candidato
    {
        private string nome;
        private double notaRedacao, notaMat, notaLing, notaMedia;

        Curso curso1 = new Curso();
        Curso curso2 = new Curso();

        public Candidato()
        {
            nome = "";
            notaRedacao = 0;
            notaMat = 0;
            notaLing = 0;
            curso1.CodCurso = 0;
            curso2.CodCurso = 0;
        }

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }
        public double NotaRedacao
        {
            get { return notaRedacao; }
            set { notaRedacao = value; }
        }
        public double NotaMat
        {
            get { return notaMat; }
            set { notaMat = value; }
        }
        public double NotaLing
        {
            get { return notaLing; }
            set { notaLing = value; }
        }
        public double NotaMedia
        {
            get { return notaMedia; }
            set { notaMedia = value; }
        }
        public int Curso1
        {
            get { return curso1.CodCurso; }
            set { curso1.CodCurso = value; }
        }
        public int Curso2
        {
            get { return curso2.CodCurso; }
            set { curso2.CodCurso = value; }
        }

    }
}
