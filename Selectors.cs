using System;

namespace avitoRequestService
{
     struct Selectors
    {
      internal static string link = "/html[1]/body[1]/div[4]/div[1]/div[5]/div[2]/div[2]/div[1]/div[2]/div[1]/a";
      internal static string name = "/html[1]/body[1]/div[4]/div[1]/div[5]/div[2]/div[2]/div[1]/div[2]/div[1]/a";
      internal static string price = "*//span[contains(@class,'price')]";
      internal static string dateTime = "/html[1]/body[1]/div[4]/div[1]/div[5]/div[2]/div[2]/div[1]/div[2]/div[2]/div[1]/div[3]/div[1]";
    }
}