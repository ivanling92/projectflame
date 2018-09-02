using Project_Flame_Console_App.Properties;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Project_Flame_Console_App
{
    class Program
    {
        private static BackgroundWorker bw1;
        static bool running = true;
        public static bool user_started = false;
        private static string lastTweet = "";
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.Run(new GUI_flame());
            bw1 = new BackgroundWorker();
            bw1.DoWork += new DoWorkEventHandler(bw1_DoWork);
            bw1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw1_RunWorkerCompleted);
            bw1.RunWorkerAsync();
            Application.Run(new MyCustomApplicationContext());
        }
        private static void bw1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Application.Exit();
        }

        private static void bw1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!user_started)
            {

            }
            MessageBox.Show("Following @" + TwitterData.Twitterhandle);
            int eCount = 0;
            do
            {
                TwitterData twitterData = new TwitterData();
                twitterData.TwitterAPIConnection();
                twitterData.GetTweets();

                if (twitterData.currentTweet != lastTweet)
                {
                    try
                    {
                        Console.WriteLine(twitterData.currentTweet);
                        string output = checkTweet(twitterData.currentTweet.ToLower());
                        Console.WriteLine("Check tweet...");
                        MyCustomApplicationContext.trayIcon.Text = "Checking tweet...";
                        ArduinoData arduinoData = new ArduinoData();
                        arduinoData.sendData(output);
                    }
                    catch (Exception err)
                    {
                        //MessageBox.Show("Error getting tweets from user! Retrying!");
                        System.Threading.Thread.Sleep(3000);
                        eCount++;
                        if (eCount > 5)
                        {
                            MessageBox.Show("Reverting to default user!");
                            TwitterData.Twitterhandle = "projectflame_my";
                        }
                    }

                }
                else
                {
                    Console.WriteLine("currentTweet still == lastTweet");
                    MyCustomApplicationContext.trayIcon.Text = "No new tweets received...Last tweet: "+ twitterData.currentTweet.ToString();
                }
                lastTweet = twitterData.currentTweet;
                System.Threading.Thread.Sleep(2000);

            } while (running);
        }

        static string checkTweet(string tweet)
        {
            if (tweet.Contains("red") || tweet.Contains("angry") || tweet.Contains("manchester") || tweet.Contains("rose") || tweet.Contains("anger") || tweet.Contains("hatred"))
            {
                return "r";
            }
            else if (tweet.Contains("blue") || tweet.Contains("sad") || tweet.Contains("chelsea") || tweet.Contains("sea") || tweet.Contains("ocean"))
            {
                return "b";
            }
            else if (tweet.Contains("green") || tweet.Contains("forest") || tweet.Contains("leaf") || tweet.Contains("jealous") || tweet.Contains("tree") || tweet.Contains("hulk") || tweet.Contains("gamora"))
            {
                return "g";
            }
            else if (tweet.Contains("yellow") || tweet.Contains("pikachu") || tweet.Contains("cheese") || tweet.Contains("gold") || tweet.Contains("sponge") || tweet.Contains("banana"))
            {
                return "y";
            }
            else if (tweet.Contains("white") || tweet.Contains("bright") || tweet.Contains("cloud") || tweet.Contains("paper"))
            {
                return "w";
            }
            else if (tweet.Contains("purple") || tweet.Contains("thanos") || tweet.Contains("junk"))
            {
                return "p";
            }
            else if (tweet.Contains("rainbow") || tweet.Contains("fade") || tweet.Contains("multi"))
            {
                return "x";
            }
            else if (tweet.Contains("disco") || tweet.Contains("random") || tweet.Contains("colourful") || tweet.Contains("beat"))
            {
                return "r";
            }
            else if (tweet.Contains("iron") || tweet.Contains("marvel") || tweet.Contains("flash"))
            {
                return "k";
            }
            else if (tweet.Contains("razor") || tweet.Contains("razer") || tweet.Contains("samsung"))
            {
                return "!";
            }
            else if (tweet.Contains("french") || tweet.Contains("francais") || tweet.Contains("france"))
            {
                return "i";
            }
            else if (tweet.Contains("turn off") || tweet.Contains("dark") || tweet.Contains("black") || tweet.Contains("empty"))
            {
                return "e";
            }
            else
            {
                return "x";
            }
        }

    }
    public class MyCustomApplicationContext : ApplicationContext
    {
        public static NotifyIcon trayIcon;
       
        public static string toolTipStatus = "Running...";
        public MyCustomApplicationContext()
        {
            
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.TrayIcon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Exit", Exit_handler),
                new MenuItem("Change Handle", Change),

            }),
                Visible = true
            };
            trayIcon.Text = "Running!";
            trayIcon.Click += TrayIcon_Click;
            
        }

        private void Exit_handler(object sender, EventArgs e)
        {
            Exit();
        }

        private void TrayIcon_Click(object sender, EventArgs e)
        {
            trayIcon.ShowBalloonTip(1000, "Status", MyCustomApplicationContext.toolTipStatus, ToolTipIcon.Info);
        }

        public static void Exit()
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;
            Application.Exit();
        }

        void Change(object sender, EventArgs e)
        {
            //TwitterData.Twitterhandle = "projectflame_my";
            TwitterData.Twitterhandle = Prompt.ShowDialog("Enter new twitter handle @", "Change Handle");
        }
    }
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 200,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 100 };
            Button confirmation = new Button() { Text = "Ok", Left = 50, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
