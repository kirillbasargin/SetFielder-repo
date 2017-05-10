/*
 /input "E:\Users\haxoid\Documents\visual studio 2010\Projects\SetFielder\SetFielder\bin\Debug\input.pdf" 
 * /output "E:\Users\haxoid\Documents\visual studio 2010\Projects\SetFielder\SetFielder\bin\Debug\output.pdf" 
 * /parameters "name.0:Тест Тест Тест | zaemchik:Yes | name.3:Hello"
 * SetFielded.exe /input "C:\Users\haxoid\Desktop\input.pdf" /output "C:\Users\haxoid\Desktop\output.pdf" /parameters "name.0:Тест1 Тест1 Тест1 | zaemchik:Yes | name.3:Hello"
 * ilmerge /target:winexe /out:SelfContainedProgram.exe SetFielder.exe itextsharp.dll
 * https://habrahabr.ru/post/112707/
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
//using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SetFielder
{
    class Program
    {
        static void Main(string[] args)
        {          
            //string[] args;
            //args = System.Environment.GetCommandLineArgs();

            Dictionary<string, string> Params = new Dictionary<string, string>();

            string input = "";
            string output = "";
            string parameters = "";

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "/input")
                    if (!String.IsNullOrEmpty(args[i + 1]))
                        input = args[i + 1];
                    else
                        return;
                if (args[i] == "/output")
                    if (!String.IsNullOrEmpty(args[i + 1]))
                        output = args[i + 1];
                    else
                        return;
                if (args[i] == "/parameters")
                    if (!String.IsNullOrEmpty(args[i + 1]))
                        parameters = args[i + 1];
                    else
                        return;               
            }

            char[] delimiterCharsFirst = { '|' }; 
            char[] delimiterCharsSecond = { ':' };

            string[] elements = parameters.Split(delimiterCharsFirst);

            foreach (string element in elements)
            {
                string[] split_parameters = element.Split(delimiterCharsSecond);
                if (split_parameters.Length == 2)
                    Params.Add(split_parameters[0].Trim(), split_parameters[1].Trim());
                else
                    return;
            }
            
            try
            {
                var doc = new Document();
                PdfReader template = new PdfReader(input); //Application.StartupPath + @"\input.pdf"
                PdfStamper stamper = new PdfStamper(template, new FileStream(output, FileMode.OpenOrCreate)); //Application.StartupPath + @"\output.pdf"
                AcroFields fields = stamper.AcroFields;

                if (fields.Fields.Count < Params.Count)
                    return;

                BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                fields.AddSubstitutionFont(baseFont);

                foreach (KeyValuePair<string, string> Par in Params)
                try
                {
                    fields.SetField(Par.Key, Par.Value);
                }
                catch
                {
                    continue;
                }

                stamper.FormFlattening = false;
                stamper.Close();
            }
            catch
            {
                return;
            }
        }
    }
}
