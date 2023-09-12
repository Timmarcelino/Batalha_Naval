using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.IO;

namespace Batalha_Naval
// TRABALHO INTERDICIPLINAR 
{
    class Funcoes
    {
        static public int Altura = 60;
        static public int Largura = 80;
        static public string sCompBemVindo = "!     ";
        static public int Linhas;
        static public bool CancelarCampo = true;

        /* Tipos de dados digitados */
        public enum TipoDados
        {
            Texto = 0,
            Inteiro = 1,
            InteiroPositivo = 2,
            Real = 3,
            RealPositivo = 4,
            Data = 5,
            Hora = 6,
            DataHora = 7
        }

        static public string Duplicar(string S, int Tamanho)
        {
            string nS = S;
            while (S.Length < Tamanho) S = S + nS;
            return S;
        }

        static public void Escrever(string pLinha)
        {
            Console.CursorLeft = 0;
            Console.CursorVisible = false;
            if (pLinha.Length > Largura - 3) pLinha = pLinha.Substring(0, Largura - 3);
            pLinha = "  " + pLinha;
            Console.WriteLine(pLinha);
            Linhas++;
        }

        static public string AlinharEsquerda(string S, int Tamanho)
        {
            while (S.Length < Tamanho) S = S + " ";
            return S;
        }

        static public void CabecalhoTela()
        {
            Linhas = 0;
            Console.Clear();
            Console.CursorTop = 0;
            Console.CursorLeft = 1;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(Duplicar(" ", 78));
            Console.ResetColor();
            Linhas++;
            Console.CursorTop++;
            Console.CursorLeft = 1;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(AlinharEsquerda(" BATALHA NAVAL", 78));
            Console.ResetColor();
            Linhas++;
            Console.CursorTop++;
            Console.CursorLeft = 1;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(Duplicar(" ", 78));
            Console.ResetColor();
            Linhas++;
            Console.CursorTop++;
            Escrever("");
        }
        /* Cor do fundo para botão */
        static public void CorBotao()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        /* Cor do fundo para titulo */
        static public void CorTitulo()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /* Cor do fundo para header de grid */
        static public void CorHeaderGrid()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        /* Cor do campo */
        static public void CorCampo()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        /* Desenhar botão */
        static public void DesenharMenu(int Coluna, string Rotulo, string Descricao, bool IsPrimeiro)
        {
            if (!IsPrimeiro)
            {
                Console.CursorTop++;
                Console.CursorTop++;
            }
            Console.CursorLeft = Coluna;
            Funcoes.CorBotao();
            Console.Write(" <" + Rotulo + "> ");
            Console.ResetColor();
            Console.Write(" " + Descricao);
        }

        /* Recebe um valor digitado com um tamanho máximo indicado */
        static public string ReceberCampo(int Linha, int ColunaInicio, int Tamanho, TipoDados TipoDado)
        {
            string S = "";
            int I = 0;
            bool TeclaNaoPermitida = false;
            Console.CursorTop = Linha;
            Console.CursorLeft = ColunaInicio;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(Duplicar(" ", Tamanho));
            Console.CursorLeft = ColunaInicio;
            CancelarCampo = true;
            //I = ValorPadrao.Length;
            //if (I > 0)
            //{
            //    Console.Write(ValorPadrao);
            //    S = ValorPadrao;
            //    if (I == Tamanho) Console.CursorLeft--;
            //}
            Console.CursorVisible = true;
            ConsoleKeyInfo K = new ConsoleKeyInfo();
            while (I < Tamanho + 1)
            {
                CancelarCampo = false;
                TeclaNaoPermitida = false;
                K = Console.ReadKey(true);
                //Confirmação do que foi digitado 
                if ((K.Key == ConsoleKey.Enter) || (K.Key == ConsoleKey.Escape))
                {
                    I = Tamanho + 1;
                    //Cancelar com ESC  
                    if (K.Key == ConsoleKey.Escape)
                        CancelarCampo = true;
                    else
                    {
                        //Verifica tipo de dados 
                        try
                        {
                            switch (TipoDado)
                            {
                                //Números positivos 
                                case TipoDados.Inteiro:
                                case TipoDados.InteiroPositivo: S = String.Format("{0:" + Duplicar("0", Tamanho) + "}", Convert.ToInt64(S)); break;
                            }
                        }
                        catch
                        {
                            S = "";
                        }
                    }
                }
                else
                {
                    //Apagar digitação 
                    if ((K.Key == ConsoleKey.Backspace) || (K.Key == ConsoleKey.UpArrow) || (K.Key == ConsoleKey.LeftArrow))
                    {
                        if (S.Length > 0)
                        {
                            S = S.Substring(0, S.Length - 1);
                            if (S.Length < Tamanho - 1)
                            {
                                Console.CursorLeft--;
                                Console.Write(" ");
                                Console.CursorLeft--;
                            }
                            else
                            {
                                Console.Write(" ");
                            }
                        }
                    }
                    else
                    {
                        //Verifica tipos de dados 
                        switch (TipoDado)
                        {
                            case TipoDados.InteiroPositivo:
                                TeclaNaoPermitida = (((K.Key < ConsoleKey.NumPad0) || (K.Key > ConsoleKey.NumPad9)) && ((K.Key < ConsoleKey.D0) || (K.Key > ConsoleKey.D9)));
                                break;
                            default:
                                TeclaNaoPermitida = (((K.Key < ConsoleKey.NumPad0) || (K.Key > ConsoleKey.NumPad9)) && ((K.Key < ConsoleKey.D0) || (K.Key > ConsoleKey.D9)) && ((K.Key < ConsoleKey.A) || (K.Key > ConsoleKey.Z)) && (K.Key != ConsoleKey.Spacebar));
                                break;
                        }
                        if (!((S.Length == Tamanho) || TeclaNaoPermitida))
                        {
                            S = S + K.KeyChar.ToString();
                        }
                    }
                    I = S.Length;
                    Console.CursorLeft = ColunaInicio;
                    Console.Write(S);
                    if (S.Length == Tamanho) Console.CursorLeft--;
                }
            }
            Console.ResetColor();
            return S.Trim();
        }

        /* Janela com título */
        static public void JanelaTitulo(string Titulo)
        {
            Funcoes.CabecalhoTela();
            //Titulo da janela 
            Console.CursorLeft = 2;
            Funcoes.CorTitulo();
            Console.Write("= " + Titulo.ToUpper().Trim() + " " + Funcoes.Duplicar("=", 73 - Titulo.Trim().Length));
            Console.ResetColor();
            //Dá espaço 
            Console.CursorTop++;
            Funcoes.Escrever("");
            Console.CursorLeft = 0;
        }

        /* Desenhar campo */
        static public void DesenharCampo(string Nome, int Linha, int ColunaLabel, int ColunaCampo, int Tamanho)
        {
            Console.CursorTop = Linha;
            Console.CursorLeft = ColunaLabel;
            if (Nome.Length > 0) Console.Write(Nome + ":");
            Console.BackgroundColor = ConsoleColor.White;
            Console.CursorLeft = ColunaCampo;
            Console.Write(Funcoes.Duplicar(" ", Tamanho));
            Console.ResetColor();
        }

        /* Desenhar botão */
        static public void DesenharBotao(string Rotulo, int Linha, int Coluna)
        {
            CorBotao();
            Console.CursorTop = Linha;
            Console.CursorLeft = Coluna;
            Console.Write(" " + Rotulo + " ");
            Console.ResetColor();
            Console.CursorVisible = false;
        }

        /* Verifica se o campo foi digitado */
        static public bool Recebeu()
        {
            bool Retorno = !CancelarCampo;
            CancelarCampo = true;
            return Retorno;
        }

        /* Função de confirmação */
        static public bool Confirmar(string Pergunta, int Linha, int Coluna)
        {
            Console.CursorTop = Linha;
            Console.CursorLeft = Coluna;
            Console.WriteLine(Pergunta + "?");
            int LinhaBotao = Console.CursorTop + 1;
            DesenharBotao("<S> = Sim", LinhaBotao, Coluna);
            DesenharBotao("<N> = Nao", LinhaBotao, Coluna + 13);
            //Ler teclas 
            ConsoleKeyInfo K = Console.ReadKey(true);
            while ((K.Key != ConsoleKey.N) && (K.Key != ConsoleKey.S)) K = Console.ReadKey(true);
            if (K.Key == ConsoleKey.N)
            {
                //Apagar mensagem 
                Console.CursorTop = Linha;
                Console.CursorLeft = Coluna;
                Console.WriteLine(Duplicar(" ", Pergunta.Length + 1));
                Escrever("");
                Console.CursorLeft = Coluna;
                Console.WriteLine(Duplicar(" ", 25));
                return false;
            }
            else
                return true;
        }

        /* Escrever mensagem */
        static public void EscreverMensagem(string Mensagem, int Linha, int Coluna)
        {
            Console.CursorTop = Linha;
            Console.CursorLeft = Coluna;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("- " + Mensagem);
            Console.ResetColor();
        }

        /* Funcao para apagar células */
        static public void Apagar(int Linha, int Coluna, int Tamanho)
        {
            Console.CursorTop = Linha;
            Console.CursorLeft = Coluna;
            Console.ResetColor();
            Console.Write(Duplicar(" ", Tamanho));
        }
    }
    class Naval
    {
        static public int Inicio;
        static public String Player;
        static public int[,] M_Player = new int[16, 16];
        static public int[,] M_Pc = new int[16, 16];

        public Naval()
        {
            for (int l = 0; l < 16; l++)
                for (int c = 0; c < 16; c++)
                {
                    M_Player[l, c] = 0;
                    M_Pc[l, c] = 0;
                }
        }

        static public void Tela_SelectShips()
        {
            Funcoes.CabecalhoTela();
            Console.CursorTop = 1;
            Console.CursorLeft = 15;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(" - SELECIONE AS COORDENADAS DOS SEUS BARCOS");
            Console.ResetColor();
            Console.CursorTop = 4;
            if (Player == null)
                Player = " Anonimo";
            int i;
            Inicio = 20; i = 1;
            Console.CursorLeft = Inicio + 2; Console.WriteLine(" A B C D E F G H I J K L M N O");
            Console.CursorLeft = Inicio + 2; Console.WriteLine(" _ _ _ _ _ _ _ _ _ _ _ _ _ _ _");
            while (i < 16)
            {
                Console.ResetColor();
                Console.CursorLeft = 1;
                if (i == 7) Console.Write("{0}", Player);
                Console.CursorLeft = Inicio;
                Console.Write("{0,2}", i);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("|_|_|_|_|_|_|_|_|_|_|_|_|_|_|_|");
                i++;
            }
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.CursorTop = 23; Console.CursorLeft = 6; Console.Write("1");
            Console.CursorTop = 25; Console.CursorLeft = 6; Console.Write("22");
            Console.CursorTop = 27; Console.CursorLeft = 6; Console.Write("333");
            Console.CursorTop = 29; Console.CursorLeft = 6; Console.Write("4444");
            Console.CursorTop = 31; Console.CursorLeft = 6; Console.Write("55555");
            Console.ResetColor();
            Console.CursorTop = 23; Console.CursorLeft = 4; Console.Write("1.");
            Console.CursorTop = 25; Console.CursorLeft = 4; Console.Write("2.");
            Console.CursorTop = 27; Console.CursorLeft = 4; Console.Write("3.");
            Console.CursorTop = 29; Console.CursorLeft = 4; Console.Write("4.");
            Console.CursorTop = 31; Console.CursorLeft = 4; Console.Write("5.");

            /*Funcoes.DesenharBotao("<Enter> Selecionar", 24, 24);
            Funcoes.DesenharBotao("   Manualmente    ", 25, 24);
            Funcoes.DesenharBotao("<F3>  Selecionar  ", 27, 24);
            Funcoes.DesenharBotao("   Aleatoriamente ", 28, 24);
            Funcoes.DesenharBotao("<ESC> Voltar Tela ", 30, 24);*/

            /*ConsoleKeyInfo S = Console.ReadKey();
            if (S.Key == ConsoleKey.F3)
            {
                Console.CursorTop = 23; Console.CursorLeft = 23; Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("Selecionar Aleatoriamente");
                Thread.Sleep(2000);
                Console.ResetColor();
            }
            if (S.Key == ConsoleKey.Escape)
            {
                Console.CursorTop = 23; Console.CursorLeft = 23; Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("Tela Inicial");
                Console.ResetColor();
            }
            if (S.Key == ConsoleKey.Enter)
            {
                Console.CursorTop = 23; Console.CursorLeft = 23; Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("Digite abaixo as opcoes:");
                
                Console.ResetColor();
            }

            Console.ResetColor(); Console.CursorTop = 24; Console.CursorLeft = 24; Console.Write("\t\t\t");
            Console.ResetColor(); Console.CursorTop = 25; Console.CursorLeft = 24; Console.Write("\t\t\t");
            Console.ResetColor(); Console.CursorTop = 27; Console.CursorLeft = 24; Console.Write("\t\t\t");
            Console.ResetColor(); Console.CursorTop = 28; Console.CursorLeft = 24; Console.Write("\t\t\t");
            Console.ResetColor(); Console.CursorTop = 30; Console.CursorLeft = 24; Console.Write("\t\t\t");
            */
        }
        static public void SelectShips(ref int[,] Selection)
        {
            Selection = new int[16, 16];
            Coordenadas Batalha = new Coordenadas();

            int Pos, L, C;
            int[] Barcos = new int[6];
            Barcos[0] = 0; Barcos[1] = 0; Barcos[2] = 0; Barcos[3] = 0; Barcos[4] = 0; Barcos[5] = 0;
            do
            {
                Funcoes.DesenharCampo("Escolha Seu Barco    ", 40, 8, 30, 4);
                int TpBarco = Convert.ToInt32(Funcoes.ReceberCampo(40, 31, 2, 0));
                if (TpBarco == 1) Console.CursorTop = 23; Console.CursorLeft = 2; Console.Write("->");
                if (TpBarco == 2) Console.CursorTop = 25; Console.CursorLeft = 2; Console.Write("->");
                if (TpBarco == 3) Console.CursorTop = 27; Console.CursorLeft = 2; Console.Write("->");
                if (TpBarco == 4) Console.CursorTop = 29; Console.CursorLeft = 2; Console.Write("->");
                if (TpBarco == 5) Console.CursorTop = 31; Console.CursorLeft = 2; Console.Write("->");
                if (Barcos[TpBarco] == 0)
                {
                    do
                    {
                        Console.CursorTop = 40; Console.CursorLeft = 1; Console.Write("\t\t\t\t\t\t\t\t\t\t");
                        Funcoes.DesenharCampo("Escolha a Coordenada ", 40, 8, 30, 4);
                        string Coord = Funcoes.ReceberCampo(40, 31, 3, 0);

                        if (TpBarco == 1)
                            Pos = 1;
                        else
                        {
                            Funcoes.DesenharCampo("Horizontal<1> / Vertical<2> ", 40, 8, 38, 4);
                            Pos = Convert.ToInt32(Funcoes.ReceberCampo(40, 39, 1, 0));
                        }
                        if (!Vazio(Coord, Pos, TpBarco, Selection))
                        {
                            //Console.CursorLeft = 40;
                            Console.Write("              Posicao ja ocupada"); Thread.Sleep(2000);

                        }
                        else
                        {
                            Batalha.ins_barco(TpBarco, Coord, Pos, ref Selection, Selection);

                            Console.CursorTop = 5;
                            Console.BackgroundColor = ConsoleColor.Blue;
                            for (L = 1; L < 16; L++)
                            {
                                Console.Write("\n");
                                Console.CursorLeft = 23;
                                for (C = 1; C < 16; C++)
                                {
                                    if (Selection[L, C] != 0)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.BackgroundColor = ConsoleColor.Green;
                                        Console.Write("{0}", Selection[L, C]);
                                        Console.ForegroundColor = ConsoleColor.Gray;
                                        Console.BackgroundColor = ConsoleColor.Blue;
                                        Console.Write("|");
                                    }
                                    else
                                    {
                                        Console.BackgroundColor = ConsoleColor.Blue;
                                        Console.ForegroundColor = ConsoleColor.Gray;
                                        Console.Write("_|", Selection[L, C]);
                                    }
                                }
                            }
                            Console.ResetColor();
                            Barcos[TpBarco] = TpBarco;
                            Barcos[0]++;
                            Console.ResetColor();
                        }
                    } while (Barcos[TpBarco] == 0);

                }
                else
                {
                    Console.CursorTop = 39; Console.Write("                                                     ");
                    Console.CursorLeft = 42; Console.Write("Barco ja escolhido ");
                }
                Console.CursorTop = 40; Console.CursorLeft = 1; Console.Write("\t\t\t\t\t\t\t\t\t\t");
            } while (Barcos[0] < 5);

            Console.Clear();
            return;
        }
        static public bool Vazio(string Coordenada, int Hor_Ver, int TpBarco, int[,] Matriz)
        {
            Coordenadas Batalha = new Coordenadas();
            int L = 0, C = 0;
            Batalha.SetCoord(Coordenada, ref L, ref C);
            bool Vazio = true;
            switch (Hor_Ver)
            {
                case 1:
                    switch (TpBarco)
                    {
                        case 1:
                            if (Matriz[L, C] == 0)
                                Vazio = true;
                            else Vazio = false;
                            break;
                        case 2:
                            C = C + 1;
                            if (Matriz[L, C - 1] == 0 && Matriz[L, C] == 0)
                                Vazio = true;
                            else Vazio = false;
                            break;
                        case 3:
                            C = C + 2;
                            if (Matriz[L, C - 2] == 0 && Matriz[L, C - 1] == 0 && Matriz[L, C] == 0)
                                Vazio = true;
                            else Vazio = false;
                            break;
                        case 4:
                            C = C + 3;
                            if (Matriz[L, C - 3] == 0 && Matriz[L, C - 2] == 0 && Matriz[L, C - 1] == 0 && Matriz[L, C] == 0)
                                Vazio = true;
                            else Vazio = false;
                            break;
                        case 5:
                            C = C + 4;
                            if (Matriz[L, C - 4] == 0 && Matriz[L, C - 3] == 0 && Matriz[L, C - 2] == 0 && Matriz[L, C - 1] == 0 && Matriz[L, C] == 0)
                                Vazio = true;
                            else Vazio = false;
                            break;
                    }
                    break;
                case 2:
                    switch (TpBarco)
                    {
                        case 1:
                            if (Matriz[L, C] == 0)
                                Vazio = true;
                            else Vazio = false;
                            break;
                        case 2:
                            L = L + 1;
                            if (Matriz[L - 1, C] == 0 && Matriz[L, C] == 0)
                                Vazio = true;
                            else Vazio = false;
                            break;
                        case 3:
                            L = L + 2;
                            if (Matriz[L - 2, C] == 0 && Matriz[L - 1, C] == 0 && Matriz[L, C] == 0)
                                Vazio = true;
                            else Vazio = false;
                            break;
                        case 4:
                            L = L + 3;
                            if (Matriz[L - 3, C] == 0 && Matriz[L - 2, C] == 0 && Matriz[L - 1, C] == 0 && Matriz[L, C] == 0)
                                Vazio = true;
                            else Vazio = false;
                            break;
                        case 5:
                            L = L + 4;
                            if (Matriz[L - 4, C] == 0 && Matriz[L - 3, C] == 0 && Matriz[L - 2, C] == 0 && Matriz[L - 1, C] == 0 && Matriz[L, C] == 0)
                                Vazio = true;
                            else Vazio = false;
                            break;
                    }
                    break;
            }
            return Vazio;

        }
        static public void Tela_Jogo()
        {
            Funcoes.CabecalhoTela();
            if (Player == null)
                Player = " Anonimo";
            int i;
            Inicio = 20; i = 1;
            Console.CursorLeft = Inicio + 2; Console.WriteLine(" A B C D E F G H I J K L M N O P");
            Console.CursorLeft = Inicio + 2; Console.WriteLine(" _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _");
            while (i < 16)
            {
                Console.ResetColor();
                Console.CursorLeft = 1;
                if (i == 7) Console.Write("{0}", Player);
                Console.CursorLeft = Inicio;
                Console.Write("{0,2}", i);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("|_|_|_|_|_|_|_|_|_|_|_|_|_|_|_|_|");
                i++;
            }
            Console.ResetColor();

            Console.WriteLine("\n\n\n");

            Inicio = 20; i = 1;
            Console.CursorLeft = Inicio + 2; Console.WriteLine(" A B C D E F G H I J K L M N O P");
            Console.CursorLeft = Inicio + 2; Console.WriteLine(" _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _");
            while (i < 16)
            {
                Console.ResetColor();
                Console.CursorLeft = 1;
                if (i == 7) Console.Write(" COMPUTADOR");
                Console.CursorLeft = Inicio;
                Console.Write("{0,2}", i);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("|_|_|_|_|_|_|_|_|_|_|_|_|_|_|_|_|");
                i++;
            }
            Console.ResetColor();
            Funcoes.DesenharBotao("  <ESC>  ", 22, 59);
            Funcoes.DesenharBotao("Para sair", 23, 59);
            ConsoleKeyInfo S = Console.ReadKey(true);
            while (S.Key != ConsoleKey.Escape) S = Console.ReadKey(true);
            return;
        }

    }
    class Coordenadas
    {
        //public int Linha;
        //public int Coluna;
        public int L, C;
        private string Col;
        private string S;

        public void SetCoord(string Q, ref int Linha, ref int Coluna)
        {
            S = Q;
            int COL = 0;
            if (TestCoord())
            {
                if (S.Length == 2)
                {
                    if (S[0] >= '0' && S[0] <= '9')
                    {
                        Linha = (Convert.ToInt32(S[0]) - 48);
                        Col = S[1].ToString();
                        Converte_Coluna(Col, ref COL);
                        Coluna = COL;
                    }
                    else
                    {
                        Col = S[0].ToString();
                        Converte_Coluna(Col, ref COL);
                        Coluna = COL;
                        Linha = (Convert.ToInt32(S[1]) - 48);
                    }
                }
                if (S.Length == 3)
                {
                    if (S[0] >= '0' && S[0] <= '9')
                    {
                        StringBuilder S1 = new StringBuilder();
                        S1.Append(S[0]);
                        S1.Append(S[1]);
                        Linha = Convert.ToInt32(S1.ToString());
                        Col = Convert.ToString(S[2]);
                        Converte_Coluna(Col, ref COL);
                        Coluna = COL;
                    }
                    else
                        if (S[1] >= '0' && S[1] <= '9')
                        {
                            StringBuilder S1 = new StringBuilder();
                            S1.Append(S[1]);
                            S1.Append(S[2]);
                            Linha = Convert.ToInt32(S1.ToString());
                            Col = Convert.ToString(S[0]);
                            Converte_Coluna(Col, ref COL);
                            Coluna = COL;
                        }
                }
            }
        }

        public void EscreverCoord(ref int Linha, ref int Coluna)
        {
            do
            {
                Console.Write("Digite a COORDENADA: ");
                S = Console.ReadLine();
                if (!TestCoord())
                {
                    Console.Write("Coodernadas Invalida.");
                    Thread.Sleep(2000);
                }
            } while (!TestCoord());
            SetCoord(S, ref L, ref C);
            Linha = L; Coluna = C;
            return;
        }

        public void Converte_Coluna(string Col, ref int Coluna)
        {
            if (Col == "A" || Col == "a") Coluna = 1;
            if (Col == "B" || Col == "b") Coluna = 2;
            if (Col == "C" || Col == "c") Coluna = 3;
            if (Col == "D" || Col == "d") Coluna = 4;
            if (Col == "E" || Col == "e") Coluna = 5;
            if (Col == "F" || Col == "f") Coluna = 6;
            if (Col == "G" || Col == "g") Coluna = 7;
            if (Col == "H" || Col == "h") Coluna = 8;
            if (Col == "I" || Col == "i") Coluna = 9;
            if (Col == "J" || Col == "j") Coluna = 10;
            if (Col == "K" || Col == "k") Coluna = 11;
            if (Col == "L" || Col == "l") Coluna = 12;
            if (Col == "M" || Col == "m") Coluna = 13;
            if (Col == "N" || Col == "n") Coluna = 14;
            if (Col == "O" || Col == "o") Coluna = 15;
        }
        public bool TestCoord()
        {
            bool Verifica = false;
            if (S.Length > 3 || S.Length == 1) Verifica = false;
            if (S.Length == 2)
                if (S[0] >= '0' && S[0] <= '9')
                    if (S[1] >= '0' && S[1] <= '9') Verifica = false;
                    else
                        if (S[0] == '0') Verifica = false;
                        else
                        {
                            if (S[1] == 'A' || S[1] == 'a' || S[1] == 'B' || S[1] == 'b' || S[1] == 'C' || S[1] == 'c' || S[1] == 'D' || S[1] == 'd' || S[1] == 'E' || S[1] == 'e' || S[1] == 'F' || S[1] == 'f' || S[1] == 'G' || S[1] == 'g' || S[1] == 'H' || S[1] == 'h' || S[1] == 'I' || S[1] == 'i' || S[1] == 'J' || S[1] == 'j' || S[1] == 'K' || S[1] == 'k' || S[1] == 'L' || S[1] == 'l' || S[1] == 'M' || S[1] == 'm' || S[1] == 'N' || S[1] == 'n' || S[1] == 'O' || S[1] == 'o')
                                Verifica = true;
                            else
                                Verifica = false;
                        }
                else
                    if (S[1] >= '0' && S[1] <= '9')
                        if (S[1] == '0') Verifica = false;
                        else
                        {
                            if (S[0] == 'A' || S[0] == 'a' || S[0] == 'B' || S[0] == 'b' || S[0] == 'C' || S[0] == 'c' || S[0] == 'D' || S[0] == 'd' || S[0] == 'E' || S[0] == 'e' || S[0] == 'F' || S[0] == 'f' || S[0] == 'G' || S[0] == 'g' || S[0] == 'H' || S[0] == 'h' || S[0] == 'I' || S[0] == 'i' || S[0] == 'J' || S[0] == 'j' || S[0] == 'K' || S[0] == 'k' || S[0] == 'L' || S[0] == 'l' || S[0] == 'M' || S[0] == 'm' || S[0] == 'N' || S[0] == 'n' || S[0] == 'O' || S[0] == 'o')
                                Verifica = true;
                            else
                                Verifica = false;
                        }
                    else Verifica = false;

            if (S.Length == 3)
                if (S[0] >= '0' && S[0] <= '9')
                    if (S[1] >= '0' && S[1] <= '9')
                        if ((S[0] == '0' && S[1] == '0') || (S[0] >= '1' && S[1] > '5')) Verifica = false;
                        else
                        {
                            if (S[2] == 'A' || S[2] == 'a' || S[2] == 'B' || S[2] == 'b' || S[2] == 'C' || S[2] == 'c' || S[2] == 'D' || S[2] == 'd' || S[2] == 'E' || S[2] == 'e' || S[2] == 'F' || S[2] == 'f' || S[2] == 'G' || S[2] == 'g' || S[2] == 'H' || S[2] == 'h' || S[2] == 'I' || S[2] == 'i' || S[2] == 'J' || S[2] == 'j' || S[2] == 'K' || S[2] == 'k' || S[2] == 'L' || S[2] == 'l' || S[2] == 'M' || S[2] == 'm' || S[2] == 'N' || S[2] == 'n' || S[2] == 'O' || S[2] == 'o')
                                Verifica = true;
                            else
                                Verifica = false;
                        }
                    else Verifica = false;
                else
                    if (S[1] >= '0' && S[1] <= '9')
                        if (S[2] >= '0' && S[2] <= '9')
                            if ((S[1] == '0' && S[2] == '0') || (S[1] >= '1' && S[2] > '5')) Verifica = false;
                            else
                            {
                                if (S[0] == 'A' || S[0] == 'a' || S[0] == 'B' || S[0] == 'b' || S[0] == 'C' || S[0] == 'c' || S[0] == 'D' || S[0] == 'd' || S[0] == 'E' || S[0] == 'e' || S[0] == 'F' || S[0] == 'f' || S[0] == 'G' || S[0] == 'g' || S[0] == 'H' || S[0] == 'h' || S[0] == 'I' || S[0] == 'i' || S[0] == 'J' || S[0] == 'j' || S[0] == 'K' || S[0] == 'k' || S[0] == 'L' || S[0] == 'l' || S[0] == 'M' || S[0] == 'm' || S[0] == 'N' || S[0] == 'n' || S[0] == 'O' || S[0] == 'o')
                                    Verifica = true;
                                else
                                    Verifica = false;
                            }
                        else Verifica = false;
            return Verifica;
        }
        public bool Test_Ocupado(int L, int C, int[,] Matriz)
        {
            bool Ocupado = false;
            if (Matriz[L, C] != 0)
                Ocupado = true;
            else
                Ocupado = false;
            return Ocupado;
        }

        public bool Test_limite(int L, int C)
        {
            bool verifica = true;
            if (L == 0 || L > 15) verifica = false;
            if (C == 0 || C > 15) verifica = false;
            return verifica;
        }

        public void ins_barco(int Tipo_Barco, string Coordenada, int Ver_hor, ref int[,] Matriz, int[,] M)
        {
            SetCoord(Coordenada, ref L, ref C);
            switch (Ver_hor)
            {
                case 1:
                    switch (Tipo_Barco)
                    {
                        case 1:
                            if (!Test_limite(L, C)) Console.Write("Posicao Invalida");
                            else
                                if (!Test_Ocupado(L, C, M)) Matriz[L, C] = 1;
                                else Console.Write("Posicao Ocupada");
                            break;
                        case 2:
                            if (!Test_limite(L, C)) Console.Write("Posicao Invalida");
                            C = C + 1;
                            if (!Test_limite(L, C))
                                Console.Write("Posicao Invalida");
                            else
                                if (!Test_Ocupado(L, C, M) || !Test_Ocupado(L, C - 1, M))
                                { Matriz[L, C - 1] = 2; Matriz[L, C] = 2; }
                                else Console.Write("Posicao Ocupada");
                            break;
                        case 3:
                            if (!Test_limite(L, C)) Console.Write("Posicao Invalida");
                            C = C + 2;
                            if (!Test_limite(L, C))
                                Console.Write("Posicao Invalida");
                            else
                                if (!Test_Ocupado(L, C, M) || !Test_Ocupado(L, C - 2, M) || !Test_Ocupado(L, C - 1, M))
                                { Matriz[L, C - 2] = 3; Matriz[L, C - 1] = 3; Matriz[L, C] = 3; }
                                else Console.Write("Posicao Ocupada");
                            break;
                        case 4:
                            if (!Test_limite(L, C)) Console.Write("Posicao Invalida");
                            C = C + 3;
                            if (!Test_limite(L, C))
                                Console.Write("Posicao Invalida");
                            else
                                if (!Test_Ocupado(L, C, M) || !Test_Ocupado(L, C - 3, M) || !Test_Ocupado(L, C - 2, M) || !Test_Ocupado(L, C - 1, M))
                                { Matriz[L, C - 3] = 4; Matriz[L, C - 2] = 4; Matriz[L, C - 1] = 4; Matriz[L, C] = 4; }
                                else Console.Write("Posicao Ocupada");
                            break;
                        case 5:
                            if (!Test_limite(L, C)) Console.Write("Posicao Invalida");
                            C = C + 4;
                            if (!Test_limite(L, C))
                                Console.Write("Posicao Invalida");
                            else
                                if (!Test_Ocupado(L, C, M) || !Test_Ocupado(L, C - 4, M) || !Test_Ocupado(L, C - 3, M) || !Test_Ocupado(L, C - 2, M) || !Test_Ocupado(L, C - 1, M))
                                { Matriz[L, C - 4] = 5; Matriz[L, C - 3] = 5; Matriz[L, C - 2] = 5; Matriz[L, C - 1] = 5; Matriz[L, C] = 5; }
                                else Console.Write("Posicao Ocupada");
                            break;
                    }
                    break;
                case 2:
                    switch (Tipo_Barco)
                    {
                        case 1:
                            if (!Test_limite(L, C)) Console.Write("Posicao Invalida");
                            else
                                if (!Test_Ocupado(L, C, M))
                                    Matriz[L, C] = 1;
                                else Console.Write("Posicao Ocupada");
                            break;
                        case 2:
                            if (!Test_limite(L, C)) Console.Write("Posicao Invalida");
                            L = L + 1;
                            if (!Test_limite(L, C))
                                Console.Write("Posicao Invalida");
                            else
                                if (!Test_Ocupado(L - 1, C, M) || !Test_Ocupado(L, C, M))
                                { Matriz[L - 1, C] = 2; Matriz[L, C] = 2; }
                                else Console.Write("Posicao Ocupada");
                            break;
                        case 3:
                            if (!Test_limite(L, C)) Console.Write("Posicao Invalida");
                            L = L + 2;
                            if (!Test_limite(L, C))
                                Console.Write("Posicao Invalida");
                            else
                                if (!Test_Ocupado(L - 2, C, M) || !Test_Ocupado(L - 1, C, M) || !Test_Ocupado(L, C, M))
                                { Matriz[L - 2, C] = 3; Matriz[L - 1, C] = 3; Matriz[L, C] = 3; }
                                else Console.Write("Posicao Ocupada");
                            break;
                        case 4:
                            if (!Test_limite(L, C)) Console.Write("Posicao Invalida");
                            L = L + 3;
                            if (!Test_limite(L, C))
                                Console.Write("Posicao Invalida");
                            else
                                if (!Test_Ocupado(L - 3, C, M) || !Test_Ocupado(L - 2, C, M) || !Test_Ocupado(L - 1, C, M) || !Test_Ocupado(L, C, M))
                                { Matriz[L - 3, C] = 4; Matriz[L - 2, C] = 4; Matriz[L - 1, C] = 4; Matriz[L, C] = 4; }
                                else Console.Write("Posicao Ocupada");
                            break;
                        case 5:
                            if (!Test_limite(L, C)) Console.Write("Posicao Invalida");
                            L = L + 4;
                            if (!Test_limite(L, C))
                                Console.Write("Posicao Invalida");
                            else
                                if (!Test_Ocupado(L - 4, C, M) || !Test_Ocupado(L - 3, C, M) || !Test_Ocupado(L - 2, C, M) || !Test_Ocupado(L - 1, C, M) || !Test_Ocupado(L, C, M))
                                { Matriz[L - 4, C] = 5; Matriz[L - 3, C] = 5; Matriz[L - 2, C] = 5; Matriz[L - 1, C] = 5; Matriz[L, C] = 5; }
                                else Console.Write("Posicao Ocupada");
                            break;
                    }
                    break;


            }
        }
        public void ins_barco(int Tipo_Barco, int Linha, int Coluna, int Ver_hor, ref int[,] Matriz, int[,] M)
        {
            L = Linha; C = Coluna;
            switch (Ver_hor)
            {
                case 1:
                    switch (Tipo_Barco)
                    {
                        case 1:
                            Matriz[L, C] = 1;
                            break;
                        case 2:
                            C = C + 1;
                            Matriz[L, C - 1] = 2; Matriz[L, C] = 2;
                            break;
                        case 3:
                            C = C + 2;
                            Matriz[L, C - 2] = 3; Matriz[L, C - 1] = 3; Matriz[L, C] = 3;
                            break;
                        case 4:

                            C = C + 3;
                            Matriz[L, C - 3] = 4; Matriz[L, C - 2] = 4; Matriz[L, C - 1] = 4; Matriz[L, C] = 4;
                            break;
                        case 5:
                            C = C + 4;
                            Matriz[L, C - 4] = 5; Matriz[L, C - 3] = 5; Matriz[L, C - 2] = 5; Matriz[L, C - 1] = 5; Matriz[L, C] = 5;
                            break;
                    }
                    break;
                case 2:
                    switch (Tipo_Barco)
                    {
                        case 1:
                            Matriz[L, C] = 1;
                            break;
                        case 2:
                            L = L + 1;
                            Matriz[L - 1, C] = 2; Matriz[L, C] = 2;
                            break;
                        case 3:
                            L = L + 2;
                            Matriz[L - 2, C] = 3; Matriz[L - 1, C] = 3; Matriz[L, C] = 3;
                            break;
                        case 4:
                            L = L + 3;
                            Matriz[L - 3, C] = 4; Matriz[L - 2, C] = 4; Matriz[L - 1, C] = 4; Matriz[L, C] = 4;
                            break;
                        case 5:
                            L = L + 4;
                            Matriz[L - 4, C] = 5; Matriz[L - 3, C] = 5; Matriz[L - 2, C] = 5; Matriz[L - 1, C] = 5; Matriz[L, C] = 5;
                            break;
                    }
                    break;
            }
        }

        public void Select_Random(ref int[,] Selection)
        {
            Selection = new int[16, 16];
            Coordenadas Batalha = new Coordenadas();
            Random r = new Random();
            int Pos, L = 0, C = 0;
            int[] Barcos = new int[6];
            Barcos[0] = 0; Barcos[1] = 0; Barcos[2] = 0; Barcos[3] = 0; Barcos[4] = 0; Barcos[5] = 0;
            do
            {
                int TpBarco = r.Next(1, 6); // escolhe o tipo de barco 
                //Console.WriteLine("TpBarco: {0}", TpBarco);

                if (Barcos[TpBarco] == 0)
                {
                    do
                    {
                        if (TpBarco == 1)
                            Pos = 1;
                        else
                        {
                            Pos = r.Next(1, 3); // Escolhe se Vertical ou Horizontal
                            //Console.WriteLine("Pos: {0}", Pos);
                        }
                        L_C_Random(TpBarco, ref L, ref C); // escolhe os valores para Linha e Coluna
                        if (Vazio_Rnd(L, C, Pos, TpBarco, Selection))
                        {
                            Batalha.ins_barco(TpBarco, L, C, Pos, ref Selection, Selection); // insere o navio na Matriz                                
                            Barcos[TpBarco] = TpBarco;
                            Barcos[0]++;
                            Console.ResetColor();
                        }
                    } while (Barcos[TpBarco] == 0);
                }
            } while (Barcos[0] < 5);
            Console.Clear();
            return;
        }
        static public void L_C_Random(int Tpbarco, ref int Linha, ref int Coluna)
        {
            int[] L, C;
            int i, j;
            Random r = new Random();
            L = new int[6]; C = new int[6];
            i = 1;
            while (i < 6)
            {
                L[i] = r.Next(1, 16);
                C[i] = r.Next(1, 16);
                j = 1;
                while (j < 6)
                {
                    if (L[i] == L[j] && i != j)
                        L[i] = r.Next(1, 16);
                    if (C[i] == C[j] && i != j)
                        C[i] = r.Next(1, 16);
                    if (L[i] == L[j] && i != j || C[i] == C[j] && i != j)
                        j = 1;
                    else j++;
                }
                i++;
            }
            Linha = L[Tpbarco];
            Coluna = C[Tpbarco];
            return;
        }
        static public bool Vazio_Rnd(int L, int C, int Hor_Ver, int TpBarco, int[,] Matriz)
        {
            bool Vazio = true;
            switch (Hor_Ver)
            {
                case 1:
                    switch (TpBarco)
                    {
                        case 1:
                            if (Matriz[L, C] == 0)
                                Vazio = true;
                            else Vazio = false;
                            break;
                        case 2:
                            C = C + 1;
                            if (C == 0 || C > 15)
                                Vazio = false;
                            else
                                if (Matriz[L, C - 1] == 0 && Matriz[L, C] == 0)
                                    Vazio = true;
                                else Vazio = false;
                            break;
                        case 3:
                            C = C + 2;
                            if (C == 0 || C > 15)
                                Vazio = false;
                            else
                                if (Matriz[L, C - 2] == 0 && Matriz[L, C - 1] == 3 && Matriz[L, C] == 0)
                                    Vazio = true;
                                else Vazio = false;
                            break;
                        case 4:
                            C = C + 3;
                            if (C == 0 || C > 15)
                                Vazio = false;
                            else
                                if (Matriz[L, C - 3] == 0 && Matriz[L, C - 2] == 0 && Matriz[L, C - 1] == 0 && Matriz[L, C] == 0)
                                    Vazio = true;
                                else Vazio = false;
                            break;
                        case 5:
                            C = C + 4;
                            if (C == 0 || C > 15)
                                Vazio = false;
                            else
                                if (Matriz[L, C - 4] == 0 && Matriz[L, C - 3] == 0 && Matriz[L, C - 2] == 0 && Matriz[L, C - 1] == 0 && Matriz[L, C] == 0)
                                    Vazio = true;
                                else Vazio = false;
                            break;
                    }
                    break;
                case 2:
                    switch (TpBarco)
                    {
                        case 1:
                            if (Matriz[L, C] == 0)
                                Vazio = true;
                            else Vazio = false;
                            break;
                        case 2:
                            L = L + 1;
                            if (L == 0 || L > 15)
                                Vazio = false;
                            else
                                if (Matriz[L - 1, C] == 0 && Matriz[L, C] == 0)
                                    Vazio = true;
                                else Vazio = false;
                            break;
                        case 3:
                            L = L + 2;
                            if (L == 0 || L > 15)
                                Vazio = false;
                            else
                                if (Matriz[L - 2, C] == 0 && Matriz[L - 1, C] == 0 && Matriz[L, C] == 0)
                                    Vazio = true;
                                else Vazio = false;
                            break;
                        case 4:
                            L = L + 3;
                            if (L == 0 || L > 15)
                                Vazio = false;
                            else
                                if (Matriz[L - 3, C] == 0 && Matriz[L - 2, C] == 0 && Matriz[L - 1, C] == 0 && Matriz[L, C] == 0)
                                    Vazio = true;
                                else Vazio = false;
                            break;
                        case 5:
                            L = L + 4;
                            if (L == 0 || L > 15)
                                Vazio = false;
                            else
                                if (Matriz[L - 4, C] == 0 && Matriz[L - 3, C] == 0 && Matriz[L - 2, C] == 0 && Matriz[L - 1, C] == 0 && Matriz[L, C] == 0)
                                    Vazio = true;
                                else Vazio = false;
                            break;
                    }
                    break;
            }
            return Vazio;

        }
    }
    class program
    {
        public static void Main()
        {
            IntPtr hConsole = GetStdHandle(-11);
            SetConsoleDisplayMode(hConsole, 1); // COMANDO PARA TELA INTEIRA                        
            //int C = 0, L = 0;

            int[,] M_player = new int[16, 16];
            int[,] M_pc = new int[16, 16];
            int[] Barcos = new int[6];
            for (int l = 0; l < 16; l++)
                for (int c = 0; c < 16; c++)
                {
                    M_player[l, c] = 0;
                    M_pc[l, c] = 0;
                }

            Coordenadas Batalha = new Coordenadas();

            Naval.Tela_SelectShips();
            Batalha.Select_Random(ref M_pc);
            Naval.Tela_SelectShips();
            Funcoes.DesenharBotao("<Enter> Selecionar", 24, 24);
            Funcoes.DesenharBotao("   Manualmente    ", 25, 24);
            Funcoes.DesenharBotao("<F3>  Selecionar  ", 27, 24);
            Funcoes.DesenharBotao("   Aleatoriamente ", 28, 24);
            //Funcoes.DesenharBotao("<F2>  Iniciar Jogo", 18, 59);
            Funcoes.DesenharBotao("<ESC> Voltar Tela ", 30, 24);

            ConsoleKeyInfo S = Console.ReadKey(true);
            if (S.Key == ConsoleKey.F3)
            {
                Console.CursorTop = 23; Console.CursorLeft = 23; Console.BackgroundColor = ConsoleColor.Blue;

                Console.ResetColor(); Console.CursorTop = 24; Console.CursorLeft = 24; Console.Write("\t\t\t");
                Console.ResetColor(); Console.CursorTop = 25; Console.CursorLeft = 24; Console.Write("\t\t\t");
                Console.ResetColor(); Console.CursorTop = 27; Console.CursorLeft = 24; Console.Write("\t\t\t");
                Console.ResetColor(); Console.CursorTop = 28; Console.CursorLeft = 24; Console.Write("\t\t\t");
                Console.ResetColor(); Console.CursorTop = 30; Console.CursorLeft = 24; Console.Write("\t\t\t");

                Console.CursorTop = 26; Console.CursorLeft = 20;
                Console.BackgroundColor = ConsoleColor.Red; Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("Selecionando Aleatoriamente");

                int qq = 52;
                while (qq < 65)
                {
                    Console.ResetColor(); Console.ForegroundColor = ConsoleColor.Red;
                    Console.CursorLeft = qq;
                    Console.Write(".");
                    Thread.Sleep(100);
                    qq++;
                }
                Batalha.Select_Random(ref M_player); // chamar funcao para escolher aleatoriamente                                
            }
            if (S.Key == ConsoleKey.Escape)
            {
                Console.CursorTop = 23; Console.CursorLeft = 23; Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("Tela Inicial");

                Console.ResetColor(); Console.CursorTop = 24; Console.CursorLeft = 24; Console.Write("\t\t\t");
                Console.ResetColor(); Console.CursorTop = 25; Console.CursorLeft = 24; Console.Write("\t\t\t");
                Console.ResetColor(); Console.CursorTop = 27; Console.CursorLeft = 24; Console.Write("\t\t\t");
                Console.ResetColor(); Console.CursorTop = 28; Console.CursorLeft = 24; Console.Write("\t\t\t");
                Console.ResetColor(); Console.CursorTop = 30; Console.CursorLeft = 24; Console.Write("\t\t\t");
                //chamar funcao que retorna ao menu incial
                Console.ResetColor();
            }
            if (S.Key == ConsoleKey.Enter)
            {
                Console.CursorTop = 23; Console.CursorLeft = 23; Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("Digite abaixo as opcoes:");

                Console.ResetColor(); Console.CursorTop = 24; Console.CursorLeft = 24; Console.Write("\t\t\t");
                Console.ResetColor(); Console.CursorTop = 25; Console.CursorLeft = 24; Console.Write("\t\t\t");
                Console.ResetColor(); Console.CursorTop = 27; Console.CursorLeft = 24; Console.Write("\t\t\t");
                Console.ResetColor(); Console.CursorTop = 28; Console.CursorLeft = 24; Console.Write("\t\t\t");
                Console.ResetColor(); Console.CursorTop = 30; Console.CursorLeft = 24; Console.Write("\t\t\t");

                Naval.SelectShips(ref M_player);
                Console.ResetColor();
            }
            Naval.Tela_SelectShips();
            //Print_Matriz(M_pc);
            //Thread.Sleep(2000);            
            Print_Matriz(M_player);
            //Console.ResetColor();
            Console.ReadKey();
            Naval.Tela_Jogo();
        }

        // imprimir matriz para teste
        static public void Print_Matriz(int[,] M)
        {
            int L, C;
            Console.CursorTop = 5;
            Console.BackgroundColor = ConsoleColor.Blue;
            for (L = 1; L < 16; L++)
            {
                Console.Write("\n");
                Console.CursorLeft = 23;
                for (C = 1; C < 16; C++)
                {
                    if (M[L, C] != 0)
                    {

                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Green;
                        //Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("{0}", M[L, C]);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write("|");
                        //Console.BackgroundColor = ConsoleColor.Blue;
                        //Console.Write(" ");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("_|", M[L, C]);
                    }
                }
            }
        }



        // funções de modo de tela

        [StructLayout(LayoutKind.Sequential)]
        private struct COORD
        {
            public short X;
            public short Y;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public COORD Size;
            public COORD CursorPosition;
            public short Attributes;
            public SMALL_RECT Window;
            public COORD MaximumWindowSize;
        }
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int handle);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleDisplayMode(IntPtr hConsole, int mode);
        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleScreenBufferInfo(IntPtr hConsole, out CONSOLE_SCREEN_BUFFER_INFO info);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleWindowInfo(IntPtr hConsole, bool absolute, ref SMALL_RECT rect);

    }
}