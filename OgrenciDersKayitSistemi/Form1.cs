using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace OgrenciDersKayitSistemi
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		SqlConnection baglanti = new SqlConnection(@"Data Source=emin\SQLEXPRESS;Initial Catalog=EtutDb;Integrated Security=True;Encrypt=False");

		void dersListesi()
		{
			SqlDataAdapter da = new SqlDataAdapter("Select * From tbldersler", baglanti);
			DataTable dt = new DataTable();
			da.Fill(dt);
			CmbDers.ValueMember = "DERSID";
			CmbDers.DisplayMember = "DERSAD";
			CmbDers.DataSource = dt;
		}

		//Etüt listesi
		void etutListesi()
		{
			SqlDataAdapter da3 = new SqlDataAdapter("execute etut", baglanti);
			DataTable dt3 = new DataTable();
			da3.Fill(dt3);
			dataGridView1.DataSource = dt3;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			dataGridView1.CellClick += dataGridView1_CellClick;
			dersListesi();
			etutListesi();
		}

		private void CmbDers_SelectedIndexChanged(object sender, EventArgs e)
		{
			SqlDataAdapter da2 = new SqlDataAdapter("Select * from tblogretmen where bransıd=" + CmbDers.SelectedValue, baglanti);
			DataTable dt2 = new DataTable();
			da2.Fill(dt2);
			CmbOgretmen.ValueMember = "OGRTID";
			CmbOgretmen.DisplayMember = "AD";
			CmbOgretmen.DataSource = dt2;
		}

		private void BtnEtutOlustur_Click(object sender, EventArgs e)
		{
			baglanti.Open();
			SqlCommand komut = new SqlCommand("insert into tbletut (dersıd, ogretmenıd, tarıh, saat) values (@p1,@p2,@p3,@p4)", baglanti);
			komut.Parameters.AddWithValue("@p1", CmbDers.SelectedValue);
			komut.Parameters.AddWithValue("@p2", CmbOgretmen.SelectedValue);
			komut.Parameters.AddWithValue("@p3", MskTarih.Text);
			komut.Parameters.AddWithValue("@p4", MskSaat.Text);
			komut.ExecuteNonQuery();
			baglanti.Close();
			MessageBox.Show("Etüt Oluşturuldu", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

		}

		private void dataGridView1_CellClick(object sender, EventArgs e)
		{
			int secilen = dataGridView1.SelectedCells[0].RowIndex;

			TxtEtutId.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
		}

		private void BtnEtutVer_Click(object sender, EventArgs e)
		{
			baglanti.Open();
			SqlCommand komut = new SqlCommand("Update tbletut set ogrencııd=@p1,durum=@p2 where ıd=@p3", baglanti);
			komut.Parameters.AddWithValue("@p1", TxtOgrenci.Text);
			komut.Parameters.AddWithValue("@p2", "True");
			komut.Parameters.AddWithValue("@p3", TxtEtutId.Text);
			komut.ExecuteNonQuery();
			baglanti.Close();
			MessageBox.Show("Etüt öğrenciye Verildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		private void BtnFoto_Click(object sender, EventArgs e)
		{
			openFileDialog1.ShowDialog();
			pictureBox1.ImageLocation = openFileDialog1.FileName;
		}

		private void BtnOgrenciEkle_Click(object sender, EventArgs e)
		{
			baglanti.Open();
			SqlCommand komut = new SqlCommand("insert into tblogrencı (ad,soyad,fotograf,sınıf,telefon,maıl) values (@p1,@p2,@p3,@p4,@p5,@p6)", baglanti);
			komut.Parameters.AddWithValue("@p1", TxtAd.Text);
			komut.Parameters.AddWithValue("@p2", TxtSoyad.Text);
			komut.Parameters.AddWithValue("@p3", pictureBox1.ImageLocation);
			komut.Parameters.AddWithValue("@p4", TxtSinif.Text);
			komut.Parameters.AddWithValue("@p5", MskTel.Text);
			komut.Parameters.AddWithValue("@p6", TxtMail.Text);
			komut.ExecuteNonQuery();
			baglanti.Close();
			MessageBox.Show("Öğrenci Sisteme Kaydedildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}
