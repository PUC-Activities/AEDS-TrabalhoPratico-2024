using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEDS_TrabalhoPratico_2024
{
    internal class Fila<T>
    {
        private Candidato[] fila;
        private int primeiro, ultimo;

        public Fila(int tamanho)
        {
            fila = new Candidato[tamanho + 1];
            primeiro = ultimo = 0;
        }

        public int ObterTamanho()
        {
            return ultimo - primeiro;
        }

        public Candidato ObterPrimeiro()
        {
            Candidato primeiroFila = fila[primeiro];
            return primeiroFila;
        }

        public void Inserir(Candidato candidato)
        {

            if ((ultimo + 1) % fila.Length != primeiro)
            {
                fila[ultimo] = candidato;
                ultimo = (ultimo + 1) % fila.Length;
            }                   

        }

        public Candidato Remover()
        {
            if (primeiro == ultimo)
            {
                throw new Exception("Erro, fila vazia");
            }

            Candidato resposta = fila[primeiro];
            primeiro = (primeiro + 1) % fila.Length;
            return resposta;
        }

        public void Mostrar()
        {
            int i = primeiro;

            while (i != ultimo)
            {
                Console.WriteLine($"{fila[i].Nome} {fila[i].NotaMedia} {fila[i].NotaRedacao} {fila[i].NotaMat} {fila[i].NotaLing}");
                i = (i + 1) % fila.Length;
            }

        }

    }
}
