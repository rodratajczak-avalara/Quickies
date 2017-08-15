using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;

namespace ConvertImage2ByteArray
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Image myImage = Image.FromFile(args[0]);
                ImageConverter converter = new ImageConverter();
                byte[] bytes = (byte[])converter.ConvertTo(myImage, typeof(byte[]));

                // Write bytearrray to a text file
                StreamWriter outputFile = new StreamWriter(File.Open(args[0]+".txt.", FileMode.Append));
                //outputFile.Write(System.Text.Encoding.UTF8.GetString(bytes));
                outputFile.Write(JsonConvert.SerializeObject(bytes));
                outputFile.Close();

                Console.WriteLine("Converted to String Successfully");

            }
            catch (FileNotFoundException ioEx)
            {
                Console.WriteLine(ioEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled Exception[" + ex.Message +"]");
            }

        }
    }
}
