using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;

namespace Winsoft_Assistant
{
    public partial class Form1 : Form
    {
        Timer timer;
        int animationCounter = 0;

        SpeechRecognitionEngine _Recognition = new SpeechRecognitionEngine();

        public Form1()
        {
            InitializeComponent();
            
            timer = new Timer();
            timer.Interval = 2;
            timer.Tick += new EventHandler(animationLoaderTimer);
        }
        private void animationLoaderTimer(object sender, EventArgs args)
        {
            if (animationCounter > 69)
                animationCounter = 0;
            var image = Image.FromFile("C:/Users/Arnyminer Z/Pictures/orangeLoading/frame_" + animationCounter + "_delay-0.02s.gif");
            Loader.Image = image;
            animationCounter++;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Location = new Point(0, 0);
            panel1.Width = Width;

            timer.Start();

            UpdateRecordButtonImage();
        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            label1.ForeColor = Color.FromArgb(200, 200, 200);
        }
        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.ForeColor = Color.FromArgb(255, 255, 255);
        }
        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_MouseHover(object sender, EventArgs e)
        {
            label2.ForeColor = Color.FromArgb(200, 200, 200);
        }
        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.FromArgb(255, 255, 255);
        }
        private void label2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        #region Move Form
        //Global variables;
        private bool _dragging = false;
        private Point _start_point = new Point(0, 0);
        
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;  // _dragging is your variable flag
            _start_point = new Point(e.X, e.Y);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this._start_point.X, p.Y - this._start_point.Y);
            }
        }
        #endregion

        #region Speech reconigzer
        bool mustRecord = true;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            UpdateRecordButtonImage();
        }
        private void pictureBox1_Hover(object sender, EventArgs e)
        {
            if (mustRecord)
                RecordMic.Image = Image.FromFile("C:/Users/Arnyminer Z/Pictures/orangeLoading/no_mic.png");
        }
        private void pictureBox1_Leave(object sender, EventArgs e)
        {
            if (mustRecord)
                RecordMic.Image = Image.FromFile("C:/Users/Arnyminer Z/Pictures/orangeLoading/mic.png");
        }
        private void UpdateRecordButtonImage()
        {
            mustRecord = !mustRecord;
            if(mustRecord)
                RecordMic.Image = Image.FromFile("C:/Users/Arnyminer Z/Pictures/orangeLoading/mic.png");
            else
                RecordMic.Image = Image.FromFile("C:/Users/Arnyminer Z/Pictures/orangeLoading/mic_off.png");

            if (mustRecord)
            {
                _Recognition.SetInputToDefaultAudioDevice();
                _Recognition.LoadGrammar(new DictationGrammar());
                _Recognition.SpeechRecognized += _Recognition_SpeechRecognized;
                _Recognition.RecognizeAsync(RecognizeMode.Multiple);
            }

            RecordMic.BringToFront();
        }
        void _Recognition_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string result = e.Result.Text;
            foreach (RecognizedWordUnit word in e.Result.Words){ }
            ai(result);
        }
        #endregion

        public void ai(string text)
        {

        }

        public class Utility
        {
            /// <summary>
            /// Takes the full name of a resource and loads it in to a stream.
            /// </summary>
            /// <param name="resourceName">Assuming an embedded resource is a file
            /// called info.png and is located in a folder called Resources, it
            /// will be compiled in to the assembly with this fully qualified
            /// name: Full.Assembly.Name.Resources.info.png. That is the string
            /// that you should pass to this method.</param>
            /// <returns></returns>
            public static Stream GetEmbeddedResourceStream(string resourceName)
            {
                return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            }

            /// <summary>
            /// Get the list of all emdedded resources in the assembly.
            /// </summary>
            /// <returns>An array of fully qualified resource names</returns>
            public static string[] GetEmbeddedResourceNames()
            {
                return Assembly.GetExecutingAssembly().GetManifestResourceNames();
            }
        }
    }
}
