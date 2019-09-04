using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
namespace Hafıza_oyunu
{
    public partial class Form_Hafıza : Form
    {
        public Form_Hafıza()
        {
            InitializeComponent();
        }


        public const int WM_NCLBUTTONDOWN = 0xA1;
    public const int HT_CAPTION = 0x2;

    [DllImportAttribute("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd,
        int Msg, int wParam, int lParam);

    [DllImportAttribute("user32.dll")]
    public static extern bool ReleaseCapture();


    enum hafıza
        {
            ilkhafıza,ikincihafıza
        }
        hafıza tıklama = hafıza.ilkhafıza;     


        public void saniyefonk(int süre)
        {
            lbl_süre.Text = süre.ToString();
        }

        

        ArrayList resimliste = new ArrayList();
        void resimler(int süre)
        {
            Random rnd = new Random();
            if (süre > 0)
            {
                for (int i = 0; i < (ımageList1.Images.Count - 1) * 2; i++) resimliste.Add((i % 20) + 1);
                foreach (PictureBox item in panel_arkaplan.Controls)
                {
                    int tıklanan = rnd.Next(resimliste.Count);
                        item.Tag = resimliste[tıklanan];
                        resimliste.RemoveAt(tıklanan);
                }
            }
            else
            {
                pcb_yeniden.Visible = true;
                foreach (PictureBox item in panel_arkaplan.Controls)
                {
                    item.Image = ımageList1.Images[0];
                    item.Enabled = true;
                }
            }
        }
        public void resimgizle()
        {
            foreach (PictureBox item in panel_arkaplan.Controls)
            {
                item.Image = ımageList1.Images[0];
            }
        }
        public void yeniden ()
        {
            lbl_oyuncu1.ForeColor = Color.Maroon;
            lbl_oyuncu2.ForeColor = Color.Black;
            label2.Visible = false;
            label1.ForeColor = Color.Black;
            oyuncu1 = 1;
            oyuncu2 = 0;
            txt_oyuncu1.Text = "0";
            txt_oyuncu2.Text = "0";
            tıklabessaniye = 5;
            süre = 5;
            sanıye.Start();
            resimler(süre);
            hafıza tıklama = hafıza.ilkhafıza;
            resimgizle();
            for (int i = 0; i < (ımageList1.Images.Count - 1) * 2; i++)
            {
                PictureBox pcb = (PictureBox)panel_arkaplan.Controls["pcb_" + (i + 1)];

                pcb.Image = ımageList1.Images[(int)pcb.Tag];
                pcb.Show();
                pcb.Enabled = false;
            }
        }
        public void kazanan(int oyuncu1,int oyuncu2)
        {
            if((oyuncu1==110) |(oyuncu2==110))
            {
                label2.Visible = true;
                
                foreach (PictureBox item in panel_arkaplan.Controls)
                {
                    item.Visible = false;
                }
               
                if (oyuncu2==110)
                {
                    label2.Text = "Kazanan oyuncu 2";   
                }
                lbl_süre.Text = "0";
                tıklamakontrol.Stop();
                Thread.Sleep(500);
                DialogResult sonuc;
                sonuc = MessageBox.Show("Yenioyun ister misiniz?", "Oyun bitti", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (sonuc == DialogResult.No)
                {
                    Application.Exit();
                }
                if (sonuc == DialogResult.Yes)
                {
                    yeniden();
                }
            }
            else if ((oyuncu1 == 100) & (oyuncu2 == 100))
            {
                label2.Visible = true;
                label2.Text = "Oyun berabere bitti ...";
            }
        }
        int süre = 5;
        private void btn_basla_Click(object sender, EventArgs e)
        {
            
        }

        private void Form_Hafıza_Load(object sender, EventArgs e)
        {
            pcb_1.Tag = 1;
            pcb_yeniden.Visible = false;
            //btn_yeniden.Visible = false;
            lbl_oyuncu1.ForeColor = Color.Maroon;
            lbl_oyuncu2.ForeColor = Color.Black;
            foreach (PictureBox item in panel_arkaplan.Controls)
            {
                item.Image = ımageList1.Images[0];
                item.Enabled = false;
            }
        }

        
        int tıklabessaniye = 5;
        PictureBox pcb2;
        int oyuncu1 = 1, oyuncu2 = 0;

        private void btn_yeniden_Click(object sender, EventArgs e)
        {
            yeniden();
        }
        private void pcb_1_Click(object sender, EventArgs e)
        {
            label1.Text = ":Seçim süreniz:";
            label1.ForeColor = Color.Maroon;
            lbl_süre.ForeColor = Color.Maroon;
            PictureBox pcb = (PictureBox)sender;
            pcb.Image = ımageList1.Images[(int)pcb.Tag];
            panel_arkaplan.Refresh();
            if (pcb2==pcb)
            {
                tıklamakontrol.Stop();
                MessageBox.Show("ikincikez tıklama yapılamaz");
                tıklamakontrol.Start();
                return;
            }
            switch (tıklama)
            {
                case hafıza.ilkhafıza:
                    pcb2 = pcb;
                    tıklama = hafıza.ikincihafıza;
                    tıklabessaniye = 5;
                    saniyefonk(5);
                    tıklamakontrol.Start();
                    break;
                case hafıza.ikincihafıza:
                    Thread.Sleep(500);

                    if (pcb.Tag.ToString()==pcb2.Tag.ToString())
                    {
                        if (oyuncu1==1)
                        {
                            txt_oyuncu1.Text = (Convert.ToInt32(txt_oyuncu1.Text)+10).ToString();
                        }
                        else
                        {
                            txt_oyuncu2.Text = (Convert.ToInt32(txt_oyuncu2.Text) + 10).ToString();
                        }
                        
                        pcb.Hide();
                        pcb2.Hide();
                        kazanan(Convert.ToInt32(txt_oyuncu1.Text), Convert.ToInt32(txt_oyuncu2.Text));
                        saniyefonk(5);
                        tıklamakontrol.Stop();
                    }
                    else
                    {
                        if (oyuncu1==1)
                        {
                            lbl_oyuncu2.ForeColor = Color.Maroon;
                            lbl_oyuncu1.ForeColor = Color.Black;
                            oyuncu1 = 0;
                            oyuncu2 = 1;
                        }
                        else
                        {
                            lbl_oyuncu1.ForeColor = Color.Maroon;
                            lbl_oyuncu2.ForeColor = Color.Black;
                            oyuncu1 = 1;
                            oyuncu2 = 0;
                        }
                        resimgizle();
                        saniyefonk(5);
                        tıklamakontrol.Stop();
                    }
                    tıklama = hafıza.ilkhafıza;
                    pcb2 = null;
                    break;
            }

        }
        private void tıklamakontrol_Tick(object sender, EventArgs e)
        {
            
            tıklabessaniye -=1;
            if (tıklabessaniye <= 0)
            {
                if (oyuncu1 == 1)
                {
                    lbl_oyuncu2.ForeColor = Color.Maroon;
                    lbl_oyuncu1.ForeColor = Color.Black;
                    oyuncu1 = 0;
                    oyuncu2 = 1;
                }
                else
                {
                    lbl_oyuncu1.ForeColor = Color.Maroon;
                    lbl_oyuncu2.ForeColor = Color.Black;
                    oyuncu1 = 1;
                    oyuncu2 = 0;
                }
                pcb2 = null;
                lbl_süre.Text = "0";
                tıklamakontrol.Stop();
                resimgizle();
                tıklama = hafıza.ilkhafıza;

            }
            else
            {
                lbl_süre.Text = "5";
                saniyefonk(tıklabessaniye);
            }
        }
       
        private void Form_Hafıza_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void Form_Hafıza_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pcb_başla_Click(object sender, EventArgs e)
        {
            süre = 5;
            sanıye.Start();
            resimler(süre);
            for (int i = 0; i < (ımageList1.Images.Count - 1) * 2; i++)
            {
                PictureBox pcb = (PictureBox)panel_arkaplan.Controls["pcb_" + (i + 1)];

                pcb.Image = ımageList1.Images[(int)pcb.Tag];

                pcb.Show();
            }
            pcb_başla.Visible = false;
        }

        private void pcb_yeniden_Click(object sender, EventArgs e)
        {
            yeniden();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            süre -= 1;

            if (süre == 0)
            {
                lbl_süre.Text =süre.ToString();
                sanıye.Stop();
                saniyefonk(süre);
                resimler(süre);
            }
            else
            {
                saniyefonk(süre);
            }
        }
    }
}
