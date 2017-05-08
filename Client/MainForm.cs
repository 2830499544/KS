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
using Newtonsoft.Json;
using System.Net;

namespace Client
{
    public partial class MainForm : Form
    {
        HttpClientHandler m_Handler = null; 
        HttpClient m_HttpClient = null;
        public MainForm()
        {
            InitializeComponent();
        }

        void init()
        {
            m_Handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            m_HttpClient = new HttpClient(m_Handler);
            m_HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));
            m_HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            m_HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
            m_HttpClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("Chrome", "56.0.2924.76"));
        }

        private async void button_Login_Click(object sender, EventArgs e)
        {
            string vUrl = string.Format("http://learn.open.com.cn/Account/UnitLogin?t=0.08834811193444936&loginName={0}&passWord={1}&validateNum={2}&rememberMe=0",textBox1.Text,textBox2.Text,textBox3.Text);
            string body = await m_HttpClient.GetStringAsync(vUrl);
            MessageBox.Show(body);
            var vCookies = m_Handler.CookieContainer.GetCookies(new Uri("http://learn.open.com.cn"));
            LoginJsonEF vLoginState = JsonConvert.DeserializeObject<LoginJsonEF>(body);
            if (vLoginState.status == 0 )
            {
                string vbody = await m_HttpClient.GetStringAsync(@"http://learn.open.com.cn/StudentCenter/MyWork/GetOnlineJsonAll");
                //textBox4.Text = vbody;
                WorkJsonEF vWorkData = JsonConvert.DeserializeObject<WorkJsonEF>(vbody);
                if ( vWorkData.status == 0 )
                {
                    string vResult = "";
                    foreach (var vTempWork in vWorkData.data)
                    {
                        vResult += vTempWork.CourseName+"\r\n";
                        foreach( var vTempWorkDetail in vTempWork.Data)
                        {
                            //vResult += string.Format("作业名称{0} 起止时间{1}-{2} 提交次数{3}/{4} 最高分{5} \r\n",vTempWorkDetail.ExerciseName,
                            //    vTempWorkDetail.StartDate, vTempWorkDetail.EndDate, vTempWorkDetail.MaxTimesOfTrying,vTempWorkDetail.SubmitCount, vTempWorkDetail.MaxScore);
                            ListViewItem vNewItem = new ListViewItem()
                            {
                                Text = vTempWorkDetail.CourseExerciseID.ToString(),
                            };
                            vNewItem.SubItems.Add(vTempWorkDetail.ExerciseName);
                            vNewItem.SubItems.Add(string.Format("{0}--{1}", vTempWorkDetail.StartDate, vTempWorkDetail.EndDate));
                            vNewItem.SubItems.Add(string.Format("{0}/{1}", vTempWorkDetail.MaxTimesOfTrying, vTempWorkDetail.SubmitCount));
                            vNewItem.SubItems.Add(vTempWorkDetail.MaxScore.ToString());
                            listView1.Items.Add(vNewItem);
                        }
                    }
                    //textBox4.Text = vResult;
                }
            }
        }

        async Task<string> getWebValidatePic()
        {
            string body = await m_HttpClient.GetStringAsync(@"http://learn.open.com.cn/");
            HtmlAgilityPack.HtmlDocument vHtmlDocument = new HtmlAgilityPack.HtmlDocument();
            vHtmlDocument.LoadHtml(body);
            
            //m_HttpClient.DefaultRequestHeaders.
            string vValidatePic = vHtmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"img_Validate\"]").Attributes["src"].Value;
            vValidatePic = vValidatePic.Substring(0,vValidatePic.IndexOf('?'));
            vValidatePic = string.Format("{0}?token=1c595904-0272-4295-80ea-4a330b2bd0a8", vValidatePic);
            return vValidatePic;
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            string vValidatePic = await getWebValidatePic();
            string vUrl = string.Format("http://learn.open.com.cn/{0}", vValidatePic);
            HttpResponseMessage response = m_HttpClient.GetAsync(new Uri(vUrl)).Result;
            var vPic = await response.Content.ReadAsByteArrayAsync();
            var vFile = System.IO.File.Create(@"C:\1.jpg");
            vFile.Write(vPic, 0, vPic.Length);
            vFile.Flush();
            vFile.Close();
            pictureBox1.Image = null;
            pictureBox1.WaitOnLoad = false; //设置为异步加载图片  
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            //pictureBox1.ImageLocation= (string.Format("http://learn.open.com.cn/{0}", vValidatePic));
            pictureBox1.ImageLocation = (@"C:\1.jpg");
        }

        private async void button_Post_Click(object sender, EventArgs e)
        {
            var content = new FormUrlEncodedContent(new[]
          {
                new KeyValuePair<string, string>("", "login")
            });
            m_Handler.CookieContainer.SetCookies(new Uri("http://wenjuan.openonline.com.cn"), "UserId=hguowen1609,UserName=%e9%bb%84%e5%9b%bd%e6%96%87");
            //vCookies.Add(new Cookie("UserId", "qianyh1609"));
            //vCookies.Add(new Cookie("UserName", "%E9%92%B1%E7%8E%89%E7%BA%A2"));

            var vResponse = await m_HttpClient.PostAsync("http://wenjuan.openonline.com.cn/Home/FormResult", content);
            //string[] strCookies = vResponse.Headers.GetValues("Set-Cookie").ToArray(); ;
            var vHtml = vResponse.Content.ReadAsStringAsync();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            init();
        }
    }
}
