using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Microsoft.Test.Input;
namespace skribblBOT
{
    public partial class Form1 : Form
    {
        public string FILE_NAME;
        public int rysunek = 1; //dodawanie do labela numeru rysunku 
        private const double BW_THRESHOLD = 0.5; 
        private readonly Color colorBlack =
          Color.FromArgb(255, 0, 0, 0); //detekcja czarnego
        private readonly Color colorWhite =
          Color.FromArgb(255, 255, 255, 255); //detekcja białego
        public Bitmap originalImage; //inicjowanie oryginalnego obrazka
        public Bitmap convertedImage; //inicjowanie przekonwertowanego obrazka
        private readonly List<Vertex> vertices = new List<Vertex>(); //lista wierzchołków
        public Form1()
        {
            InitializeComponent();
            richTextBox1.Text = "skribblBOT.exe\nZostał pomyślnie aktywowany.\n==========\nZanim przejdziesz do rysowania, odpal okno przeglądarki pod programem. Pamiętaj, aby wyłączyć program podczas rysowania użyj Ctrl+Alt+Delete i zabij proces.\n==========\nGOTOWY."; //tekst w konsoli przy odpaleniu programu
        }

        public Bitmap Img2BW(Bitmap imgSrc, double threshold)
        {
            int width = imgSrc.Width;
            int height = imgSrc.Height;
            Color pixel;
            Bitmap imgOut = new Bitmap(imgSrc);
            label1.Text = "Konwertowanie obrazu...";
            progressBar1.Value = 0; //progressbar  ktory nie jest  uzywany mozna  do niego dopisac jakas funkcje
            for (int row = 0; row < height - 1; row++) //konwertowanie  obrazka
            {
                for (int col = 0; col < width - 1; col++)
                {
                    pixel = imgSrc.GetPixel(col, row);
                    if (pixel.GetBrightness() < threshold)
                    {
                        this.vertices.Add(new Vertex(col, row));
                        imgOut.SetPixel(col, row, this.colorBlack);
                    }
                    else
                    {
                        imgOut.SetPixel(col, row, this.colorWhite);
                    }
                }
            } //koniec konwertowania
            label1.Text = "Zakończono konwertowanie..."; //komunikat
            richTextBox1.AppendText("\nBłąd, nie podano ścieżki"); //wpis  do konsoli
            return imgOut;
        }
    private void button1_Click(object sender, EventArgs e)
        {
            if (FILE_NAME == null)
            {
                richTextBox1.AppendText("\nBłąd, nie podano ścieżki"); //komunikat do konsoli
            }
            else
            {
                Mouse.MoveTo(new Point(1700, 400)); //proces rysowania zautomatyzowaną myszką, ustawienie myszki etc.
                Mouse.Click(MouseButton.Left);
                Mouse.MoveTo(new Point(875, 128)); //punkty do klikania w paincie na  czarny na rozdzialce 1920x1080
                Mouse.Click(MouseButton.Left);
                label1.Text = "Rysowanie...";
                rysunek++;
                label2.Text = "Rysunek: " + rysunek; //komunikat liczący rysunki
                foreach (Vertex vert in this.vertices)
                {
                    string[] sizeParts = vert.ToString().Split(':'); //rysowanie przekonwertowanego obrazka
                    int height = int.Parse(sizeParts[0]);
                    int width = int.Parse(sizeParts[1]);
                    Size mySize = new Size(height, width);
                    Mouse.MoveTo(new Point(height + 3, width + 3));
                    Mouse.Click(MouseButton.Left);
                    richTextBox1.AppendText("=="); //bez tego program za szybko dziala  i go wykurwia wiem ze moglem uzyc sleepa ale  po co pozdro
                }
                vertices.Clear();
                label1.Text = "Zakończono rysowanie...";
                richTextBox1.AppendText("\nGOTOWE."); //komunikaty
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Jeszcze nie wiem co to będzie robiło", "Jebać sąd"); // xD
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }



        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog //dialog otwieranie pliku
            {
                InitialDirectory = @"C:\",
                Title = "Przeglądaj pliki graficzne",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "jpg",
                Filter = "Pliki graficzne (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK) //na  wypadek klikniecia  ok w dialogu
            {
                textBox1.Text = openFileDialog1.FileName;
                FILE_NAME = openFileDialog1.FileName;
                pictureBox1.ImageLocation = FILE_NAME;
                originalImage = new Bitmap(FILE_NAME);
                convertedImage =
                  this.Img2BW(this.originalImage, BW_THRESHOLD);
                vertices.Clear(); //czyszczenie listy z wierzchołkami
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Program został napisany w C#, użyłem do niego bibliotek do testowania programów prosto od Microsoftu.", "O programie...");
        }
    }

    public class Vertex
    {
        public Vertex(int i, int j)
        {
            //koordynaty lewego gornego rogu pola rysowania
            this.X = i+483; 
            this.Y = j+217;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public string ToString()
        {
            return string.Format("{0}:{1}", this.X, this.Y);  //zwracanie  puntku
        }
    }
}
