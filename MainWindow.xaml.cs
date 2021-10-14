using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool first = true;  //pokazuje kto gra pierwszy domyślnie w pvp i czy komputer jest o jeśli true czy x jeśli false
        bool gracz = true;  //pokazuje kto teraz gra
        int jakgramy = 0;   //okresla level
        bool znak = false;   //czy bramy na XO domyślnie czy obrazkami
        bool AI = true;        //określa czy grę z komputerem zaczyna komputer -true czy człowiek - false
        bool ruch = false;      //powie nam czy komputer ruszył się już w tej turze
        int opcja = 0; //pomorze w grze na impossible
        int pomopcja = 0; //pomocna gdy się komplikuje
        int ileruchow = 0;      //określa ile ruchów zostało wykonanych w danej grze
        Button[] table;
        const string slonko = "c:\\Program Files\\TicTacToeWPF\\TicTacToeWPF\\Resources\\slonce.jpg";
        const string heart = "c:\\Program Files\\TicTacToeWPF\\TicTacToeWPF\\Resources\\serce.jpg";
        const string ogien = "c:\\Program Files\\TicTacToeWPF\\TicTacToeWPF\\Resources\\ogien.png";
        const string snieg = "c:\\Program Files\\TicTacToeWPF\\TicTacToeWPF\\Resources\\snieg.jpg";
        const string moon = "c:\\Program Files\\TicTacToeWPF\\TicTacToeWPF\\Resources\\moon.jpg";
        const string ziemia = "c:\\Program Files\\TicTacToeWPF\\TicTacToeWPF\\Resources\\ziemia.gif";
        const string rose = "c:\\Program Files\\TicTacToeWPF\\TicTacToeWPF\\Resources\\rose.jpg";
        const string lips = "c:\\Program Files\\TicTacToeWPF\\TicTacToeWPF\\Resources\\lips.gif";
        string slonce = slonko;
        string serce = heart;
        int kindofsigns = 1;

        //System.Drawing.Bitmap bitmap1 = TicTacToeWPF.Properties.Resources.serce;

        public MainWindow()     //ta funkcja była potrzebna żeby tablica buttonów działała
        {
            InitializeComponent();
            table = new Button[9] { button1, button2, button3, button4, button5, button6, button7, button8, button9 };
        }

        private void PutPicture(string sciezka, object sender)
        {
            Button button = (sender as Button);
            button.Content = new Image
            {
                Source = new BitmapImage(new Uri(sciezka)),
                VerticalAlignment = VerticalAlignment.Center
            };
            if (sciezka == heart) { button.Background = new SolidColorBrush(Colors.MistyRose); }
            if (sciezka == slonko) { button.Background = new SolidColorBrush(Colors.DeepSkyBlue); }
            if (sciezka == lips) { button.Background = new SolidColorBrush(Colors.MediumVioletRed); }
            if (sciezka == rose) { button.Background = new SolidColorBrush(Colors.Lime); }
            if (sciezka == ogien) { button.Background = new SolidColorBrush(Colors.OrangeRed); }
            if (sciezka == snieg) { button.Background = new SolidColorBrush(Colors.Snow); }
            if (sciezka == moon) { button.Background = new SolidColorBrush(Colors.Gold); }
            if (sciezka == ziemia) { button.Background = new SolidColorBrush(Colors.PaleTurquoise); }
        }

        private void LosowyRuchKomputera()   //wykonuje losowy ruch komputera
        {
            Random y = new Random();
            int sto = y.Next(0, 9 - ileruchow);     //losujemy nr od 0 do ilości wolnych buttonów, zapisujemy do sto
            int[] wolne;
            wolne = new int[9];     
            int nr = 0;
            for (int i = 0; i < 9; i++)
            {
                if(table[i].Tag.ToString().Equals("0")) { wolne[nr] = i;  nr++; }
            }     //znajdujemy wolne buttony zapisujemy do tablicy wolne
            nr = wolne[sto];        //bierzemy wylosowany button

            ileruchow++;        //dodajemy ruch do licznika
            if (first)      //musimy pamiętacz czym gra komputer - wykonujemy ruch
            {
                if (znak)
                {
                    table[nr].Content = "O";
                }
                else
                {
                    PutPicture(slonce, table[nr]);
                }
                textBox.Text = "Your turn player";
                table[nr].Foreground = new SolidColorBrush(Colors.Blue);
                table[nr].Tag = 1;
            }
            else
            {
                if (znak)
                {
                    table[nr].Content = "X";
                }
                else
                {
                    PutPicture(serce, table[nr]);
                }       //ruch wykonany, nadajemy tag 1 dla komputera i kosmetykę
                textBox.Text = "Your turn player";
                table[nr].Foreground = new SolidColorBrush(Colors.Red);
                table[nr].Tag = 1;
            }
            gracz = false;      //zmieniamy gracza, którego teraz jest kolej jak zawsze po ruchu
        }

        private void resetuj()   //funkcja rozpoczyna nową grę
        {
            ileruchow = 0;
            for (int i = 0; i < 9; i++)
            {
                table[i].Content = "";
                table[i].Tag = 0;
                table[i].Background = new SolidColorBrush(Colors.Gainsboro);
            }
            gracz = first;      //czyścimy ilość ruchów, napisy/obrazki na buttonach, tagi buttonów ustawiamy turę gracza, który powinien zaczynać
            if (jakgramy != 0) { gracz = AI; }
            if (gracz)
            {
                textBox.Text = "Your turn cross";
            }
            else
            {
                textBox.Text = "Your turn circle";
            }
            if (AI && jakgramy == 3) { KomputerPostaw(button1); }
        }

        private void wygrana()//funkcja sprawdza po każdym ruchu czy nastąpiła wygrana
        {                      //sprawdzamy 3 rzędy 3 kolumny i dwie przekątne
            if  ((button1.Tag.ToString().Equals("1") && button2.Tag.ToString().Equals("1") && (button3.Tag.ToString().Equals("1"))) 
            ||  (button1.Tag.ToString().Equals("1") && button4.Tag.ToString().Equals("1") && (button7.Tag.ToString().Equals("1")))
            ||  (button1.Tag.ToString().Equals("1") && button5.Tag.ToString().Equals("1") && (button9.Tag.ToString().Equals("1")))
            ||  (button4.Tag.ToString().Equals("1") && button5.Tag.ToString().Equals("1") && (button6.Tag.ToString().Equals("1")))
            ||  (button9.Tag.ToString().Equals("1") && button3.Tag.ToString().Equals("1") && (button6.Tag.ToString().Equals("1")))
            ||  (button2.Tag.ToString().Equals("1") && button5.Tag.ToString().Equals("1") && (button8.Tag.ToString().Equals("1")))
            ||  (button3.Tag.ToString().Equals("1") && button5.Tag.ToString().Equals("1") && (button7.Tag.ToString().Equals("1")))
            ||  (button7.Tag.ToString().Equals("1") && button8.Tag.ToString().Equals("1") && (button9.Tag.ToString().Equals("1")))) {
                if (jakgramy == 0)  //jeśli gramy pvp wygrywa krzyżyk a jeśli nie komputer
                {
                    MessageBox.Show("Cross won!");
                    textBox.Text = "Click any button to play again";
                }
                else
                {
                    MessageBox.Show("AI won!");
                    textBox.Text = "Click any button to play again";
                }
                ileruchow = 9;
            }
            else {

                if((button1.Tag.ToString().Equals("2") && button2.Tag.ToString().Equals("2") && (button3.Tag.ToString().Equals("2")))
                || (button1.Tag.ToString().Equals("2") && button4.Tag.ToString().Equals("2") && (button7.Tag.ToString().Equals("2")))
                || (button1.Tag.ToString().Equals("2") && button5.Tag.ToString().Equals("2") && (button9.Tag.ToString().Equals("2")))
                || (button4.Tag.ToString().Equals("2") && button5.Tag.ToString().Equals("2") && (button6.Tag.ToString().Equals("2")))
                || (button9.Tag.ToString().Equals("2") && button3.Tag.ToString().Equals("2") && (button6.Tag.ToString().Equals("2")))
                || (button2.Tag.ToString().Equals("2") && button5.Tag.ToString().Equals("2") && (button8.Tag.ToString().Equals("2")))
                || (button3.Tag.ToString().Equals("2") && button5.Tag.ToString().Equals("2") && (button7.Tag.ToString().Equals("2")))
                || (button7.Tag.ToString().Equals("2") && button8.Tag.ToString().Equals("2") && (button9.Tag.ToString().Equals("2"))))
                {
                    ileruchow = 9;
                    if (jakgramy == 0)
                    {
                        MessageBox.Show("Circle won!");
                        textBox.Text = "Click any button to play again";
                    } else
                    {
                        MessageBox.Show("Human won!");
                        textBox.Text = "Click any button to play again";
                        ruch = true;
                        if (jakgramy == 1 || jakgramy == 2) { ileruchow++; } 
                    }

                   
                }
                else
                {
                    if (ileruchow == 9) //jeśli po 9 ruchach nie ma wygranej to znaczy że jest remis
                    {
                        MessageBox.Show("Tie!");
                        textBox.Text = "Click any button to play again";
                    }
                }
            }
        }

        private void RuchCzlowiekavsAI(object sender)     //opisuje ruch człowiek przeciwko komputerowi
        {
            Button button = (sender as Button);
            if (!gracz && ileruchow <= 9)       //mogłoby nie być tego warunku - ruch człowieka
            {
                if (first)
                {
                    if (znak)
                    {
                        button.Content = "X";
                    }
                    else
                    {
                        PutPicture(serce, button);
                    }
                    button.Tag = 2;
                    button.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    button.Tag = 2;
                    if (znak)
                    {
                        button.Content = "O";
                    }
                    else
                    {
                        PutPicture(slonce, button);
                    }
                    button.Foreground = new SolidColorBrush(Colors.Blue);
                }
            }
            gracz = true; //zmienamy na turę komputera
        }

        private void KomputerPostaw(object sender)
        {
            Button button = (sender as Button);
            if (first)
            {
                button.Tag = 1;
                if (znak)
                {
                    button.Content = "O";
                }
                else
                {
                    PutPicture(slonce, button);
                }
                button.Foreground = new SolidColorBrush(Colors.Blue);
            }
            else
            {
                if (znak)
                {
                    button.Content = "X";
                }
                else
                {
                    PutPicture(serce, button);
                }
                button.Tag = 1;
                button.Foreground = new SolidColorBrush(Colors.Red);
            }
            gracz = false;
        }

        private int LiczLinie(string tak,int a, int b, int c)    //liczymy ile jakich znaków w lini
        {
            int wynik = 0;
            string przeciwnik="";
            if (tak.Equals("1")) { przeciwnik = "2"; }
            if (tak.Equals("2")) { przeciwnik = "1"; }
            if (!(table[a-1].Tag.ToString().Equals(przeciwnik)|| table[b-1].Tag.ToString().Equals(przeciwnik)|| table[c-1].Tag.ToString().Equals(przeciwnik))) {
                if (table[a-1].Tag.ToString().Equals(tak)) { wynik++; }
                if (table[b-1].Tag.ToString().Equals(tak)) { wynik++; }
                if (table[c-1].Tag.ToString().Equals(tak)) { wynik++; }
            }
            return wynik;
        }

        private int[] CzyMozna(string tak)        //uzupełniemy tablicę liczbami dla każdej linii
        {
            int[] zajete;
            zajete = new int[8];
            for (int i = 0; i < 7; i++) { zajete[i] = 0; }  //stworzylismy tablice do liczenia zajetości
            zajete[0] = LiczLinie(tak, 1, 2, 3);
            zajete[1] = LiczLinie(tak, 4, 5, 6);
            zajete[2] = LiczLinie(tak, 7, 8, 9);
            zajete[3] = LiczLinie(tak, 1, 4, 7);
            zajete[4] = LiczLinie(tak, 2, 5, 8);
            zajete[5] = LiczLinie(tak, 3, 6, 9);
            zajete[6] = LiczLinie(tak, 1, 5, 9);
            zajete[7] = LiczLinie(tak, 3, 5, 7);
            return zajete;

        }   

        private bool PostawWLinii (bool ruch, int[] zajete, int linia,int a,int b,int c)
        {
            if (!ruch&&zajete[linia]==2)
            {
                if (table[a-1].Tag.ToString().Equals("0")) { KomputerPostaw(table[a-1]); }
                if (table[b-1].Tag.ToString().Equals("0")) { KomputerPostaw(table[b-1]); }
                if (table[c-1].Tag.ToString().Equals("0")) { KomputerPostaw(table[c-1]); }
                return true;
            }
            else
            {
                return ruch;
            }
        }

        private bool Wygraj(string tak, bool ruch)
        {
            int[] zajete;
            zajete = CzyMozna(tak);
            ruch=PostawWLinii(ruch, zajete, 0, 1, 2, 3);
            ruch=PostawWLinii(ruch, zajete, 1, 4, 5, 6);
            ruch=PostawWLinii(ruch, zajete, 2, 7, 8, 9);
            ruch=PostawWLinii(ruch, zajete, 3, 1, 4, 7);
            ruch=PostawWLinii(ruch, zajete, 4, 2, 5, 8);
            ruch=PostawWLinii(ruch, zajete, 5, 3, 6, 9);
            ruch=PostawWLinii(ruch, zajete, 6, 1, 5, 9);
            ruch=PostawWLinii(ruch, zajete, 7, 3, 5, 7);
            return ruch;
        }

        private bool BokObok(int rog)    //potrzebne w imposible po początku w rogu
        {
            if (rog == 1)
            {
                if(table[1].Tag.ToString().Equals("2")|| table[3].Tag.ToString().Equals("2")) { return true; }
            }
            if (rog == 3)
            {
                if (table[1].Tag.ToString().Equals("2") || table[5].Tag.ToString().Equals("2")) { return true; }
            }
            if (rog == 7)
            {
                if (table[7].Tag.ToString().Equals("2") || table[3].Tag.ToString().Equals("2")) { return true; }
            }
            if (rog == 9)
            {
                if (table[5].Tag.ToString().Equals("2") || table[7].Tag.ToString().Equals("2")) { return true; }
            }
            return false;
        }

        private int Pomocnicza(int pom, int rog, int jaki, int jeden, int dwa, int ruch1, int ruch2) {
            if (pom == 0)
            {
                if (rog == jaki)
                {
                    if (table[jeden].Tag.ToString().Equals("2")) { return ruch1; }
                    if (table[dwa].Tag.ToString().Equals("2")) { return ruch2; }
                }
            }
            return pom;
        }

        private int BokNaprzeciw (int rog) //potrzebne w impossiblem po początku w rogu
        {
            int pom=0;
            pom = Pomocnicza(pom, rog, 1, 5, 7, 7, 5);
            pom = Pomocnicza(pom, rog, 3, 3, 7, 7, 3);
            pom = Pomocnicza(pom, rog, 7, 1, 5, 5, 1);
            pom = Pomocnicza(pom, rog, 9, 1, 3, 3, 1);
            return pom;
        }

        private int RogZBoku (int rog)
        {
            int pom = 0;
            pom = Pomocnicza(pom, rog, 1, 2, 6, 1, 2);
            pom = Pomocnicza(pom, rog, 3, 0, 8, 1, 2);
            pom = Pomocnicza(pom, rog, 7, 0, 8, 2, 1);
            pom = Pomocnicza(pom, rog, 9, 2, 6, 2, 1);
            return pom;   //1-gdy wolne będą boki na linii poziomej, 2-na pionowej, 0-nie ma takiego rogu
        }

        private bool Bok1(int bok)
        {
            if (bok == 2)
            {
                if (table[0].Tag.ToString().Equals("2")) { return true; }
                if (table[2].Tag.ToString().Equals("2")) { return true; }
            }
            if (bok == 4)
            {
                if (table[0].Tag.ToString().Equals("2")) { return true; }
                if (table[6].Tag.ToString().Equals("2")) { return true; }
            }
            if (bok == 6)
            {
                if (table[8].Tag.ToString().Equals("2")) { return true; }
                if (table[2].Tag.ToString().Equals("2")) { return true; }
            }
            if (bok == 8)
            {
                if (table[8].Tag.ToString().Equals("2")) { return true; }
                if (table[6].Tag.ToString().Equals("2")) { return true; }
            }
            return false;
        }

        private int Bok3(int bok)
        {
            int pom = 0;
            pom = Pomocnicza(pom, bok, 4, 2, 8, 7, 1);
            pom = Pomocnicza(pom, bok, 6, 0, 6, 7, 1);
            pom = Pomocnicza(pom, bok, 2, 6, 8, 5, 3);
            pom = Pomocnicza(pom, bok, 8, 0, 2, 5, 3);
            if (pom == 0) { pom = 777; }
            return pom;
        }

        private int Bok2i4(int bok)
        {
            if (button2.Tag.ToString().Equals("2") && button6.Tag.ToString().Equals("2")){ return 3; }
            if (button4.Tag.ToString().Equals("2") && button8.Tag.ToString().Equals("2")){ return 3; }
            bok = 0;
            if (button2.Tag.ToString().Equals("2")) { bok++; }
            if (button4.Tag.ToString().Equals("2")) { bok++; }
            if (button6.Tag.ToString().Equals("2")) { bok++; }
            if (button8.Tag.ToString().Equals("2")) { bok++; }
            return bok;
        }

        public void button_Click(object sender, RoutedEventArgs e)         //ciało programu, określa co się dzieje w trakcie gry
        {
            Button button = (sender as Button);
            if (ileruchow<9 && !button.Tag.ToString().Equals("0")) return;  //jeżeli button jest zajęty nic nie robimy
            ileruchow++;
            if (ileruchow == 10)        //sprawdzamy czy gra nie jest już skończona
            {
                resetuj();
                ileruchow = 10;
            }
            switch (jakgramy) {     //switch sprawdza level i wybiera ruch w zależności od niego
                case 1:         //easy-komputer wykonuje tylko losowe ruchy
                    gracz = false;   //człowiek zawsze zaczyna po kliku
                    RuchCzlowiekavsAI(button);
                    gracz = true;
                    if (ileruchow < 9) wygrana();     //zanim ruszy się komputer należy sprawdzić czy człowiek nie wygrał
                    if (ileruchow == 9) { textBox.Text = "Click any button to play again"; }
                    if (gracz && ileruchow < 9)     //musimy sprawdzić czy ruch człowieka nie zapełnił planszy i zrobić ruch komputera
                    {
                        LosowyRuchKomputera();
                        if (ileruchow == 9) { textBox.Text = "Click any button to play again"; }
                    }
                    break;
                case 2: //hard-komputer świadomie wygrywa i broni się przed porażką
                    gracz = false;
                    ruch = false;
                    RuchCzlowiekavsAI(button);  //nie tylko ruch ale i wszystkie późniejsze warunki
                    if (ileruchow < 9) wygrana();     //zanim ruszy się komputer należy sprawdzić czy człowiek nie wygrał
                    if (ileruchow == 9) { textBox.Text = "Click any button to play again"; }
                    if (ileruchow > 9)
                    {
                        resetuj();
                    }
                    if (gracz && ileruchow < 9)  //tu będzie ruch komputera
                    {
                        if (ileruchow < 3) { LosowyRuchKomputera(); }
                        else
                        {
                            if (!ruch) { ruch = Wygraj("1", ruch); }
                            if (!ruch) { ruch = Wygraj("2", ruch); }
                            if (ruch) { gracz = false; ileruchow++; }
                            if (!ruch) { LosowyRuchKomputera(); }
                        }
                    }
                    ruch = false;
                    //if (ileruchow <= 9) wygrana();
                    if (ileruchow == 9) { textBox.Text = "Click any button to play again"; }
                    break; 
                case 3:     //impossible-komputer zna wszystkie kombinacje ruchów i jest nie do pokonania
                    gracz = false;
                    RuchCzlowiekavsAI(button);
                    wygrana();
                    gracz = true;
                    if (ileruchow >8) { textBox.Text = "Click any button to play again"; }
                    if (ileruchow > 9)
                    {
                        resetuj();
                        if (AI) { KomputerPostaw(button1); gracz = false; }
                    }
                    if (gracz)
                    {
                        ruch = false;
                        if (AI) { ileruchow++; }
                        switch (ileruchow)  //nieparzyste to ruchy obronne a parzyste gdy AI zaczyna
                        {
                            case 1:
                                for (int i = 0; i < 9; i++)
                                {
                                    if (table[i].Tag.ToString().Equals("2")) { pomopcja = i; }      //pomopcja na razie przechowa pierwszy ruch gracza
                                }
                                pomopcja++;
                                if (pomopcja == 1 || pomopcja == 3 || pomopcja == 7 || pomopcja == 9)
                                {   //UWAGA! właśnie został zrobiony najlepszy możliwy ruch, brońmy się
                                    KomputerPostaw(button5);
                                    opcja = 15; //w razie czego odróżnijmy opcje przy ataku i obronie
                                }
                                if (pomopcja == 8 || pomopcja == 2 || pomopcja == 4 || pomopcja == 6) { KomputerPostaw(button5); opcja = 20; } //tu załatwiamy boki 
                                if (pomopcja == 5) { KomputerPostaw(table[0]); opcja = 10; } //zaczął na środku odpowiedz w róg
                                break;
                            case 2:
                                if (button3.Tag.ToString().Equals("2")) { KomputerPostaw(button9); opcja = 3; }
                                if (button7.Tag.ToString().Equals("2")) { KomputerPostaw(button9); opcja = 9; } //analogia do poprzedniej, ale następny tuch musi być inny
                                if (button5.Tag.ToString().Equals("2")) { KomputerPostaw(button9); opcja = 5; } //czlowiek postawil na środku - najgorzej
                                if (button2.Tag.ToString().Equals("2") || button8.Tag.ToString().Equals("2") || button6.Tag.ToString().Equals("2")) { KomputerPostaw(button7); opcja = 7; } //czlowiek postawil na boku
                                if (button4.Tag.ToString().Equals("2")) { KomputerPostaw(button3); opcja = 7; } //pozostały bok
                                if (button9.Tag.ToString().Equals("2")) { KomputerPostaw(button3); opcja = 3; } //ruch naprzeciwko
                                break;
                            case 3:
                                ruch = false;
                                ruch = Wygraj("1", ruch);
                                if (!ruch) { ruch = Wygraj("2", ruch); }
                                switch (opcja)
                                {
                                    case 15:    //1. ruch był w róg
                                        if ((button3.Tag.ToString().Equals("2") && button7.Tag.ToString().Equals("2")) || (button1.Tag.ToString().Equals("2") && button9.Tag.ToString().Equals("2")))
                                        {
                                            KomputerPostaw(button2);    //gracz gra naszą strategią, ale dalej obronimy się automatyczne
                                        }
                                        if (BokObok(pomopcja)) { opcja = 101; }
                                        if (BokNaprzeciw(pomopcja) != 0) { KomputerPostaw(table[BokNaprzeciw(pomopcja)]); } //rozprawiliśmy się z grą na bokach gorzej jeśli zagra w poboczny róg
                                        pomopcja = RogZBoku(pomopcja);
                                        if (pomopcja != 0) { opcja = 200; } //tego potrzebujemy w następnym ruchu
                                        break;
                                    case 10:    //zaczął od środka
                                        if (button6.Tag.ToString().Equals("2") || button8.Tag.ToString().Equals("2")) { ruch = Wygraj("2", ruch); } //zagrał w boku po przeciwnej stronie-blokuj/dalej gramy na wygraj, nie przegraj
                                        if (button9.Tag.ToString().Equals("2")) { KomputerPostaw(table[2]); opcja = 101; }
                                        if (button3.Tag.ToString().Equals("2") || button7.Tag.ToString().Equals("2")) { ruch = Wygraj("2", ruch); opcja = 101; } //do tej opcji wrócimy w 4 ruchu, następny to W/NP
                                        if (button2.Tag.ToString().Equals("2")) { KomputerPostaw(button8); opcja = 102; }
                                        if (button4.Tag.ToString().Equals("2")) { KomputerPostaw(button6); opcja = 104; } //te dwie opcje są symetryczne takie zaznaczenie do tą symetrię
                                        break;
                                    case 20: //zaczął z boku
                                        opcja = Bok3(pomopcja);         //drugi ruch w róg dalej pierwszego ruchu
                                        if (opcja != 777) { KomputerPostaw(table[opcja]); } //dalej obrani się automatycznie
                                        if (Bok1(pomopcja)) { opcja = 101; }    //drugi ruch obok 1.
                                        pomopcja = Bok2i4(pomopcja);
                                        if (pomopcja == 2) { KomputerPostaw(button3); }
                                        if (pomopcja == 3) { KomputerPostaw(button1); }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case 4:
                                ruch = false;
                                ruch = Wygraj("1", ruch);
                                if (!ruch)
                                {
                                    switch (opcja)
                                    {
                                        case 7: //najpierw rozprawiamy się z bokami
                                            KomputerPostaw(button5); //pewna wygrana w następnym ruchu na przekatnej
                                            break;
                                        case 3:
                                            KomputerPostaw(button7); // pewna wygrana w następnym ruchu bok lub przekątna
                                            break;
                                        case 9:
                                            KomputerPostaw(button3); //jak wyżej, bok lub przekątna
                                            break;
                                        default://na pewno nie ma jeszcze wygranej
                                            ruch = Wygraj("2", ruch); //jeśli człowiek postawił w rogu wygraliśmy jeśli na boku będzie remis
                                            break;
                                    }
                                }
                                break;
                            case 5:
                                ruch = false;
                                ruch = Wygraj("1", ruch);
                                if (!ruch) { ruch = Wygraj("2", ruch); }
                                if (!ruch && opcja == 200)
                                {
                                    if (pomopcja == 1) { KomputerPostaw(button4); }
                                    if (pomopcja == 2) { KomputerPostaw(button2); }
                                }   //zakończenie rogu + rogu obok
                                if (opcja == 102) { if (button4.Tag.ToString().Equals("2") || button7.Tag.ToString().Equals("2")) { opcja = 101; } } // w tej sytuacji na końcu musi być losowy ruch
                                if (opcja == 104 && (button2.Tag.ToString().Equals("2") || button3.Tag.ToString().Equals("2"))) { opcja = 101; }
                                if (button9.Tag.ToString().Equals("2") && (opcja == 102)) { KomputerPostaw(button7); }
                                if (!ruch && button9.Tag.ToString().Equals("2") && (opcja == 104)) { KomputerPostaw(button3); } //SKOŃCZONY POCZĄTEK Z POCZĄTKIEM NA ŚRODKU
                                break;
                            case 6:
                                ruch = false;
                                ruch = Wygraj("1", ruch);   //jedyny przydek który już tu nie zadziała to ruchy 5 i na bok
                                if (!ruch) { ruch = Wygraj("2", ruch); }
                                if (!ruch) { LosowyRuchKomputera(); }
                                break;
                            case 7:
                                ruch = false;
                                ruch = Wygraj("1", ruch);
                                if (!ruch) { ruch = Wygraj("2", ruch); }
                                if (!ruch && opcja == 101) { LosowyRuchKomputera(); ileruchow--; }
                                break;
                            case 8:
                                ruch = false;
                                ruch = Wygraj("1", ruch);
                                if (!ruch) { ruch = Wygraj("2", ruch); } //jeżeli człowiek grał dobrze w tym miejscu będzie remis
                                if (!ruch) { LosowyRuchKomputera(); ileruchow--; }
                                ileruchow++;
                                break;
                            default:
                                break;
                        }
                        if (!AI) { ileruchow++; }
                        if (first&&ileruchow<9) { textBox.Text = "Your turn cross"; }
                    }
                    break;
                default:    //gra plaver vs player
                    if (gracz)  //sprawdzamy czy pierwszy gracz jest krzyżykiem czy kółkiem i czekamy na ruch
                    {
                        if (znak)   //sprawdzamy czy gramy znakami czy obrazkami
                        {
                            button.Content = "X";
                        }
                        else
                        {
                            PutPicture(serce, button);
                        }
                        button.Tag = 1;
                        button.Foreground = new SolidColorBrush(Colors.Red);
                        textBox.Text = "Your turn circle";
                        gracz = false;
                        // MessageBox.Show("You won!");
                    }
                    else
                    {
                        button.Tag = 2;
                        if (znak)
                        {
                            button.Content = "O";
                        }
                        else
                        {
                            PutPicture(slonce, button);
                        }
                        textBox.Text = "Your turn cross";
                        button.Foreground = new SolidColorBrush(Colors.Blue);
                        gracz = true;

                    }
                    
                    break;
            }
            if (ileruchow < 10) { wygrana(); }       //po każdym ruchu nie zależnie od levela sprawdzamy czy nie ma wygranej
            if (ileruchow == 9) { textBox.Text = "Click any button to play again";}
            if (ileruchow > 9 && jakgramy!=3)
            {
                resetuj(); 
                if (AI)
                {
                    if (jakgramy == 1 || jakgramy == 2)
                    { LosowyRuchKomputera(); }  //jeśli gramy przeciw komputarowi wykonujemy losowy ruch dla tych dwóch leveli
                }
            }

        }

        private void XOButton_Click(object sender, RoutedEventArgs e)   //zmiana wyświetlanych obrazków na znaki x i o
        {
            znak = true;
            slonce = slonko;
            serce = heart;
            PicturesButton.IsChecked = false;
            Pictures2Button.IsChecked = false;
            FireButton.IsChecked = false;
            MoonButton.IsChecked = false;
            XOButton.IsChecked = true;      //zmieniamy znak, żeby funkcje gry działały poprawnie, zmieniamy zaznaczenie w menu
            for(int i = 0; i < 9; i++)
            {
                table[i].Background = new SolidColorBrush(Colors.Gainsboro);
                //if ((table[i].Tag.ToString().Equals("1") && ((jakgramy == 0 && first) || (!first && jakgramy != 0))) || (table[i].Tag.ToString().Equals("2") && ((first && jakgramy != 0) || (jakgramy == 0 && !first)))) { table[i].Content = "X"; table[i].Foreground = new SolidColorBrush(Colors.Red); }
                //if ((table[i].Tag.ToString().Equals("2") && ((jakgramy == 0 && first) || (!first && jakgramy != 0))) || (table[i].Tag.ToString().Equals("1") && ((first && jakgramy != 0) || (jakgramy == 0 && !first)))) { table[i].Content = "O"; table[i].Foreground = new SolidColorBrush(Colors.Blue); }
                if ((table[i].Tag.ToString().Equals("1") && ((jakgramy == 0) || (!first && jakgramy != 0))) || (table[i].Tag.ToString().Equals("2") && (first && jakgramy != 0))) { table[i].Content = "X"; table[i].Foreground = new SolidColorBrush(Colors.Red); }
                if ((table[i].Tag.ToString().Equals("2") && ((jakgramy == 0) || (!first && jakgramy != 0))) || (table[i].Tag.ToString().Equals("1") && (first && jakgramy != 0))) { table[i].Content = "O"; table[i].Foreground = new SolidColorBrush(Colors.Blue); }

            }
            //jeżeli gramy pvp lub komputer gra krzyżykiem, albo gdy człowiek gra krzyżykiem przeciw AI wstawiamy X
        }

        private void PicturesButton_Click(object sender, RoutedEventArgs e) //zmiana znaków x i o na obrazki
        {
            slonce = slonko;
            serce = heart;
            if(jakgramy==0 && !first)
            {
                slonce = heart;
                serce = slonko;
            }
            znak = false;
            kindofsigns = 1;
            XOButton.IsChecked = false;
            PicturesButton.IsChecked = true;    //zmieniamy zaznaczenie i boola
            Pictures2Button.IsChecked = false;
            FireButton.IsChecked = false;
            MoonButton.IsChecked = false;
            //warunek jak w poprzedniej funkcji
            for (int i = 0; i < 9; i++)
            {
                if ((table[i].Tag.ToString().Equals("1") && ((jakgramy == 0 && first) || (!first && jakgramy != 0))) || (table[i].Tag.ToString().Equals("2") && ((first && jakgramy != 0) || (jakgramy == 0 && !first))))
                { PutPicture(serce, table[i]);  }
                if ((table[i].Tag.ToString().Equals("2") && ((jakgramy == 0 && first) || (!first && jakgramy != 0))) || (table[i].Tag.ToString().Equals("1") && ((first && jakgramy != 0) || (jakgramy == 0 && !first))))
                { PutPicture(slonce, table[i]); }
            }
            if (jakgramy == 0 && !first)
            {
                slonce = slonko;
                serce = heart;
            }
        }

        private void MoonButton_Click(object sender, RoutedEventArgs e)
        {
            slonce = ziemia;
            serce = moon;
            if (jakgramy == 0 && !first)
            {
                slonce = moon;
                serce = ziemia;
            }
            znak = false;
            kindofsigns = 3;
            XOButton.IsChecked = false;
            PicturesButton.IsChecked = false;    //zmieniamy zaznaczenie i boola
            Pictures2Button.IsChecked = false;
            FireButton.IsChecked = false;
            MoonButton.IsChecked = true;
            //warunek jak w poprzedniej funkcji
            for (int i = 0; i < 9; i++)
            {
                if ((table[i].Tag.ToString().Equals("1") && ((jakgramy == 0 && first) || (!first && jakgramy != 0))) || (table[i].Tag.ToString().Equals("2") && ((first && jakgramy != 0) || (jakgramy == 0 && !first))))
                { PutPicture(serce, table[i]); }
                if ((table[i].Tag.ToString().Equals("2") && ((jakgramy == 0 && first) || (!first && jakgramy != 0))) || (table[i].Tag.ToString().Equals("1") && ((first && jakgramy != 0) || (jakgramy == 0 && !first))))
                { PutPicture(slonce, table[i]); }
            }
            if (jakgramy == 0 && !first)
            {
                slonce = ziemia;
                serce = moon;
            }
        }

        private void Pictures2Button_Click(object sender, RoutedEventArgs e)
        {
            slonce = rose;
            serce = lips;
            if (jakgramy == 0 && !first)
            {
                slonce = lips;
                serce = rose;
            }
            znak = false;
            kindofsigns = 2;
            XOButton.IsChecked = false;
            PicturesButton.IsChecked = false;    //zmieniamy zaznaczenie i boola
            Pictures2Button.IsChecked = true;
            FireButton.IsChecked = false;
            MoonButton.IsChecked = false;
            //warunek jak w poprzedniej funkcji
            for (int i = 0; i < 9; i++)
            {
                if ((table[i].Tag.ToString().Equals("1") && ((jakgramy == 0 && first) || (!first && jakgramy != 0))) || (table[i].Tag.ToString().Equals("2") && ((first && jakgramy != 0) || (jakgramy == 0 && !first))))
                { PutPicture(serce, table[i]); }
                if ((table[i].Tag.ToString().Equals("2") && ((jakgramy == 0 && first) || (!first && jakgramy != 0))) || (table[i].Tag.ToString().Equals("1") && ((first && jakgramy != 0) || (jakgramy == 0 && !first))))
                { PutPicture(slonce, table[i]); }
            }
            if (jakgramy == 0 && !first)
            {
                slonce = rose;
                serce = lips;
            }
        }

        private void FireButton_Click(object sender, RoutedEventArgs e)
        {
            slonce = snieg;
            serce = ogien;
            if (jakgramy == 0 && !first)
            {
                slonce = ogien;
                serce = snieg;
            }
            znak = false;
            kindofsigns = 4;
            XOButton.IsChecked = false;
            PicturesButton.IsChecked = false;    //zmieniamy zaznaczenie i boola
            Pictures2Button.IsChecked = false;
            FireButton.IsChecked = true;
            MoonButton.IsChecked = false;
            //warunek jak w poprzedniej funkcji
            for (int i = 0; i < 9; i++)
            {
                if ((table[i].Tag.ToString().Equals("1") && ((jakgramy == 0 && first) || (!first && jakgramy != 0))) || (table[i].Tag.ToString().Equals("2") && ((first && jakgramy != 0) || (jakgramy == 0 && !first))))
                { PutPicture(serce, table[i]); }
                if ((table[i].Tag.ToString().Equals("2") && ((jakgramy == 0 && first) || (!first && jakgramy != 0))) || (table[i].Tag.ToString().Equals("1") && ((first && jakgramy != 0) || (jakgramy == 0 && !first))))
                { PutPicture(slonce, table[i]); }
            }
            if (jakgramy == 0 && !first)
            {
                slonce = snieg;
                serce = ogien;
            }
        }

        private void EasyButton_Click(object sender, RoutedEventArgs e) //zmianiamy level na easy, zaczynamy nową grę
        {
            PlayerButton.IsChecked = false;
            EasyButton.IsChecked = true;
            HardButton.IsChecked = false;
            ImpossibleButton.IsChecked = false;
            jakgramy = 1;
            resetuj();
            if (AI)
            {
                 LosowyRuchKomputera(); 
            }
        }

        private void HardButton_Click(object sender, RoutedEventArgs e) //zmieniamy level na hard zaczynamy nową grę
        {
            PlayerButton.IsChecked = false;
            EasyButton.IsChecked = false;
            HardButton.IsChecked = true;
            ImpossibleButton.IsChecked = false;
            jakgramy = 2;
            resetuj();
            if (AI)
            {
                LosowyRuchKomputera();
            }
        }

        private void ImpossibleButton_Click(object sender, RoutedEventArgs e)   //zmieniamy level zaczynamy nową grę
        {
            PlayerButton.IsChecked = false;
            EasyButton.IsChecked = false;
            HardButton.IsChecked = false;
            ImpossibleButton.IsChecked = true;
            jakgramy = 3;
            resetuj();
            if (AI)
            {
                 KomputerPostaw(button1); 
            }
        }

        private void PlayerButton_Click(object sender, RoutedEventArgs e)   //jak wyżej
        {
            PlayerButton.IsChecked = true;
            EasyButton.IsChecked = false;
            HardButton.IsChecked = false;
            ImpossibleButton.IsChecked = false;
            jakgramy = 0;
            resetuj();
        }

        private void XfirstButton_Click(object sender, RoutedEventArgs e) //first=true oznacza, że w pvp zaczyna X, a w grzez z AI człowiek gra x lub sercem
        {
             if (!first && jakgramy == 0)
             {
                 for (int i = 0; i <= 8; i++)
                 {
                     if (table[i].Tag.ToString().Equals("1")) { table[i].Tag = 2; first = true; }
                     if (!first && table[i].Tag.ToString().Equals("2")) {  table[i].Tag = 1; }
                     first = false;
                 }
                 gracz = !gracz;
             }  //trochę jest namieszania w pvp, bo tagi są tam równoznaczne znakom
            first = true;
            OfirstButton.IsChecked = false;
            XfirstButton.IsChecked = true;
            if (znak) { XOButton_Click(sender, e); } else {
                switch (kindofsigns)
                {
                    case 1:
                        PicturesButton_Click(sender, e);
                        break;
                    case 2:
                        Pictures2Button_Click(sender, e);
                        break;
                    case 3:
                        MoonButton_Click(sender, e);
                        break;
                    case 4:
                        FireButton_Click(sender, e);
                        break;
                    default:
                        break;
                }
            }
        }

        private void OfirstButton_Click(object sender, RoutedEventArgs e) //first=false oznacze że w pvp zaczyna o, a vs AI człowiek gra o lub sloncem
        {
            OfirstButton.IsChecked = true;
            XfirstButton.IsChecked = false;
            if (first && jakgramy == 0)
            {
                for (int i = 0; i <= 8; i++)
                {
                    if (table[i].Tag.ToString().Equals("1")) { table[i].Tag = 2;  first = false; }
                    if (first && table[i].Tag.ToString().Equals("2")) { table[i].Tag = 1; }
                    first = true;
                }
                gracz = !gracz;
            }
            if (jakgramy != 0) { first = false; }
            if (znak) { XOButton_Click(sender, e); } else
            {
                switch (kindofsigns)
                {
                    case 1:
                        PicturesButton_Click(sender, e);
                        break;
                    case 2:
                        Pictures2Button_Click(sender, e);
                        break;
                    case 3:
                        MoonButton_Click(sender, e);
                        break;
                    case 4:
                        FireButton_Click(sender, e);
                        break;
                    default:
                        break;
                }
                
            }
            first = false;

        }

        private void AIfirstButton_Click(object sender, RoutedEventArgs e)        //komputer ma zaczynać
        {
            AI = true;
            AIfirstButton.IsChecked = true;
            HumanfirstButton.IsChecked = false;
            if (jakgramy != 0) { resetuj(); }
            if (AI)
            {
                if (jakgramy == 1 || jakgramy == 2)
                { LosowyRuchKomputera(); }  //jeśli gramy przeciw komputarowi wykonujemy losowy ruch dla tych dwóch leveli
                if (jakgramy == 3) { KomputerPostaw(button1); }
            }
        }

        private void HumanfirstButton_Click(object sender, RoutedEventArgs e) //człowiek ma zaczynać grę przeciw AI
        {
            AI = false;
            AIfirstButton.IsChecked = false;
            HumanfirstButton.IsChecked = true;
            if (jakgramy != 0) { resetuj(); }
        }
    }
}
