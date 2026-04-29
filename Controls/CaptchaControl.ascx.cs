using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assignment6
{
    public partial class CaptchaControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateCaptcha();
            }
        }

        protected void GenerateCaptcha()
        {
            string captchaText = NewCaptchaText();
            Session["CurrentCaptcha"] = captchaText;

            Bitmap bmap = new Bitmap(495, 80);
            Random rand = new Random();

            using (Graphics gr = Graphics.FromImage(bmap))
            using (MemoryStream mstream = new MemoryStream())
            {
                gr.Clear(Color.White);
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                Font font = new Font("Arial", 48, FontStyle.Bold);
                float currX = 10;
                foreach (char c in captchaText)
                {
                    SizeF charSize = gr.MeasureString(c.ToString(), font);

                    GraphicsState grState = gr.Save();
                    float shiftY = rand.Next(5, 15);
                    gr.TranslateTransform(currX + (charSize.Width / 2), shiftY + (charSize.Height / 2));

                    gr.RotateTransform(rand.Next(-30, 31));
                    gr.DrawString(c.ToString(), font, Brushes.Black, -charSize.Width / 2, -charSize.Height / 2);

                    gr.Restore(grState);
                    int temp = (int)charSize.Width;
                    float shiftX = rand.Next((temp / 2), temp);
                    currX += shiftX;

                }
                bmap.Save(mstream, ImageFormat.Png);
                imgCaptcha.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(mstream.ToArray());
            }
        }
        protected string NewCaptchaText()
        {
            Random r = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            string code = "";
            for (int i = 0; i < 10; i++)
            {
                code += chars[r.Next(chars.Length)];
            }
            return code;
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            if (txtInput.Text == (string)Session["CurrentCaptcha"])
            {
                lblStatus.Text = "Verifaction Successful";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblStatus.Text = "Invalid Verification. Try new code";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                GenerateCaptcha();
                txtInput.Text = "";
            }

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            GenerateCaptcha();
        }
    }
}


