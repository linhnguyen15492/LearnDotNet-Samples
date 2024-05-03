using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class FileLineSource(string fileName) : ILineSource
    {
        private StreamReader? reader;

        public void Close()
        {
            reader?.Close();
        }

        public void Open()
        {
            reader = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read));
        }

        public string? ReadLine()
        {
            if (reader == null)
            {
                throw new InvalidOperationException();
            }

            return reader.ReadLine();
        }
    }
}
