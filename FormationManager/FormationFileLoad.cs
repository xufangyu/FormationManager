using FormationManager.Bean;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace FormationManager
{
    // 所有阵型信息
    public class FormationFileLoad
    {
        // 实例
        public static FormationFileLoad Instance { get; private set; }
        // 加载的数据
        public static bool isLoad = false;
        public static List<FormationInfo> fiList;
        public static int Size;

        public static List<string> nameList = new List<string>();
        public static List<string> nameCNList = new List<string>();

        public static int getIndexForName(string name)
        {
            int indexNum = nameList.ToList().IndexOf(name);
            return indexNum;
        }


        //public static bool Load()
        //{
        //JsonTextReader reader = new JsonTextReader(new StringReader(sBuilder.ToString()));
        //while (reader.Read())
        //{
        //    if (reader.Value != null)
        //    {
        //        //Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
        //        Console.WriteLine(reader.TokenType + "\t\t" + reader.ValueType + "\t\t" + reader.Value);
        //    }
        //    else
        //    {
        //        Console.WriteLine("Token: {0}", reader.TokenType);
        //    }
        //}
        //using (System.IO.StreamReader file = System.IO.File.OpenText(FormationInfoFilePath))
        //{
        //    using (JsonTextReader reader = new JsonTextReader(file))
        //    {
        //        JToken jToken = JToken.ReadFrom(reader);
        //        JObject o = (JObject)JToken.ReadFrom(reader);
        //        var value = o["Name"].ToString();
        //        System.Console.WriteLine("Hello World!");
        //        System.Console.WriteLine($"CurrentDirectory{Environment.CurrentDirectory}");
        //    }
        //}
        //    try
        //    {
        //        if (!isLoad)
        //        {
        //            // 加载文件

        //            isLoad = FormationFileLoad.Load();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        public static bool Load()
        {
            if (isLoad)
            {
                // 加载文件
                return true;
            }
            //try
            //{
            //    Assembly assembly = Assembly.LoadFrom(@"E:\SteamLibrary\steamapps\common\FromJianghu\Mods\FormationManager\System.Numerics.dll");
            //    AssemblyName[] arr = assembly.GetReferencedAssemblies();
            //    LoadAssembly(arr);
            //}
            //catch (Exception e)
            //{
            //    //FormationMod.logger.Log($"LoadAssembly error:{e}");
            //    Console.WriteLine($"LoadAssembly error:{e}");
            //}
            try
            {
                // .../common/FromJianghu/FromJianghu_Data
                //FormationMod.logger.Log($"dataPath:{Application.dataPath}");
                //// .../AppData/LocalLow/Gamexel/FromJianghu
                //FormationMod.logger.Log($"persistentDataPath:{Application.persistentDataPath}");
                //// .../common/FromJianghu
                //FormationMod.logger.Log($"CurrentDirectory{Environment.CurrentDirectory}");
                Console.WriteLine($"CurrentDirectory{Environment.CurrentDirectory}");
                string FormationInfoFilePath = "F:/workspace/C#/Projects/FormationManager0.1/ConsoleApplication1/bin/Release/Formation.json";
                //string FormationInfoFilePath = Environment.CurrentDirectory + "/Mods/FormationManager/Formation.json";
                StreamReader sReader = File.OpenText(FormationInfoFilePath);
                StringBuilder sBuilder = new StringBuilder();
                string input = null;
                while ((input = sReader.ReadLine()) != null)
                {
                    sBuilder.Append(input);
                }
                //FormationInfo fi = JsonConvert.DeserializeObject<FormationInfo>(sBuilder.ToString());
                fiList = JsonConvert.DeserializeObject<List<FormationInfo>>(sBuilder.ToString());
                // 获得大小
                Size = fiList.Count();
                //FormationMod.logger.Log($"Size:{Size}");
                // 获取阵法名称列表
                nameList = new List<string>();
                foreach (var item in fiList)
                {
                    nameList.Add(item.Name);
                    nameCNList.Add(item.NameCN);
                }

            }
            catch (Exception e)
            {
                //Console.WriteLine($"Exception{e}");
                FormationMod.logger.Log($"Exception:{e}");
                return false;
            }
            //Console.WriteLine("Token: {0}", fiList[0].Positions[0].Buffs[0].BuffType);
            //Console.WriteLine("Token: {0}", fiList[0].Positions[0].Buffs[0].Value);

            return true;
        }

        public static bool Write()
        {
            //StringWriter sw = new StringWriter();
            //JsonWriter writer = new JsonTextWriter(sw);

            FormationInfo fi = new FormationInfo();
            // position1
            FormationPosition fp1 = new FormationPosition();
            fp1.Buffs = new List<PositionBuff>();
            fp1.Buffs.Add(new PositionBuff(CharacterPropertyType.Atk, "50"));
            fp1.Buffs.Add(new PositionBuff(CharacterPropertyType.RecieveDamage, "40"));
            // position1
            FormationPosition fp2 = new FormationPosition();
            fp2.Buffs = new List<PositionBuff>();
            fp2.Buffs.Add(new PositionBuff(CharacterPropertyType.Atk, "50"));
            fp2.Buffs.Add(new PositionBuff(CharacterPropertyType.RecieveDamage, "40"));
            // position1
            FormationPosition fp3 = new FormationPosition();
            fp3.Buffs = new List<PositionBuff>();
            fp3.Buffs.Add(new PositionBuff(CharacterPropertyType.Atk, "50"));
            fp3.Buffs.Add(new PositionBuff(CharacterPropertyType.RecieveDamage, "40"));
            // position1
            FormationPosition fp4 = new FormationPosition();
            fp4.Buffs = new List<PositionBuff>();
            fp4.Buffs.Add(new PositionBuff(CharacterPropertyType.Atk, "50"));
            fp4.Buffs.Add(new PositionBuff(CharacterPropertyType.RecieveDamage, "40"));
            // position1
            FormationPosition fp5 = new FormationPosition();
            fp5.Buffs = new List<PositionBuff>();
            fp5.Buffs.Add(new PositionBuff(CharacterPropertyType.Atk, "50"));
            fp5.Buffs.Add(new PositionBuff(CharacterPropertyType.RecieveDamage, "40"));


            fi.Name = "zhenfa1";
            fi.Positions = new List<FormationPosition>();
            fi.Positions.Add(fp1);
            fi.Positions.Add(fp2);
            fi.Positions.Add(fp3);
            fi.Positions.Add(fp4);
            fi.Positions.Add(fp5);



            Console.WriteLine(JsonConvert.SerializeObject(new List<FormationInfo>() { fi }, Formatting.Indented));
            //Console.WriteLine(JsonConvert.SerializeObject(new List<FormationInfo>() { fi }));


            return false;
        }

        private static void LoadAssembly(AssemblyName[] arr)
        {
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<string> names = new List<string>();
            foreach (Assembly assem in loadedAssemblies)
            {
                names.Add(assem.FullName);
            }

            foreach (AssemblyName aname in arr)
            {
                if (!names.Contains(aname.FullName))
                {
                    try
                    {
                        Console.WriteLine($"aname.Name:{aname.Name}");
                        Assembly loadedAssembly = Assembly.LoadFrom(@"E:\SteamLibrary\steamapps\common\FromJianghu\Mods\FormationManager\" + aname.Name + ".dll");
                        AssemblyName[] referencedAssemblies = loadedAssembly.GetReferencedAssemblies();
                        LoadAssembly(referencedAssemblies);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception{ex}");
                        continue;
                    }
                }
            }
        }
    }
}
