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
                    else if(countLinhas > quantCursos + 1 || countLinhas <= quantCursos + quantCandidatos + 1)
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


                curso.InstanciaObj(temp.Count);
                temp.CopyTo(listaCursos.ElementAt(i).Value.TodosCandidatos);

            }

            for (int i = 0; i < listaCursos.Count; i++)
            {

                Quicksort(listaCursos.ElementAt(i).Value.TodosCandidatos, 0, listaCursos.ElementAt(i).Value.TodosCandidatos.Length - 1);

                listaCursos[i].NotaDeCorte = listaCursos[i].TodosCandidatos[listaCursos[i].QuantVagas - 1].NotaMedia;              

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
                                c1 = listaCandidatos[i].NotaRedacao > listaCandidatos[i + 1].NotaRedacao;

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
                                c2 = listaCandidatos[i].NotaRedacao > listaCandidatos[i + 1].NotaRedacao;

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

                ExibirSaida(listaCursos);                

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
