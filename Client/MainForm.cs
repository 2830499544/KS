using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

namespace Client
{
    public partial class MainForm : Form
    {
        HttpClient m_HttpClient = new HttpClient();
        public MainForm()
        {
            InitializeComponent();
        }

        void init()
        {
            m_HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));
            m_HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            m_HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
            m_HttpClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("Chrome", "56.0.2924.76"));
        }

        private async void button_Login_Click(object sender, EventArgs e)
        {
            string vUrl = string.Format("http://learn.open.com.cn/Account/UnitLogin?t=0.08834811193444936&loginName={0}&passWord={1}&validateNum={2}&rememberMe=0",textBox1,textBox2,textBox3);
            string body = await m_HttpClient.GetStringAsync(vUrl);
            MessageBox.Show(body);
        }

        async Task<string> getWebValidatePic()
        {
            string body = await m_HttpClient.GetStringAsync(@"http://learn.open.com.cn/");
            HtmlAgilityPack.HtmlDocument vHtmlDocument = new HtmlAgilityPack.HtmlDocument();
            vHtmlDocument.LoadHtml(body);
            
            //m_HttpClient.DefaultRequestHeaders.
            string vValidatePic = vHtmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"img_Validate\"]").Attributes["src"].Value;
            vValidatePic = vValidatePic.Substring(0,vValidatePic.IndexOf('?'));
            vValidatePic = string.Format("{0}?0.17864648687671529", vValidatePic);
            return vValidatePic;
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            string vValidatePic = await getWebValidatePic();
            pictureBox1.Image = null;
            pictureBox1.WaitOnLoad = false; //设置为异步加载图片  
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.LoadAsync(string.Format("http://learn.open.com.cn/{0}", vValidatePic));
        }
    }
}
