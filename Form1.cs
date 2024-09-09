using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bilheteria
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeUI();
            LoadReservations();
        }

        private Button btnFinalizar;
        private Button btnReservar;
        private Button btnMapaOcupacao;
        private Button btnFaturamento;
        private Button btnConfirmarReserva;
        private Button btnVoltar;
        private CheckBox[][] places;

        private static readonly string ReservationFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "reservas.txt");

        private void SaveReservations()
        {
            using (StreamWriter writer = new StreamWriter(ReservationFilePath))
            {
                for (int j = 0; j < places.Length; j++)
                {
                    for (int k = 0; k < places[j].Length; k++)
                    {
                        if (places[j][k].Checked)
                        {
                            writer.WriteLine($"{j},{k}");
                        }
                    }
                }
            }
        }

        private void LoadReservations()
        {
            if (File.Exists(ReservationFilePath))
            {
                int rows = 15;
                int columns = 40;
                int checkBoxSize = 20;
                int spacing = 5;

                places = new CheckBox[rows][];

                for (int j = 0; j < rows; j++)
                {
                    places[j] = new CheckBox[columns];

                    for (int k = 0; k < columns; k++)
                    {
                        places[j][k] = new CheckBox();


                        places[j][k].Text = "";

                        places[j][k].FlatStyle = FlatStyle.Flat;
                    }
                }
                string[] lines = File.ReadAllLines(ReservationFilePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 2)
                    {
                        int row = int.Parse(parts[0]);
                        int col = int.Parse(parts[1]);

                        if (row >= 0 && row < places.Length && col >= 0 && col < places[row].Length)
                        {
                            places[row][col].Checked = true;
                            places[row][col].Enabled = false;
                        }
                    }
                }
            }
        }

        private void InitializeUI()
        {
            this.Text = "Cinema Bilheteria";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            btnFinalizar = new Button();
            btnFinalizar.Text = "Finalizar";
            btnFinalizar.Size = new Size(100, 30);
            btnFinalizar.Location = new Point(10, 10);
            btnFinalizar.Click += BtnFinalizar_Click;
            this.Controls.Add(btnFinalizar);

            btnReservar = new Button();
            btnReservar.Text = "Reservar Poltrona";
            btnReservar.Size = new Size(120, 30);
            btnReservar.Location = new Point(120, 10);
            btnReservar.Click += BtnReservar_Click;
            this.Controls.Add(btnReservar);

            btnFaturamento = new Button();
            btnFaturamento.Text = "Faturamento";
            btnFaturamento.Size = new Size(100, 30);
            btnFaturamento.Location = new Point(380, 10);
            btnFaturamento.Click += BtnFaturamento_Click;
            this.Controls.Add(btnFaturamento);

            btnMapaOcupacao = new Button();
            btnMapaOcupacao.Text = "Mapa de ocupação";
            btnMapaOcupacao.Size = new Size(120, 30);
            btnMapaOcupacao.Location = new Point(250, 10);
            btnMapaOcupacao.Click += BtnMapaOcupacao_Click;
            this.Controls.Add(btnMapaOcupacao);
        }

        private void InitializePlaces()
        {
            int rows = 15;
            int columns = 40;
            int checkBoxSize = 20;
            int spacing = 5;

            for (int j = 0; j < rows; j++)
            {
                for (int k = 0; k < columns; k++)
                {
                    places[j][k].Parent = this;

                    places[j][k].Top = j * (checkBoxSize + spacing);
                    places[j][k].Left = k * (checkBoxSize + spacing);

                    places[j][k].Width = checkBoxSize;
                    places[j][k].Height = checkBoxSize;

                    places[j][k].CheckedChanged += Place_CheckedChanged;

                    UpdateSeatColor(j, k);

                    this.Controls.Add(places[j][k]);
                }
            }

            btnConfirmarReserva = new Button();
            btnConfirmarReserva.Text = "Confirmar Reserva";
            btnConfirmarReserva.Size = new Size(120, 30);
            btnConfirmarReserva.Location = new Point(10, 500);
            btnConfirmarReserva.Click += BtnConfirmarReserva_Click;
            this.Controls.Add(btnConfirmarReserva);
        }

        private void ShowReservationScreen()
        {
            this.Controls.Clear();

            InitializePlaces();
        }

        private void BtnFinalizar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnReservar_Click(object obj, EventArgs e)
        {
            ShowReservationScreen();
        }

        private void BtnConfirmarReserva_Click(object sender, EventArgs e)
        {
            SaveReservations();
            this.Controls.Clear();
            InitializeUI();
        }

        private void BtnVoltar_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            InitializeUI();
        }

        private void BtnMapaOcupacao_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();

            int rows = 15;
            int columns = 40;
            int checkBoxSize = 20;
            int spacing = 5;

            for (int j = 0; j < rows; j++)
            {
                for (int k = 0; k < columns; k++)
                {
                    CheckBox seat = new CheckBox();
                    seat.Width = checkBoxSize;
                    seat.Height = checkBoxSize;
                    seat.Left = k * (checkBoxSize + spacing);
                    seat.Top = j * (checkBoxSize + spacing);

                    seat.Text = "";

                    seat.FlatStyle = FlatStyle.Flat;

                    seat.Checked = places[j][k].Checked;
                    seat.Enabled = false;

                    if (seat.Checked)
                    {
                        seat.BackColor = Color.Red;
                    }
                    else
                    {
                        seat.BackColor = Color.Green;
                    }
                    this.Controls.Add(seat);
                }
            }

            btnVoltar = new Button();
            btnVoltar.Text = "Voltar";
            btnVoltar.Size = new Size(90, 30);
            btnVoltar.Location = new Point(10, 500);
            btnVoltar.Click += BtnVoltar_Click;
            this.Controls.Add(btnVoltar);
        }

        private void BtnFaturamento_Click(object sender, EventArgs e)
        {
            int lugaresOcupados = 0;
            decimal totalFaturamento = 0;

            for (int j = 0; j < places.Length; j++)
            {
                for (int k = 0; k < places[j].Length; k++)
                {
                    if (places[j][k].Checked)
                    {
                        lugaresOcupados++;

                        if (j >= 0 && j <= 4)
                            totalFaturamento += 50;
                        else if (j >= 5 && j <= 9)
                            totalFaturamento += 30;
                        else if (j >= 10 && j <= 14)
                            totalFaturamento += 15;
                    }
                }
            }
            MessageBox.Show($"Qtde de lugares ocupados: {lugaresOcupados}\nValor da bilheteria: R$ {totalFaturamento:F2}", "Faturamento");
        }

        private void Place_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox clickedSeat = sender as CheckBox; ;

            if (clickedSeat != null)
            {
                if (clickedSeat.Checked)
                {
                    clickedSeat.BackColor = Color.Red;
                }
                else
                {
                    clickedSeat.BackColor = Color.Green;
                }
            }
        }

        private void UpdateSeatColor(int row, int seat)
        {
            if (places[row][seat].Checked)
            {
                places[row][seat].BackColor = Color.Red;
            }
            else
            {
                places[row][seat].BackColor = Color.Green;
            }
        }
    }
}