using AEDS_TrabalhoPratico_2024;
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
                    arqSaida.WriteLine($"{temp.Nome} {temp.NotaMedia} {temp.NotaRedacao} {temp.NotaMat} {temp.NotaLing}");
                }

                Console.WriteLine("Fila de Espera: ");
                arqSaida.WriteLine("Fila de Espera:");

                if (listaCursos.ElementAt(i).Value.FilaEspera.ObterTamanho() != 0)
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

                listaCursos.ElementAt(i).Value.InstanciaCandidatosSelecionados();
                listaCursos.ElementAt(i).Value.InstanciaFilaEspera();
            }

            for (int i = 0; i < listaCursos.Count; i++)
            {
                for (int j = 0; j < listaCursos.ElementAt(i).Value.TodosCandidatos.Length; j++)
                {
                    Candidato candidatoAtual = listaCursos.ElementAt(i).Value.TodosCandidatos[j];
                    bool possuiEspacoNoCurso1 = VerificaListaSelecionados(listaCursos.ElementAt(i).Value.CandidatosSelecionados.Count, listaCursos.ElementAt(i).Value.QuantVagas);

                    if (candidatoAtual.Curso1 == listaCursos.ElementAt(i).Key)
                    {
                        if (candidatoAtual.PassouNoCurso == false)
                        {
                            if (possuiEspacoNoCurso1)
                            {
                                listaCursos.ElementAt(i).Value.CandidatosSelecionados.Add(candidatoAtual);
                                listaCursos.ElementAt(i).Value.NotaDeCorte = candidatoAtual.NotaMedia;
                                candidatoAtual.PassouNoCurso = true;
                            }
                            else
                            {
                                int curso2 = candidatoAtual.Curso2;

                                if (curso2 >= 0 && curso2 < listaCursos.Count)
                                {
                                    var cursoSelecionado2 = listaCursos[curso2];

                                    if (cursoSelecionado2.CandidatosSelecionados.Count < cursoSelecionado2.QuantVagas)
                                    {                                       
                                        listaCursos.ElementAt(i).Value.FilaEspera.Inserir(candidatoAtual); 

                                        if (candidatoAtual.NotaRedacao > cursoSelecionado2.NotaDeCorte)
                                        {
                                            cursoSelecionado2.CandidatosSelecionados.Add(candidatoAtual);
                                            cursoSelecionado2.NotaDeCorte = candidatoAtual.NotaMedia;
                                            candidatoAtual.PassouNoCurso = true;
                                        }
                                        else if (candidatoAtual.NotaRedacao == cursoSelecionado2.NotaDeCorte)
                                        {
                                            if (candidatoAtual.NotaMat > cursoSelecionado2.NotaDeCorte)
                                            {
                                                cursoSelecionado2.CandidatosSelecionados.Add(candidatoAtual);
                                                cursoSelecionado2.NotaDeCorte = candidatoAtual.NotaMedia;
                                                candidatoAtual.PassouNoCurso = true;
                                            }
                                            else
                                            {
                                                cursoSelecionado2.FilaEspera.Inserir(candidatoAtual);
                                            }
                                        }
                                        else
                                        {
                                            cursoSelecionado2.FilaEspera.Inserir(candidatoAtual);
                                        }
                                    }
                                    else
                                    {
                                        listaCursos.ElementAt(i).Value.FilaEspera.Inserir(candidatoAtual);
                                        cursoSelecionado2.FilaEspera.Inserir(candidatoAtual);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            try
            {

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
 
 
            for (int i = 0; i < listaCursos.Count; i++)
            {
                listaCursos.ElementAt(i).Value.InstanciaCandidatosSelecionados();
                listaCursos.ElementAt(i).Value.InstanciaFilaEspera();

                for (int j = 0; j < listaCursos.ElementAt(i).Value.TodosCandidatos.Length; j++)
                {
                    Candidato candidatoAtual = listaCursos.ElementAt(i).Value.TodosCandidatos[j];
                    bool possuiEspacoNoCurso1 = VerificaListaSelecionados(listaCursos.ElementAt(i).Value.CandidatosSelecionados.Count, listaCursos.ElementAt(i).Value.QuantVagas);

                    if (candidatoAtual.Curso1 == listaCursos.ElementAt(i).Key)
                    {
                        if (candidatoAtual.PassouNoCurso == false)
                        {
                            if (possuiEspacoNoCurso1)
                            {
                                listaCursos.ElementAt(i).Value.CandidatosSelecionados.Add(candidatoAtual);
                                listaCursos.ElementAt(i).Value.NotaDeCorte = candidatoAtual.NotaMedia;
                                candidatoAtual.PassouNoCurso = true;
                            }
                            else
                            {
                                int curso2 = candidatoAtual.Curso2;

                                if (curso2 >= 0 && curso2 < listaCursos.Count)
                                {
                                    var cursoSelecionado2 = listaCursos[curso2];

                                    if (cursoSelecionado2 != null &&
                                        cursoSelecionado2.CandidatosSelecionados != null &&
                                        cursoSelecionado2.QuantVagas >= cursoSelecionado2.CandidatosSelecionados.Count)
                                    {
                                        listaCursos.ElementAt(i).Value.FilaEspera.Inserir(candidatoAtual);

                                        // Adiciona o candidato ao segundo curso com desempate utilizando comparações simples
                                        AdicionarCandidatoComDesempate(cursoSelecionado2, candidatoAtual);
                                    }
                                    else
                                    {
                                        
                                        listaCursos.ElementAt(i).Value.FilaEspera.Inserir(candidatoAtual);
                                        cursoSelecionado2.FilaEspera.Inserir(candidatoAtual);
                                    }
                                }
                                else
                                {
                                    // Caso o índice do curso2 seja inválido
                                    Console.WriteLine($"Índice de curso2 inválido: {curso2}");
                                }
                            }
                        }
                    }
                }
            }

 
 */