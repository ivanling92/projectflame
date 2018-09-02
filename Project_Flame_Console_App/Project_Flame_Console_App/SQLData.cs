using System;
using System.Data.SqlClient;

namespace Project_Flame_Console_App
{
    class SQLData : TwitterData
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader myReader;
        bool foundEmotion = false;
        string Emotion;
        public int Red;
        public int Green;
        public int Blue;

        public void ServerConnection()
        {
            // address of local sql.dbo
            con = new SqlConnection(@"server=localhost\SQLEXPRESS;Database=ProjectFLame;Trusted_Connection=True;");
            con.Open();// opens connection
        }
        public void tweetInferredEmotionQuery(string currentTweetLookup)
        {
            ServerConnection();
            try
            {
               /* So using SELECT statement we can match the a keyword in the tweet. 
                  please note that that SELECT ALL FROM table WHERE @phrase LIKE column ...
                  suprised this worked as common practice is...
                  SELECT ALL FROM WHERE column LIKE @phrase - which doesn't work for this case 
                */
                using (cmd = new SqlCommand(@"SELECT * FROM EmotionalLookUp WHERE @currentTweet LIKE '%' + Emotion + '%'", con))
                {                   
                    cmd.Parameters.Add(new SqlParameter("@currentTweet", currentTweetLookup)); // create a new SQL parameter to search with

                    myReader = cmd.ExecuteReader(); // open reader 

                    while (myReader.Read())
                    {
                        //(0)column 1 is PK
                        Emotion = myReader.GetString(1); // column 2
                        Red = myReader.GetInt32(5); // column 6
                        Green = myReader.GetInt32(6); // column 7
                        Blue = myReader.GetInt32(7); // column 8
                        Console.WriteLine("Emotion = {0}, Red = {1}, Green = {2}, Blue = {3}", Emotion, Red, Green, Blue);
                        foundEmotion = true;
                    }
                };
            }
            catch { }

            con.Close(); //close sql.dbo connection

            if (foundEmotion == false) // if no emotion creat a white light
            {
                Console.WriteLine("Emotion = none found, Red = 255, Green = 255, Blue = 255");
                Red = 255; 
                Green = 255;
                Blue = 255;
            }
            else { foundEmotion = false; }

        }

    }
}
