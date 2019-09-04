using System;

namespace avitoRequestService
{
     struct Selectors
    {
      internal static string link = "./div[1]/a";
      internal static string name = "*//a[contains(@class,'item-description-title-link')]";
      internal static string price = "*//span[contains(@class,'price')]";
      internal static string dateTime = "*//div[contains(@class,'js-item-date')]";
    }                               
}