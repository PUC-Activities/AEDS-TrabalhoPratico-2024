using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
                while (array[i].NotaMedia < pivo)
                    i++;
                while (array[j].NotaMedia > pivo)
                    j--;
                if (i <= j)
                {
                    {
                        double temp = array[i].NotaMedia;
                        array[i] = array[j];
                        array[j].NotaMedia = temp;

                        i++;
                        j--;
                    }
                }

                if (esq < j)
                    Quicksort(array, esq, j);
                if (i < dir)
                    Quicksort(array, i, dir);
            }
        }

        static void ExibirSaida(Dictionary<int, Curso> listaCursos)
        {
            for (int i = 0; i < listaCursos.Count; i++)
            {
                Console.WriteLine($"{listaCursos[i].NomeCurso} {listaCursos[i].NotaDeCorte}");
                Console.WriteLine("Selecionados");

                listaCursos[i].CandidatosSelecionados.Mostrar();
                listaCursos[i].FilaEspera.Mostrar();
            }
        }
        static void Main(string[] args)
        {
            int quantCursos = 0, quantCandidatos = 0, countLinhas = 1;
            string linhaArquivo;

            string[] informacoesArq = new string[6];

            Candidato aluno = new Candidato();
            Curso curso = new Curso();
            Dictionary<int, Curso> listaCursos = new Dictionary<int, Curso>();
            List<Candidato> listaCandidatos = new List<Candidato>();

            try
            {
                StreamReader arquivoEntrada = new StreamReader("entrada.txt", Encoding.UTF8);

                linhaArquivo = arquivoEntrada.ReadLine();

                while (linhaArquivo != null)
                {
                    informacoesArq = linhaArquivo.Split(';');

                    if (countLinhas == 1)
                    {
                        quantCursos = int.Parse(informacoesArq[0]);
                        quantCandidatos = int.Parse(informacoesArq[1]);                        
                    }

                    if(countLinhas > 1 && countLinhas < quantCursos)
                    {
                        aluno.Nome = informacoesArq[0];                     
                        aluno.NotaRedacao = int.Parse(informacoesArq[1]);
                        aluno.NotaLing = int.Parse(informacoesArq[2]);
                        aluno.NotaMat = int.Parse(informacoesArq[3]);
                        aluno.Curso1 = int.Parse(informacoesArq[4]);
                        aluno.Curso2 = int.Parse(informacoesArq[5]);
                        aluno.NotaMedia = Math.Round(((aluno.NotaRedacao + aluno.NotaLing + aluno.NotaMat) / 3), 2); 

                        listaCandidatos.Add(aluno);
                    }

                    if (countLinhas >= quantCursos)
                    {
                        if(countLinhas == quantCursos)
                        {
                            curso = new Curso(quantCandidatos);
                        }

                        curso.CodCurso = int.Parse(informacoesArq[0]);
                        curso.NomeCurso = informacoesArq[1];
                        curso.QuantVagas = int.Parse(informacoesArq[2]);

                        listaCursos.Add(curso.CodCurso, curso);
                    }
                   
                    linhaArquivo = arquivoEntrada.ReadLine();
                    countLinhas++;
                }
                arquivoEntrada.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine("Erro ao abrir o arquivo: " + e.Message);
            }

            for (int i = 0; i < listaCursos.Count; i++)
            {
                for (int j = 0; j < listaCandidatos.Count; j++)
                {
                    if (listaCursos[i].CodCurso == listaCandidatos[j].Curso1)
                    {
                        listaCursos[i].TodosCandidatos[i].Nome = listaCandidatos[j].Nome;
                    }

                    if (listaCursos[i].CodCurso == listaCandidatos[j].Curso2)
                    {
                        listaCursos[i].TodosCandidatos[i].Nome = listaCandidatos[j].Nome;
                    }
                }
            }

            for (int i = 0; i < listaCursos.Count; i++)
            {
                Quicksort(listaCursos[i].TodosCandidatos, 0, listaCursos[i].TodosCandidatos.Length);

                listaCursos[i].NotaDeCorte = listaCursos[i].TodosCandidatos[listaCursos[i].QuantVagas].NotaMedia;

            }

            for (int i = 0; i < listaCandidatos.Count; i++)
            {
                bool c1 = true, c2 = true;

                for (int j = 0; j < listaCursos.Count; j++)
                {
                    if (listaCandidatos[i].Curso1 == listaCursos[j].CodCurso)
                    {
                        if(listaCandidatos[i].NotaMedia < listaCursos[j].NotaDeCorte) { 
                            c1 = false;
                        }
                        else
                        {
                            if ((i + 1) <= (listaCandidatos.Count) && (listaCandidatos[i].NotaMedia == listaCandidatos[i + 1].NotaMedia))
                            {
                                listaCandidatos[i].NotaRedacao > listaCandidatos[i + 1].NotaRedacao ? c1 = true : c1 = false ;
                            }
                            
                        }
                    }
                    if (listaCandidatos[i].Curso2 == listaCursos[j].CodCurso)
                    {
                        if (listaCandidatos[i].NotaMedia < listaCursos[j].NotaDeCorte)
                        {
                            c2 = false;
                        }
                        else
                        {
                            if ((i + 1) <= (listaCandidatos.Count) && (listaCandidatos[i].NotaMedia == listaCandidatos[i + 1].NotaMedia))
                            {
                                listaCandidatos[i].NotaRedacao > listaCandidatos[i + 1].NotaRedacao ? c1 = true : c1 = false;
                            }
                        }
                    }

                }

                for (int h = 0; h < listaCursos.Count; h++)
                {
                    if (c1 == true && c2 == true)
                    {
                        if (listaCandidatos[i].Curso1 == listaCursos[h].CodCurso)
                        {
                            listaCursos[h].CandidatosSelecionados.Inserir(listaCandidatos[i]);
                        }
                    }
                    else if (c1 == true && c2 == false)
                    {
                        if (listaCandidatos[i].Curso1 == listaCursos[h].CodCurso)
                        {
                            listaCursos[h].CandidatosSelecionados.Inserir(listaCandidatos[i]);
                        }
                        else if (listaCandidatos[i].Curso2 == listaCursos[h].CodCurso)
                        {
                            listaCursos[i].FilaEspera.Inserir(listaCandidatos[i]);
                        }
                    }
                    else if (c1 == false && c2 == true)
                    {                        
                        if (listaCandidatos[i].Curso2 == listaCursos[h].CodCurso)
                        {
                            listaCursos[i].CandidatosSelecionados.Inserir(listaCandidatos[i]);
                        }
                        else if (listaCandidatos[i].Curso1 == listaCursos[h].CodCurso)
                        {
                            listaCursos[h].FilaEspera.Inserir(listaCandidatos[i]);
                        }
                    }
                    else if (c1 == false && c2 == false)
                    {
                        if (listaCandidatos[i].Curso1 == listaCursos[h].CodCurso)
                        {
                            listaCursos[h].FilaEspera.Inserir(listaCandidatos[i]);
                        }
                        else if (listaCandidatos[i].Curso2 == listaCursos[h].CodCurso)
                        {
                            listaCursos[h].FilaEspera.Inserir(listaCandidatos[i]);
                        }
                    }
                }                
            }

            try
            {
                StreamWriter arqSaida = new StreamWriter("saida.txt", false, Encoding.UTF8);

                linhaArquivo = arqSaida.ReadLine();

                while (linhaArquivo != null)
                {
                    ExibirSaida(listaCursos);
                }

                arqSaida.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao abrir ou criar o arquivo: " + ex.Message);
            }


            Console.ReadLine();
        }
    }
}
