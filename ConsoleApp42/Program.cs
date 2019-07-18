using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp42
{
    class Program
    {
        /// <summary>
        /// Матрица смежности графа который описывает возможные проезды между станциями
        /// </summary>
        static int?[,] graph;

        /// <summary>
        /// Массив для восстановления пути после алгоритма Дейкстры
        /// В каждой его ячейке - номер станции, которая являеться предпоследней в кратчайшем пути к данной станции
        /// </summary>
        static int[] ways;

        /// <summary>
        /// Двумерный массив для восстановления пути после алгоритма Флойда
        /// В каждой его ячейке - номер станции, которая являеться предпоследней в кратчайшем пути к данной станции
        /// </summary>
        static int?[,] Ways;

        static void Main(string[] args)
        {
            int N = int.Parse(Console.ReadLine());
            CreateGraph(N);
            OutputGraph();
            Console.WriteLine("Первый способ: алгоритм Дейкстры");
            int? FinalWeight=DijkstraAlgorithm();
            Console.WriteLine(FinalWeight);
            string FinalWay = RestoreTheWay(ways);
            Console.WriteLine(FinalWay);
            Console.WriteLine("Второй способ: алгоритс Флойда");
            Console.WriteLine(FloydAlgorithm());
            RestoreTheWay(0, N);
            Console.ReadKey();
        }
        static void CreateGraph(int N)
        {
            Random rnd = new Random();
            graph = new int?[N+1, N+1];
            for (int i = 0; i < graph.GetLength(0); i++)
                for (int j = i+1; j <graph.GetLength(1); j++)
                {
                    graph[i, j] = rnd.Next(20);
                }
        }
        static void OutputGraph()
        {
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                for (int j = 0; j < graph.GetLength(1); j++)
                  if(graph[i,j]!=null) Console.Write(graph[i, j]+"\t");
                    else Console.Write("null\t");
                Console.WriteLine();
            }
        }
        static int? DijkstraAlgorithm()
        {
            int?[] weight = new int?[graph.GetLength(1)];
            weight[0] = 0;
            for (int i = 1; i < weight.Length; i++)
                weight[i] = int.MaxValue;

            //Список вершин, для которых считаем проезд сейчас
            List<int> CurrentVertex = new List<int>();
            CurrentVertex.Add(0);

            ways = new int[graph.GetLength(1)];
            ways[0] = 0;

            //Алгоритм Дейкстры
            while (CurrentVertex.Count != 0)
            {
                    int i = CurrentVertex.IndexOf(CurrentVertex.Min());
                    for(int j = 1; j < graph.GetLength(1); j++)
                    {
                        if (graph[CurrentVertex[i], j] != null)
                        {
                            if(j!=graph.GetLength(1)-1)
                            CurrentVertex.Add(j);
                            if (weight[j] > graph[CurrentVertex[i], j] + weight[CurrentVertex[i]])
                            {
                                weight[j] = graph[CurrentVertex[i], j] + weight[CurrentVertex[i]];
                                ways[j] = CurrentVertex[i];
                            }
                        }
                    }
                    CurrentVertex.Remove(CurrentVertex[i]);
                   
            }

            return weight[weight.Length - 1];
            

        }
        static int? FloydAlgorithm()
        {
            Ways = new int?[graph.GetLength(0),graph.GetLength(1)];
            for (int i = 0; i < graph.GetLength(0); i++)
                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    if (graph[i, j] != null) Ways[i, j] = -1;
                }
            //Создаю отдельную матрицу, которая содержит вес ребер чтобы не изменять матрицу смежности
            int?[,] weight = new int?[graph.GetLength(0), graph.GetLength(1)];
            for (int i = 0; i < graph.GetLength(0); i++)
                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    int x;
                    if (graph[i, j] != null)
                    {
                        x = (int)graph[i, j];
                        weight[i, j] = x;
                    }
                    else weight[i, j] = null;
                }

            //Алгоритм Флойда
            for (int k = 0; k < weight.GetLength(0); k++)
                for (int i = 0; i < weight.GetLength(0); i++)
                    for (int j = 0; j < weight.GetLength(1); j++)
                        if (weight[i, j] > weight?[i, k] + weight?[k, j] && weight[i,j]!=null && weight[i,k]!=null && weight[k,j]!=null)
                        {
                            weight[i, j] = weight?[i, k] + weight?[k, j];
                            Ways[i, j] = k;
                        }
           
         return weight[0,weight.GetLength(1)-1];
        }
        static string RestoreTheWay(int[] ways)
        {
            int i = ways.Length - 1;
            List<string> way = new List<string>();
            way.Add(i.ToString());
            while (i != 0)
            {
                i = ways[i];
                way.Add(i.ToString());
            }
            string finalWay = "";
            for (int j = way.Count - 1; j >= 0; j--)
                finalWay += way[j]+"\t";
            return finalWay;
        }
        static void RestoreTheWay(int? x, int? y)
        {
            Console.Write(x+"\t");
            rec(x, y);
            
            void rec(int?x1, int? y1)
            {
                if (Ways[(int)x1, (int)y1] == -1 || Ways[(int)x1, (int)y1] == null || x1==null || y1 == null)
                {
                    Console.Write(y1 + "\t");
                    return;
                }
                int z = (int)Ways[(int)x1, (int)y1];
                rec(x1, z);
                rec(z, y1);
                return;
            }
        }
    }
}
