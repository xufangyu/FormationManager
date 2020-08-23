using FormationManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            // 读取阵型文件中的阵型信息
            FormationFileLoad.Load();
            //FormationFileLoad.Write();
        }
    }
}
