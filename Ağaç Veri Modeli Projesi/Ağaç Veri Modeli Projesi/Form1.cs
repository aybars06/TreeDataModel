using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ağaç_Veri_Modeli_Projesi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public class agacyapisi
        {
            public agacyapisi sol;
            public int veri;
            public agacyapisi sag;
        }
        public agacyapisi kok = null;
        int dugumsayisi = 0;
        private void dugumekle_btn_Click(object sender, EventArgs e)
        {
            agacyapisi yeni = new agacyapisi();
            yeni.veri = int.Parse(dugumekletxtBox.Text);
            dugumekletxtBox.Clear();
            //ağaca ekleme bölümü
            if (kok == null)
            {
                kok = yeni;
                kok.sag = null;
                kok.sol = null;
                dugumsayisi++;
                ekleLabel.Text = " " + dugumsayisi.ToString() + ".";
            }
            else
            {
                agacyapisi kaydedilen = kok;
                agacyapisi islenen = kok;
                while (true)
                {
                    kaydedilen = islenen;
                    if (kaydedilen.veri == yeni.veri)
                    {
                        MessageBox.Show("Daha Önceden Eklenen Veri Eklenemez", "HATA");
                        break;
                    }
                    else if (kaydedilen.veri < yeni.veri)
                    {
                        islenen = islenen.sag;
                        if (islenen == null)
                        {
                            kaydedilen.sag = yeni;
                            kaydedilen = kaydedilen.sag;
                            kaydedilen.sag = null;
                            kaydedilen.sol = null;
                            dugumsayisi++;
                            ekleLabel.Text = " " + dugumsayisi.ToString() + ".";
                            return;
                        }

                    }
                    else
                    {
                        islenen = islenen.sol;
                        if (islenen == null)
                        {
                            kaydedilen.sol = yeni;
                            kaydedilen = kaydedilen.sol;
                            kaydedilen.sol = null;
                            kaydedilen.sag = null;
                            dugumsayisi++;
                            ekleLabel.Text = " " + dugumsayisi.ToString() + ".";
                            return;
                        }

                    }


                }

            }

        }

        private void preorderYazdir(agacyapisi yazılacak)
        {
            if (yazılacak == null)
            {
                return;
            }
            preordertxtBox.Text += yazılacak.veri.ToString() + "-->";
            preorderYazdir(yazılacak.sol);
            preorderYazdir(yazılacak.sag);
        }
        private void postorderYazdir(agacyapisi yazılacak)
        {
            if (yazılacak == null)
            {
                return;
            }

            postorderYazdir(yazılacak.sol);
            postorderYazdir(yazılacak.sag);
            postordertxtBox.Text += yazılacak.veri.ToString() + "-->";
        }
        private void inorderYazdir(agacyapisi yazılacak)
        {
            if (yazılacak == null)
            {
                return;
            }

            inorderYazdir(yazılacak.sol);
            inordertxtBox.Text += yazılacak.veri.ToString() + "-->";
            inorderYazdir(yazılacak.sag);

        }
        int dugumsirasi = 0;
        private void dugumYazdir(agacyapisi yazılacak)
        {
            if (yazılacak == null)
            {
                return;
            }
            dugumsirasi++;
            dugumgostertxtBox.Text += dugumsirasi + ".Dugum -->\t" + yazılacak.veri.ToString() + Environment.NewLine;
            dugumYazdir(yazılacak.sol);
            dugumYazdir(yazılacak.sag);
        }
        int aramasayac = 0;
        private void dugumArama(agacyapisi root, int deger)
        {
            if (root == null)
            {
                aramasayac++;
                MessageBox.Show("Aradığınız Değer Ağaç Yapısında Bulunamadı...", "BİLGİ");
                aramasayac = 0;
                return;
            }
            if (deger == root.veri)
            {
                aramasayac++;
                MessageBox.Show("Aradığınız Değer Ağaç Yapısında Bulundu...\n\n\t    (" + aramasayac + ".Düzeyde Bulunuyor)", "BİLGİ");
                aramasayac = 0;
                return;
            }
            else if (deger < root.veri)
            {
                aramasayac++;
                dugumArama(root.sol, deger);
            }
            else
            {
                aramasayac++;
                dugumArama(root.sag, deger);
            }
        }
        private agacyapisi dugumSil(agacyapisi kok, int gelen)
        {
            //silinecek ağaç yok.
            if (kok == null)
            {
                MessageBox.Show("Ağaç Yapısında Bulunmuyor");
                return kok;
            }
            //Altta düğüm olmayandan silme
            if (gelen < kok.veri)     //kökün sürekli sol alt ağaçlarını kontrol et.
            {
                kok.sol = dugumSil(kok.sol, gelen);
            }

            else if (gelen > kok.veri) //kökün sürekli sağ alt ağaçlarını kontrol et.
            {
                kok.sag = dugumSil(kok.sag, gelen);
            }
            else
            {
                // sadece allta tek düğüm olandan silme
                if (kok.sol == null)
                {
                    return kok.sag;
                }
                else if (kok.sag == null)
                {
                    return kok.sol;
                }
                // sadece altta iki düğüm bulunandan silme.
                // sağ alt ağaçtaki min değeri al. 
                kok.veri = minBul(kok.sag);
                kok.sag = dugumSil(kok.sag, kok.veri);
            }

            return kok;
        }
        private void yaprakGoster(agacyapisi agac)
        {
            if (agac == null)
            {
                return;
            }
            if (agac.sag == null && agac.sol == null)
            {
                yapraklartxtBox.Text += agac.veri.ToString() + "-->";
            }

            yaprakGoster(agac.sol);
            yaprakGoster(agac.sag);
        }
        private int yukseklikBul(agacyapisi kok)
        {
            if (kok == null)
            {
                return 0;
            }
            else
            {
                int solYukseklik = yukseklikBul(kok.sol);
                int sagYukseklik = yukseklikBul(kok.sag);
                if (sagYukseklik >= solYukseklik)
                {
                    yuksekliktxtBox.Text = sagYukseklik.ToString();
                    return (sagYukseklik + 1);
                }
                else
                {
                    yuksekliktxtBox.Text = solYukseklik.ToString();
                    return (solYukseklik + 1);
                }
            }
        }
        private int maxBul(agacyapisi aranan)
        {
            int maxdeger = aranan.veri;
            {
                while (true)
                {

                    if (aranan.sag != null)
                    {
                        maxdeger = aranan.sag.veri;
                        aranan = aranan.sag;

                    }
                    return maxdeger;
                }
            }
        }
        private int minBul(agacyapisi aranan)
        {
            int mindeger = aranan.veri;
            {
                while (aranan.sol != null)
                {

                    mindeger = aranan.sol.veri;
                    aranan = aranan.sol;
                }
                return mindeger;
            }
        }
        private void dugumsil_btn_Click(object sender, EventArgs e)
        {
            int silinecek = int.Parse(dugumsiltxtBox.Text);
            dugumSil(kok, silinecek);
        }

        private void dugumbul_btn_Click(object sender, EventArgs e)
        {
            int aranan = int.Parse(dugumduzeytxtBox.Text);
            dugumArama(kok, aranan);

        }

        private void dugumgoster_btn_Click(object sender, EventArgs e)
        {
            dugumgostertxtBox.Text = null;
            dugumsirasi = 0;
            dugumYazdir(kok);
        }

        private void agacbilgileri_btn_Click(object sender, EventArgs e)
        {
            preordertxtBox.Text = null;
            inordertxtBox.Text = null;
            postordertxtBox.Text = null;
            yapraklartxtBox.Text = null;
            preorderYazdir(kok);
            inorderYazdir(kok);
            postorderYazdir(kok);
            dugumsayisitxtBox.Text = dugumsayisi.ToString();
            yaprakGoster(kok);
            yukseklikBul(kok);
        }
    }

}
