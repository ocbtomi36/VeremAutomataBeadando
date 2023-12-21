using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace VeremAutomataBeadando
{
    internal class Automata
    {
        private int cikluslépteto = 0;
        private int karakterPozicio = 0;
        private string szalag;
        private bool elfogad = false;
        private bool error = false;
        private Stack<string> stack = new Stack<string>();
        private string[,] szabalyMatrix = new string[12, 7];
        private string alkalmazottSzabalyok = string.Empty;


        public Automata(string _input,string[,] _szabalyMatrix) 
        { 
            this.szalag = $"{_input}#";
            this.szabalyMatrix = _szabalyMatrix;
            this.stack.Push("#");
            this.stack.Push("E");
        }

        public string VeremTeto()
        {
            return this.stack.Peek();
        }
        public string Elemzes()
        {
            string eredmeny = string.Empty;
            int oszlopIndex = 0;
            int sorIndex = 0;
            string szabaly = string.Empty;
            string pattern = @"^[i(]"; // Regex minta az első karakterre

            Regex regex = new Regex(pattern);
            string input = szalag;

            if (regex.IsMatch(input))
            {
                elfogad = false;
            }
            else
            {
                eredmeny = "Az első karakter nem jó!";
                error = true;
            }
            while (elfogad != true && error != true)
            {
                
                //megkeresi, hogy hol van az aktuális szabály a táblázatban
                for (int j = 0; j < this.szabalyMatrix.GetLength(1); j++)
                {
                    if (this.szabalyMatrix[0, j] == szalag[karakterPozicio].ToString())
                    {
                        oszlopIndex = j;
                    }
                }
                for (int j = 0; j < this.szabalyMatrix.GetLength(0); j++)
                {
                    if (this.szabalyMatrix[j, 0] == this.stack.Peek())
                    {
                        sorIndex = j;
                    }
                }
                stack.Pop();
                // mátrixból kiveszi az aktuális körben a szabályt és feldarabolja karakterekre
                if (this.szabalyMatrix[sorIndex,oszlopIndex].Contains("|"))
                {
                    string[] matrixSzabalySeged = this.szabalyMatrix[sorIndex, oszlopIndex].Split('|');
                    if(matrixSzabalySeged.Length != 1)
                    {
                        switch (matrixSzabalySeged.Length)
                        {
                            case 2:
                                if (matrixSzabalySeged[0] == "eps")
                                {
                                    alkalmazottSzabalyok += matrixSzabalySeged[1];
                                }
                                else if (matrixSzabalySeged[0] == "(E)")
                                {
                                    string seged = matrixSzabalySeged[0];
                                    stack.Push(seged[2].ToString());
                                    stack.Push(seged[1].ToString());
                                    stack.Push(seged[0].ToString());
                                    alkalmazottSzabalyok += matrixSzabalySeged[1];
                                }
                                else
                                {
                                    stack.Push(matrixSzabalySeged[0]);
                                    alkalmazottSzabalyok += matrixSzabalySeged[1];
                                }

                                break;
                            case 3:
                                stack.Push(matrixSzabalySeged[1]);
                                stack.Push(matrixSzabalySeged[0]);
                                alkalmazottSzabalyok += matrixSzabalySeged[2];
                                break;
                            case 4:
                                stack.Push(matrixSzabalySeged[2]);
                                stack.Push(matrixSzabalySeged[1]);
                                stack.Push(matrixSzabalySeged[0]);
                                alkalmazottSzabalyok += matrixSzabalySeged[3];
                                break;
                            default:
                                break;
                        }
                    }
                    
                }
                else if (this.szabalyMatrix[sorIndex, oszlopIndex].Contains("pop"))
                {
                    Console.WriteLine("Pop művelet");
                    karakterPozicio++;
                }
                else if (this.szabalyMatrix[sorIndex, oszlopIndex].Contains("elfogad"))
                {
                    elfogad = true;
                    eredmeny = "Az automata elfogadta a beírt stringet!";
                }
                else if (this.szabalyMatrix[sorIndex, oszlopIndex].Contains("X"))
                {
                    error = true;
                    eredmeny = "Az automata nem fogadta el a beírt stringet.";
                }
                cikluslépteto++;
            }

            return eredmeny + " a " + cikluslépteto + " körben áll le.";
        }

    }
}
