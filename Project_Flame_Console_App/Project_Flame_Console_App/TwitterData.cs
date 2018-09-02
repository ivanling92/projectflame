using System;
using Tweetinvi;
using System.Windows.Forms;

namespace Project_Flame_Console_App
{
    class TwitterData
    {
        public string currentTweet { get; private set; }
        public static string Twitterhandle = "ProjectFlame_MY";
        public static int errorcount = 0;

        public void TwitterAPIConnection()
        { 
            //Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET")
            Auth.SetUserCredentials("ZM5LrpTPqVI21QY0ffM4sVDJH", "apDBC6LloyMKNRejTY7pX3Khr7MtWHUctGEDaHuwfDx3t533UQ", "633937647-dcDJk7V47NziRXLgNuXLrQuYAZNP59Jl29Z9bDDK", "aGwMI2TA4Q8aNRxS7Pf3scT5I7wiBcusOckdRzQ2u9J0E");
        }
        //send a tweet
        //var firstTweet = Tweet.PublishTweet("@ivanling_ty #ProjectFlame is born");
        public void GetTweets()
        {
            var userMe = User.GetAuthenticatedUser();

            // AuthenticatedUsers objects differ from User as they contains private credentials that can be used to perform actions on the User account.
            var userFriend = User.GetUserFromScreenName("@"+Twitterhandle); // @ivanling_ty  @ProjectFlame_UK

            //Console.WriteLine(userMe.ScreenName);
            //Console.WriteLine(userFriend); 
            try
            {
                Console.WriteLine(userFriend.ScreenName);
                MyCustomApplicationContext.trayIcon.Text = "Following "+userFriend.ScreenName;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //Please note that only accounts that don't protect their tweets can be accessed
           

            try
            {
                var timeLineTweets = Timeline.GetUserTimeline(userFriend, 1);
                foreach (var timeLineTweet in timeLineTweets)
                {
                    Console.WriteLine(timeLineTweet);
                    MyCustomApplicationContext.trayIcon.Text = "Current tweet: " + timeLineTweet.ToString();
                    currentTweet = timeLineTweet.ToString();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + " error="+errorcount.ToString());
                MyCustomApplicationContext.trayIcon.Text = "Error...Retrying...";
                errorcount++;
                if(errorcount > 30)
                {
                    MessageBox.Show("Retry failed! Please try again!");
                    Application.Exit();
                }
            }
            


            //currentTweet = "I am feeling so Energetic";
        }
    }
}
