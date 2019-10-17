using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;


namespace FTPuploadTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void TestBtn_Click(object sender, EventArgs e)
        {
            string testFile = Application.StartupPath + @"\print.py";
            Console.WriteLine(testFile);
            

            bool status = false;
            
             //连接共享文件夹
             status = FileShare.connectState(@"\\172.24.117.129\e", "Administrator", "user_te");
             if (status)
             {
                Console.WriteLine("connect server successful!");
                DirectoryInfo theFolder = new DirectoryInfo(@"\\172.24.117.129\e\10.Weidong.Cao\FTPbackup");
                
                //先测试读文件，把目录路径与文件名连接
                 string filename = theFolder.ToString() + "\\good.txt";
                 FileShare.ReadFiles(filename);
                
                //测试写文件，拼出完整的路径
                 filename = theFolder.ToString() + "\\bad.txt";
                 FileShare.WriteFiles(filename);
                
                                 //遍历共享文件夹，把共享文件夹下的文件列表列到listbox
                 foreach (FileInfo nextFile in theFolder.GetFiles())
                 {
                   //ListBox1.Items.Add(nextFile.Name);
                 }
             }
             else
             {
                //ListBox1.Items.Add("未能连接！");
                Console.WriteLine("connect server fail!");
             }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }

    public class FileShare
      {
          public FileShare() { }
  
          public static bool connectState(string path)
          {
              return connectState(path,"","");
          }
  
          public static bool connectState(string path, string userName, string passWord)
           {
              bool Flag = false;
              Process proc = new Process();
              try
              {
                  proc.StartInfo.FileName = "cmd.exe";
                  proc.StartInfo.UseShellExecute = false;
                  proc.StartInfo.RedirectStandardInput = true;
                  proc.StartInfo.RedirectStandardOutput=true;
                  proc.StartInfo.RedirectStandardError=true;
                  proc.StartInfo.CreateNoWindow=true;
                  proc.Start();
                  string dosLine = @"net use " + path + " /User:" + userName + " " + passWord + " /PERSISTENT:YES";
                  proc.StandardInput.WriteLine(dosLine);
                  proc.StandardInput.WriteLine("exit");
                  while (!proc.HasExited)
                  {
                      proc.WaitForExit(1000);
                  }
                  string errormsg = proc.StandardError.ReadToEnd();
                  proc.StandardError.Close();
                  if (string.IsNullOrEmpty(errormsg))
                  {
                      Flag = true;
                  }
                  else
                  {
                      MessageBox.Show(errormsg);
                      //throw new Exception(errormsg);
                  }
              }
              catch (Exception ex)
              {
                   MessageBox.Show(ex.ToString());
                  //throw ex;
              }
             finally
              {
                  proc.Close();
                  proc.Dispose();
              }
              return Flag;
          }
  
  
          //read file
          public static void ReadFiles(string path)
          {
              try
              {
                  // Create an instance of StreamReader to read from a file.
                  // The using statement also closes the StreamReader.
                  using (StreamReader sr = new StreamReader(path))
                  {
                      String line;
                      // Read and display lines from the file until the end of 
                      // the file is reached.
                      while ((line = sr.ReadLine()) != null)
                      {
                          Console.WriteLine(line);
                          
                      }
                  }
              }
              catch (Exception e)
              {
                  // Let the user know what went wrong.
                  Console.WriteLine("The file could not be read:");
                  Console.WriteLine(e.Message);
                  MessageBox.Show("The file could not be read:" + e.Message);
              } 
  
          }
  
          //write file
          public static void WriteFiles(string path)
          {
              try
             {
                 // Create an instance of StreamWriter to write text to a file.
                 // The using statement also closes the StreamWriter.
                 using (StreamWriter sw = new StreamWriter(path))
                 {
                     // Add some text to the file.
                     sw.Write("This is the ");
                     sw.WriteLine("header for the file.");
                     sw.WriteLine("-------------------");
                     // Arbitrary objects can also be written to the file.
                     sw.Write("The date is: ");
                     sw.WriteLine(DateTime.Now);
                 }
             }
             catch (Exception e)
             {
                 // Let the user know what went wrong.
                 Console.WriteLine("The file could not be read:");
                 Console.WriteLine(e.Message);
             }
         }
     }
}
