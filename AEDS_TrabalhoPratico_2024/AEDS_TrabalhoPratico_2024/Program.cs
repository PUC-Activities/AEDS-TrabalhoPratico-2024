using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace AEDS_TrabalhoPratico_2024
{
    internal class Program
    {
        static void Quicksort(Candidato[] array, int esq, int dir)
        {
            int i = esq, j = dir;
            double pivo = array[(esq + dir) / 2].NotaMedia;

            while (i <= j)
            {
                while (array[i].NotaMedia > pivo)
                    i++;
                while (array[j].NotaMedia < pivo)
                    j--;
                if (i <= j)
                {
                    Candidato temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;

                    i++;
                    j--;
                }

                if (esq < j)
                    Quicksort(array, esq, j);
                if (i < dir)
                    Quicksort(array, i, dir);
            }
        }

        static bool VerificaListaSelecionados(int quant, int limiteMaximo)
        {
            bool possuiEspaco = quant < limiteMaximo;
            return possuiEspaco;
        }

        static void ExibirSaida(Dictionary<int, Curso> listaCursos, StreamWriter arqSaida)
        {
            for (int i = 0; i < listaCursos.Count; i++)
            {
                
                Console.WriteLine($"{listaCursos.ElementAt(i).Value.NomeCurso} {listaCursos.ElementAt(i).Value.NotaDeCorte}");
                arqSaida.WriteLine($"{listaCursos.ElementAt(i).Value.NomeCurso} {listaCursos.ElementAt(i).Value.NotaDeCorte}");

                arqSaida.WriteLine("Selecionados");

                for (int j = 0; j < listaCursos.ElementAt(i).Value.CandidatosSelecionados.Count; j++)
                {
                    Candidato temp = listaCursos.ElementAt(i).Value.CandidatosSelecionados[j];

                    Console.WriteLine($"{temp.Nome} {temp.NotaMedia} {temp.NotaRedacao} {temp.NotaMat} {temp.NotaLing}");
                    arqSaida.WriteLine($"{temp.Nome} {temp.NotaMedia} {temp .NotaRedacao} {temp.NotaMat} {temp.NotaLing}");
                }

                Console.WriteLine("Fila de Espera: ");
                arqSaida.WriteLine("Fila de Espera:");

                if (listaCursos.ElementAt(i).Value.FilaEspera.ObterTamanho()  != 0)
                {
                    listaCursos.ElementAt(i).Value.FilaEspera.Mostrar();
                }
                

                Console.WriteLine();
                arqSaida.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            int quantCursos = 0, quantCandidatos = 0, countLinhas = 1;
            string linhaArquivo;

            Candidato aluno;
            Curso curso = new Curso();
            Dictionary<int, Curso> listaCursos = new Dictionary<int, Curso>();
            List<Candidato> listaCandidatos = new List<Candidato>();

            try
            {
                StreamReader arquivoEntrada = new StreamReader("entrada.txt", Encoding.UTF8);

                linhaArquivo = arquivoEntrada.ReadLine();

                while (linhaArquivo != null)
                {

                    string[] informacoesArq = new string[6];
                    informacoesArq = linhaArquivo.Split(';');

                    if (countLinhas == 1)
                    {
                        quantCursos = int.Parse(informacoesArq[0]);
                        quantCandidatos = int.Parse(informacoesArq[1]);

                    }
                    else if (countLinhas > 1 && countLinhas <= quantCursos + 1)
                    {
                        curso = new Curso();

                        curso.CodCurso = int.Parse(informacoesArq[0]);
                        curso.NomeCurso = informacoesArq[1];
                        curso.QuantVagas = int.Parse(informacoesArq[2]);

                        listaCursos.Add(curso.CodCurso, curso);
                    }
                    else if (countLinhas > quantCursos + 1 || countLinhas <= quantCursos + quantCandidatos + 1)
                    {
                        aluno = new Candidato();
                        aluno.Nome = informacoesArq[0];
                        aluno.NotaRedacao = double.Parse(informacoesArq[1]);
                        aluno.NotaLing = double.Parse(informacoesArq[2]);
                        aluno.NotaMat = double.Parse(informacoesArq[3]);
                        aluno.Curso1 = int.Parse(informacoesArq[4]);
                        aluno.Curso2 = int.Parse(informacoesArq[5]);
                        aluno.NotaMedia = Math.Round(((aluno.NotaRedacao + aluno.NotaLing + aluno.NotaMat) / 3), 2);

                        listaCandidatos.Add(aluno);

                    }


                    linhaArquivo = arquivoEntrada.ReadLine();
                    countLinhas++;
                }
                arquivoEntrada.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine("Exeption: " + e.Message);
            }

            for (int i = 0; i < listaCursos.Count; i++)
            {
                List<Candidato> temp = new List<Candidato>();


                for (int j = 0; j < listaCandidatos.Count; j++)
                {

                    if (listaCursos.ElementAt(i).Key == listaCandidatos[j].Curso1)
                    {
                        Candidato c = new Candidato(listaCandidatos[j]);
                        temp.Add(c);
                    }

                    if (listaCursos.ElementAt(i).Key == listaCandidatos[j].Curso2)
                    {
                        Candidato c = new Candidato(listaCandidatos[j]);
                        temp.Add(c);
                    }

                }

                listaCursos.ElementAt(i).Value.InstanciaTodosCandidatos(temp);

            }

            for (int i = 0; i < listaCursos.Count; i++)
            {
                Quicksort(listaCursos.ElementAt(i).Value.TodosCandidatos, 0, listaCursos.ElementAt(i).Value.TodosCandidatos.Length - 1);
            }

            for (int i = 0; i < listaCursos.Count; i++) { 

                listaCursos.ElementAt(i).Value.InstanciaCandidatosSelecionados();
                listaCursos.ElementAt(i).Value.InstanciaFilaEspera();

                for (int j = 0; j < listaCursos.ElementAt(i).Value.TodosCandidatos.Length; j++)
                    {

                        bool possuiEspaco = VerificaListaSelecionados(listaCursos.ElementAt(i).Value.CandidatosSelecionados.Count, listaCursos.ElementAt(i).Value.QuantVagas);

                        if (listaCursos.ElementAt(i).Value.TodosCandidatos[j].PassouNoCurso == false)
                        {
                            if(possuiEspaco == true)
                            {
                                listaCursos.ElementAt(i).Value.CandidatosSelecionados.Add(listaCursos.ElementAt(i).Value.TodosCandidatos[j]);
                                listaCursos.ElementAt(i).Value.NotaDeCorte = listaCursos.ElementAt(i).Value.TodosCandidatos[j].NotaMedia;

                                listaCursos.ElementAt(i).Value.TodosCandidatos[j].PassouNoCurso = true;
                            }
                            else if(possuiEspaco == false)
                            {
                                listaCursos.ElementAt(i).Value.FilaEspera.Inserir(listaCursos.ElementAt(i).Value.TodosCandidatos[j]);
                            }
                            
                        }
                        else if(possuiEspaco == false && listaCursos.ElementAt(i).Value.TodosCandidatos.Length != j && listaCursos.ElementAt(i).Value.TodosCandidatos[j].PassouNoCurso == true)
                        {
                            listaCursos.ElementAt(i).Value.FilaEspera.Inserir(listaCursos.ElementAt(i).Value.TodosCandidatos[j]);
                        }
                    }
            }



            try{

                StreamWriter arqSaida = new StreamWriter("saida.txt", false, Encoding.UTF8);

                ExibirSaida(listaCursos, arqSaida);

                arqSaida.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }


            Console.ReadLine();
            }
        }
    }


/*
            

            /*for (int i = 0; i < listaCursos.Count; i++)
            {
                bool possuiEspaco = VerificaListaSelecionados(listaCursos.ElementAt(i).Value.TodosCandidatos);

                for (int j = 0; j < listaCursos.ElementAt(i).Value.TodosCandidatos.Length; j++)
                {
                    if (listaCursos.ElementAt(i).Value.TodosCandidatos[j].PassouNoCurso == false && possuiEspaco == true)
                    {
                        listaCursos.ElementAt(i).Value.InstanciaCandidatosSelecionados(listaCursos.ElementAt(i).Value.TodosCandidatos[i]);
                        listaCursos.ElementAt(i).Value.CandidatosSelecionados.Add(listaCursos.ElementAt(i).Value.TodosCandidatos[i]);

                        listaCursos.ElementAt(i).Value.TodosCandidatos[i].PassouNoCurso = true;
                    }
                    else
                    {
                        listaCursos.ElementAt(i).Value.InstanciaFilaEspera();
                        listaCursos.ElementAt(i).Value.FilaEspera.Inserir(listaCursos.ElementAt(i).Value.TodosCandidatos[i]);
                    }
                }
            }









for (int i = 0; i < listaCursos.ElementAt(i).Value.TodosCandidatos; i++)
            {
                bool c1 = false, c2 = false; 
                double notaUltimoCandidato = 0;

                if (i != 0)
                {
                    notaUltimoCandidato = listaCandidatos[i - 1].NotaMedia;
                }

                for (int j = 0; j < listaCursos.Count; j++)
                {
                    if (listaCandidatos[i].Curso1 == listaCursos.ElementAt(j).Value.CodCurso)
                    {
                        if (notaUltimoCandidato == listaCandidatos[i].NotaMedia)
                        {
                            if (listaCandidatos[i - 1].NotaRedacao == listaCandidatos[i].NotaRedacao)
                            {
                                if (listaCandidatos[i - 1].NotaRedacao < listaCandidatos[i].NotaRedacao)
                                {
                                    c1 = true;
                                }
                            }
                        }
                        else if (notaUltimoCandidato < listaCandidatos[i].NotaMedia && listaCandidatos[i].NotaMedia < listaCursos.ElementAt(j).Value.NotaDeCorte)
                        {
                            c1 = true;
                        }
                    }
                    if (listaCandidatos[i].Curso2 == listaCursos.ElementAt(j).Value.CodCurso)
                    {
                        if (notaUltimoCandidato == listaCandidatos[i].NotaMedia)
                        {
                            if (listaCandidatos[i - 1].NotaRedacao == listaCandidatos[i].NotaRedacao)
                            {
                                if (listaCandidatos[i - 1].NotaRedacao < listaCandidatos[i].NotaRedacao)
                                {
                                    c2 = true;
                                }
                            }
                        }

                        else if (notaUltimoCandidato < listaCandidatos[i].NotaMedia && listaCandidatos[i].NotaMedia < listaCursos.ElementAt(j).Value.NotaDeCorte)
                        {
                            c2 = true;
                        }
                    }

                    listaCursos.ElementAt(j).Value.NotaDeCorte = notaUltimoCandidato;
                }

                Console.WriteLine(listaCandidatos[i].Nome);
                Console.WriteLine(c1);
                Console.WriteLine(c2);

                for (int h = 0; h < listaCursos.Count; h++)
                {
                    if (c1 == true && c2 == true)
                    {
                        if (listaCandidatos[i].Curso1 == listaCursos.ElementAt(h).Value.CodCurso)
                        {
                            listaCursos.ElementAt(h).Value.InstanciaCandidatosSelecionados(listaCandidatos[i]);
                            listaCursos.ElementAt(h).Value.CandidatosSelecionados.Add(listaCandidatos[i]);
                        }
                    }
                    else if (c1 == true && c2 == false)
                    {
                        if (listaCandidatos[i].Curso1 == listaCursos.ElementAt(h).Value.CodCurso)
                        {
                            listaCursos.ElementAt(h).Value.InstanciaCandidatosSelecionados(listaCandidatos[i]);
                            listaCursos.ElementAt(h).Value.CandidatosSelecionados.Add(listaCandidatos[i]);

                        }
                        else if (listaCandidatos[i].Curso2 == listaCursos.ElementAt(h).Value.CodCurso)
                        {
                            listaCursos.ElementAt(h).Value.InstanciaFilaEspera();
                            listaCursos.ElementAt(h).Value.FilaEspera.Inserir(listaCandidatos[i]);

                        }
                    }
                    else if (c1 == false && c2 == true)
                    {
                        if (listaCandidatos[i].Curso2 == listaCursos.ElementAt(h).Value.CodCurso)
                        {
                            listaCursos.ElementAt(h).Value.InstanciaCandidatosSelecionados(listaCandidatos[i]);
                            listaCursos.ElementAt(h).Value.CandidatosSelecionados.Add(listaCandidatos[i]);

                        }
                        else if (listaCandidatos[i].Curso1 == listaCursos.ElementAt(h).Value.CodCurso)
                        {
                            listaCursos.ElementAt(h).Value.InstanciaFilaEspera();
                            listaCursos.ElementAt(h).Value.FilaEspera.Inserir(listaCandidatos[i]);

                        }
                    }
                    else if (c1 == false && c2 == false)
                    {
                        if (listaCandidatos[i].Curso1 == listaCursos.ElementAt(h).Value.CodCurso)
                        {
                            listaCursos.ElementAt(h).Value.InstanciaFilaEspera();
                            listaCursos.ElementAt(h).Value.FilaEspera.Inserir(listaCandidatos[i]);
                        }
                        else if (listaCandidatos[i].Curso2 == listaCursos.ElementAt(h).Value.CodCurso)
                        {
                            listaCursos.ElementAt(h).Value.InstanciaFilaEspera();
                            listaCursos.ElementAt(h).Value.FilaEspera.Inserir(listaCandidatos[i]);

                        }
                    }
                }

            }*/