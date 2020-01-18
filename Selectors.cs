using System;

namespace avitoRequestService
{
     struct Selectors
    {
      internal static string link = "*//a[contains(@class,'snippet-link')]";
      internal static string name = "*//a[contains(@class,'snippet-link')]";
      internal static string price = "*//span[contains(@class,'price')]";
      internal static string dateTime = "*//div[contains(@class,'js-item-date')]";
    }                               
}