using System;
using System.IO.Ports;
using System.Management;

namespace Project_Flame_Console_App
{
    class ArduinoData : SQLData 
    {
        private string AutoDectectArdunioPort() // should alwaysa auto detect the seeedunio if yours has the same name
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort"); // select all connected ports
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            try
            {
                foreach (ManagementObject item in searcher.Get())
                {

                    string desc = item["Description"].ToString(); // get description which is the name of the device
                    string deviceID = item["DeviceID"].ToString(); // gets com port number

                    if (desc.Contains("Silicon Labs CP210x USB to UART Bridge")) // name of seeedunio
                    {
                        Console.WriteLine(deviceID);
                        return deviceID;
                    }
                }
            }
            catch (ManagementException e){}

            return null;
        }

        public void sendData(string command) // send data over serial to arduino
        {
            SerialPort sp = new SerialPort(AutoDectectArdunioPort(), 9600); // return deviceID is comport so (COM3, baudrate)

            if (sp.IsOpen == false) // can't open twice if ardunio has it open already
            {
                int count = 0;
                bool continue_on = true;
                while(count < 100 && continue_on)
                {
                    try
                    {
                        sp.Open();
                        continue_on = false;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error Opening Port");
                        continue_on = true;
                    }
                }
               
            }
            else{ }
            // convert values to string and send over serial to ardunio -- if you change the writeline 
            // you will need to change how the arduino handles the data
            sp.Write(command);
            Console.WriteLine("SENT! "+command);
            MyCustomApplicationContext.toolTipStatus = "SENT!" + command;
        }


    }
}
