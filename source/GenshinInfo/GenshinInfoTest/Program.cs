using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GenshinInfo.Managers;

namespace GenshinInfoTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GenshinInfoManager manager = new("800608959", "10469721", "H0cQoFWl1ddhLiEnl8toeVjioFbZhkPc3ui9keii");

            Console.WriteLine(await TestLogin(manager));

            Console.WriteLine(await TestDailyNoteSetting(manager));

            //Dictionary<string, string> datas = await TestGetRealTimeNoteData(manager);

            //foreach (var data in datas)
            //{
            //    Console.WriteLine($"{data.Key} : {data.Value}");
            //}
        }

        static async Task<bool> TestLogin(GenshinInfoManager manager)
        {
            return await manager.CheckLogin();
        }

        static async Task<Dictionary<string, string>> TestGetRealTimeNoteData(GenshinInfoManager manager)
        {
            return await manager.GetRealTimeNotes();
        }

        static async Task<bool> TestDailyNoteSetting(GenshinInfoManager manager)
        {
            return await manager.SetDailyNoteSwitch(true);
        }
    }
}
